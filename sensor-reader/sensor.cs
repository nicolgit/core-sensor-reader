using System;
using System.IO.Ports;
using System.Threading;
using core_sensor_reader;
using Microsoft.Azure.Cosmos.Table;

public class sensor
{

    private SerialPort serialPort;
    private byte[] slice = new byte[10];

    public bool Verbose { get; set; }
    public string TableStorageConnectionString { get; set; }
    public string TableStorageTable { get; set; }
    public string Port { get; set; }
    public int Sampling { get; set; }

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

    public int Humidity => 50; // sample data
    public double Temperature => 20.1; // sample data
    public double Pressure => 1013.1; // sample data
    
    public void ReadStream ()
    {
        serialPort = new SerialPort(Port);
  
        if (Verbose) 
        {
            Console.WriteLine($"Port {Port}");     
            Console.WriteLine($"Connection String {TableStorageConnectionString}"); 
            Console.WriteLine($"Table {TableStorageTable}");
            Console.WriteLine($"Sampling {Sampling} sec");
        }
        
        serialPort.ReadTimeout = 1500;             
        serialPort.WriteTimeout = 1500;
        serialPort.Open();
        Console.WriteLine($"Serial Port -{Port}- Opened successfully.");
        int b;
        while (true)
        {
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

            if (Verbose) dump();

            if (TableStorageConnectionString != null &&
                TableStorageTable != null)
                WriteDataToCloud();
            
            Thread.Sleep( 1000 * Sampling);
        }     

        // serialPort.Close();                   
    }

    private async void WriteDataToCloud()
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
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrReplace(new StorageRow()
                {
                    PM10 = this.PM10,
                    PM25 = this.PM25,
                    Humidity = this.Humidity,
                    
                });
                await table.ExecuteAsync(insertOrMergeOperation);

                TableOperation insertOrMergeOperation2 = TableOperation.InsertOrReplace(new StorageRow("LAST")
                {
                    PM10 = this.PM10,
                    PM25 = this.PM25
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
        Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HHmmss")} PM2.5 {PM25.ToString("00.0")} μg/m3 - PM10 {PM10.ToString("00.0")} μg/m3");
        Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HHmmss")} Temperature {Temperature.ToString()} - Pressure {Pressure.ToString()} - Humidity {Humidity.ToString()}%");
    }
}