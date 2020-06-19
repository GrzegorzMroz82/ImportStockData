using System;
using System.Collections.Generic;
using System.Text;


namespace StockDataImport
{
    public interface IStockReportBuilder
    {
        void AddMaterial(string materialName, string materialId, List<(string warehouse, long quantity)> loactionAndQuantity);
        string GetReport();
    }
}
