using System;
using System.Collections.Generic;
using System.Text;

namespace StockDataImport
{
    public class StockReportBuilder : IStockReportBuilder
    {
        private StockDataStructure stockData;
        private IStockReportGenerator reporter;
        public StockReportBuilder()
        {
            stockData = new StockDataStructure()
            {
                Warehouses = new Dictionary<string,WarehouseStock>(),
                Materials = new Dictionary<string, string>()
            };
        }

        public void SetStockReport(IStockReportGenerator reporter)
        {
            this.reporter = reporter;
        }


        public string GetReport()
        {
            return reporter?.Generate(stockData);
        }

        public void AddMaterial(string materialName, string materialId, List<(string warehouse, long quantity)> loactionAndQuantity)
        {
            AddMaterialName(materialName, materialId);

            foreach (var w in loactionAndQuantity)
            {
                AddWarehouse(w);

                AddMaterialsToWarehouse(materialId, w);
            }
        }

        private void AddMaterialName(string materialName, string materialId)
        {
            if (!this.stockData.Materials.ContainsKey(materialId))
                this.stockData.Materials.Add(materialId, materialName);
        }
        private void AddWarehouse((string warehouse, long quantity) w)
        {
            if (!this.stockData.Warehouses.ContainsKey(w.warehouse))
                this.stockData.Warehouses.Add(w.warehouse, new WarehouseStock { materials = new Dictionary<string, long>() });
        }
        private void AddMaterialsToWarehouse(string materialId, (string warehouse, long quantity) w)
        {
            if (!this.stockData.Warehouses[w.warehouse].materials.ContainsKey(materialId))
                this.stockData.Warehouses[w.warehouse].materials.Add(materialId, 0);

            this.stockData.Warehouses[w.warehouse].materials[materialId] += w.quantity;
        }

        
    }


}
