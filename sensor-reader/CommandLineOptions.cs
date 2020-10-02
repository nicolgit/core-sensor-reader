using System;
using CommandLine;

public class CommandLineOptions
{
    [Option('v', "verbose", Required = false, Default=false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }

    [Option('u', "table-storage-url", Required = false, HelpText = "if present append reads to the specified Azure TableStorage i.e. mystorage in https://mystorage.table.core.windows.net/mytable")]
    public string TableStorageUrl { get; set; }

    [Option('t', "table-storage-table", Required = false, HelpText = "if present append reads to the specified Azure TableStorage i.e. mytable in https://mystorage.table.core.windows.net/mytable")]
    public string TableStorageTable { get; set; }
    [Option('k', "table-storage-key", Required = false, HelpText = "if present append reads to Azure TableStorage data i.e. 99/cBc443vT+v2CpqapPyZMxainr33fsmQE7yx+w==")]
    public string TableStorageKey { get; set; }

    [Option('s', "serial-port", Required = true, HelpText = "Serial Port used by the Sensor")]
    public string SerialPort { get; set; }

    [Option('i', "sampling-interval", Required = false, Default=2,  HelpText = "Sampling interval in seconds")]
    public int SamplingInterval { get; set; }

    [Option('e', "enumerate-ports", Required = false, HelpText = "enumerate serial ports availables on start")]
    public bool EnumeratePorts { get; set; }
}