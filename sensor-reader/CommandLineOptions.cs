using CommandLine;

public class CommandLineOptions
{
    [Option('v', "verbose", Required = false, Default=false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }

    [Option('u', "table-storage-connection-string", Required = false, HelpText = "if present append reads to the specified Azure TableStorage i.e. 'DefaultEndpointsProtocol=https;AccountName=mystorage;AccountKey=6ue+jlhTtlO6Vg46l508sK+ousmQ==;EndpointSuffix=core.windows.net'")]
    public string TableStorageConnectionString { get; set; }

    [Option('t', "table-storage-table", Required = false, HelpText = "if present append reads to the specified Azure TableStorage i.e. mytable")]
    public string TableStorageTable { get; set; }
    
    [Option('s', "serial-port", Required = false, HelpText = "Serial Port used by the Sensor")]
    public string SerialPort { get; set; }

    [Option('i', "sampling-interval", Required = false, Default=2,  HelpText = "Sampling interval in seconds")]
    public int SamplingInterval { get; set; }

    [Option('e', "enumerate-ports", Required = false, HelpText = "enumerate serial ports availables on start")]
    public bool EnumeratePorts { get; set; }
}