using System;
using System.Collections.Generic;
using System.Text;

namespace StockDataImport
{
    public class StockDataStructure
    {
        public Dictionary<string, WarehouseStock> Warehouses { get; set; }
        public Dictionary<string,string> Materials { get; set; }
    }

    public class WarehouseStock
    {
        public Dictionary<string, long> materials { get;set;}
    }
}
