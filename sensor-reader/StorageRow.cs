namespace core_sensor_reader
{
    using System;
    using Microsoft.Azure.Cosmos.Table;

    public class StorageRow : TableEntity
    {
        public StorageRow(string partitionKey, string rowKey): base(partitionKey, rowKey) { }

        public double PM10 { get; set; }
        public double PM25 { get; set; }
        public int Humidity { get; set; }
        public double Temperature { get; set; }
        public double? Pressure { get; set; }
    }
}