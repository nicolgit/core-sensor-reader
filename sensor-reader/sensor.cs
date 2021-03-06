using System;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using core_sensor_reader;
using Microsoft.Azure.Cosmos.Table;

public class sensor
{

    private SerialPort serialPort;
    private byte[] slice = new byte[10];
    private NumberFormatInfo numberProvider = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = ",",
            NumberGroupSizes = new int[] { 3 }
        };
        

    public bool Verbose { get; set; }
    public string TableStorageConnectionString { get; set; }
    public string TableStorageTable { get; set; }
    public string Port { get; set; }
    public int Sampling { get; set; }
    public string LocationName { get; set; }

    public double PM25 { 
        get
        {
            return (((double)slice[3] *256d) + (double)slice[2])/10d;
        }
    }
    public double PM10
    { 
        get
        {
            return (((double)slice[5]*256d) + (double)slice[4])/10d;
        }
    }

    public int Humidity { get; set;}

    public double Temperature {get; set;}
    
    public double? Pressure => null;
    
    public async Task ReadStream ()
    {
        serialPort = new SerialPort(Port);
  
        if (Verbose) 
        {
            Console.WriteLine($"Port {Port}");     
            Console.WriteLine($"Connection String {TableStorageConnectionString}"); 
            Console.WriteLine($"Table {TableStorageTable}");
            Console.WriteLine($"Sampling {Sampling} sec");
            Console.WriteLine($"Location name {LocationName}");
        }
        
        serialPort.ReadTimeout = 1500;             
        serialPort.WriteTimeout = 1500;
        serialPort.Open();
        
        if (Verbose) Console.WriteLine($"Serial Port -{Port}- Opened successfully.");
        int b;
        while (true)
        {
            // read data from PM sensor
            while ((b = serialPort.ReadByte()) != 0xAA)
            ;

            slice[0] = (byte)b;
            if ((b = serialPort.ReadByte()) != 0xC0)
                break;
                        
            slice[1] = (byte)b;

            for (int i = 2; i<10; i++)
            {
                slice[i] = (byte)serialPort.ReadByte();
            }
            
            // read data from temp/Hum sensor

            RunCmd c = new RunCmd();
            var output = c.Run("/usr/bin/python3", "DHT11-reader.py", "" );
            
            // Text FORMAT - DHT11|Temp|22.0|C|UMID|69.0|%
            if (Verbose) Console.WriteLine (output);
            var array = output.Split('|');

            if (array[0] == "DHT11" && array[1] == "Temp" && array[4]=="UMID")
            {
                Humidity = (int)Convert.ToDouble(array[5], numberProvider);
                Temperature = Convert.ToDouble(array[2], numberProvider);
            }   
            else
            {
                Humidity = -100;
                Temperature = -100;
                Console.WriteLine ("Wrong DHT11 message format");
            } 

            if (Verbose) dump();

            if (TableStorageConnectionString != null && TableStorageTable != null)
                await WriteDataToCloud();
            
            if (Verbose) Console.WriteLine ($"Waiting {Sampling}secs");
            Thread.Sleep( 1000 * Sampling);
        }     

        // serialPort.Close();                   
    }

    private async Task WriteDataToCloud()
        {
            if (Verbose) Console.Write ("Writing data to cloud... ");

            string storageConnectionString = TableStorageConnectionString;

            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(TableStorageTable);

            await table.CreateIfNotExistsAsync();

            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");

                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrReplace(new StorageRow(LocationName, timestamp)
                {
                    PM10 = this.PM10,
                    PM25 = this.PM25,
                    Humidity = this.Humidity,
                    Pressure = this.Pressure,
                    Temperature = this.Temperature
                });
                await table.ExecuteAsync(insertOrMergeOperation);

                TableOperation insertOrMergeOperation2 = TableOperation.InsertOrReplace(new StorageRow("LAST", LocationName)
                {
                    PM10 = this.PM10,
                    PM25 = this.PM25,
                    Humidity = this.Humidity,
                    Pressure = this.Pressure,
                    Temperature = this.Temperature
                });
                await table.ExecuteAsync(insertOrMergeOperation2);
            }
            catch (StorageException e)
            {
                Console.WriteLine("ERROR Writing Data to cloud" + e.Message);
                throw;
            }

            if (Verbose) Console.WriteLine ("Done.");
        }

    private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided.");
                throw;
            }

            return storageAccount;
        }
    private void dump()
    {
        foreach(byte b in slice)
        {
            Console.Write(Convert.ToString(b, 16).PadLeft(2, '0'));
        }
        Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HHmmss")} PM2.5 {PM25.ToString("00.0")} μg/m3 - PM10 {PM10.ToString("00.0")} μg/m3 - Temperature {Temperature.ToString()} - Humidity {Humidity.ToString()}%");
    }
}