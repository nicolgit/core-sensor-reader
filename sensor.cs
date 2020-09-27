using System;
using System.IO.Ports;
using System.Threading;

public class sensor
{
    private SerialPort serialPort;
    private byte[] slice = new byte[10];

    public bool Verbose { get; set; }
    public string TableStorageUrl { get; set; }
    public string TableStorageKey { get; set; }
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

    public void ReadStream ()
    {
        serialPort = new SerialPort(Port);
  
        serialPort.ReadTimeout = 1500;             
        serialPort.WriteTimeout = 1500;
        serialPort.Open();
        
        if (Verbose) Console.WriteLine($"Port {Port} Opened successfully.");     

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
            
            Thread.Sleep( 1000 * Sampling);
        }     

        // serialPort.Close();                   
    }

    private void dump()
    {
        foreach(byte b in slice)
        {
            Console.Write(Convert.ToString(b, 16).PadLeft(2, '0'));
        }
        Console.WriteLine($"  PM2.5 {DateTime.Now.ToString("yyyyMMdd HHmmss")} {PM25.ToString("00.0")} μg/m3 - PM10 {PM10.ToString("00.0")} μg/m3");
    }
}