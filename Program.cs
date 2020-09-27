using System;
using System.IO.Ports;
using CommandLine;
using CommandLine.Text;

namespace core_sensor_reader
{
    class Program
    {
        //static string port = "/dev/ttyUSB0"; // raspberry default
        //static string port = "COM5"; // laptop default

        static void Main(string[] args)
        {
            var parser = new CommandLine.Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<CommandLineOptions>(args);
            parserResult
                .WithParsed<CommandLineOptions>(options => Run(options))
                .WithNotParsed(errs => DisplayHelp(parserResult));
        }
    
        static void DisplayHelp<T>(ParserResult<T> result)
        {  
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "Nove SDS011 PM reader"; //change header
                h.Copyright = "copy-left 2020 by Nicola Delfino";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            Console.WriteLine(helpText);
        }

        private static void Run(CommandLineOptions o)
        {
            sensor s = new sensor();
            s.Verbose = o.Verbose;
            s.Port = o.SerialPort;
            s.TableStorageUrl = o.TableStorageUrl;
            s.TableStorageKey = o.TableStorageKey;
            s.Sampling = o.SamplingInterval;
            
            if (o.Verbose) Console.WriteLine("App is in Verbose mode.");

            if (o.EnumeratePorts)
                {
                    string[] ports = SerialPort.GetPortNames();            
                    Console.WriteLine("The following serial ports were found:");       

                    // Display each port name to the console.    
                    foreach(string portFound in ports)             
                    {                 
                        Console.WriteLine(portFound);             
                    }             
                }    
            
            try 
            {
                s.ReadStream();
            }
            catch (Exception e)
            {
                Console.WriteLine ($"ERROR: {e.GetType().ToString()} - {e.Source} - {e.Message}");
            }   

        }
    }
}
