namespace StockDataImport
{
    public interface IStockReportGenerator
    {
        string Generate(StockDataStructure stockDataStructure);
    }
}