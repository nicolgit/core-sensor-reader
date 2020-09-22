using System;
using System.IO.Ports;

namespace core_sensor_reader
{
    class Program
    {
        static string port = "/dev/ttyUSB0";

        static SerialPort _serialPort; 

        static void Main(string[] args)
        {
            Console.WriteLine("Nove SDS011 PM reader - NicolD");

            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();            
            Console.WriteLine("The following serial ports were found:");       

            // Display each port name to the console.             
            foreach(string port in ports)             
            {                 
                Console.WriteLine(port);             
            }             

            _serialPort = new SerialPort("/dev/ttyUSB0");
            
            // Set the read/write timeouts             
            _serialPort.ReadTimeout = 1500;             
            _serialPort.WriteTimeout = 1500;
            _serialPort.Open();
            Console.WriteLine($"Port {port} Opened successfully.");     

            int i = 0;
            while (i<1000)
            {
                var b = _serialPort.ReadByte();
                Console.Write (b.ToString());

                i++;
            }

            _serialPort.Close();            
        }
    }
}
