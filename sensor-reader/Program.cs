using System;
using System.IO.Ports;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Azure.Cosmos.Table;

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
                .WithParsed<CommandLineOptions>((options) => Run(options))
                .WithNotParsed(errs => DisplayHelp(parserResult));
        }
    
        static void DisplayHelp<T>(ParserResult<T> result)
        {  
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "Nova SDS011 PM reader";
                h.Copyright = "copy-left 2020 by Nicola Delfino";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            Console.WriteLine(helpText);
        }

        private static void Run(CommandLineOptions o)
        {
            RunAsync(o).GetAwaiter().GetResult();
        }

        private static async Task RunAsync(CommandLineOptions o)
        {
            sensor s = new sensor();
            s.Verbose = o.Verbose;
            s.Port = o.SerialPort;
            s.TableStorageConnectionString = o.TableStorageConnectionString;
            s.TableStorageTable = o.TableStorageTable;
            s.LocationName = o.LocationName;
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
            
            if (o.SerialPort == null || o.LocationName == null)
            {
                Console.WriteLine("serial ports and location name required to start collecting data"); 
            }
            else
            {
                try 
                {
                    await s.ReadStream();
                }
                catch (Exception e)
                {
                    Console.WriteLine ($"ERROR: {e.GetType().ToString()} - {e.Source} - {e.Message}");
                }   
            }
            
            Console.WriteLine ("");
            Console.WriteLine ($" use --help to display the help screen");
        }
    }
}
