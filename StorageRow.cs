namespace core_sensor_reader
{
    using System;
    using Microsoft.Azure.Cosmos.Table;

    public class StorageRow : TableEntity
    {
        public StorageRow()
        {
            PartitionKey = DateTime.Now.ToString("yyyyMMdd");
            RowKey = DateTime.Now.ToString("HHmmss");
        }

        public double PM10 { get; set; }
        public double PM25 { get; set; }
    }
}