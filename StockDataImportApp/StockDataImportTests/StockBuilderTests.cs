using StockDataImport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StockDataImportTests
{
    public class StockBuilderTests : IStockReportGenerator
    {
        StockDataStructure buildedTestData;
        StockReportBuilder bulder = new StockReportBuilder();
        public StockBuilderTests()
        {
            bulder.SetStockReport(this);
        }

        [Fact]
        public void GetReport_AddMethodNotExecuted_EmptyReport()
        {
            bulder.GetReport();
            Assert.Empty(buildedTestData.Warehouses);
            Assert.Empty(buildedTestData.Materials);
        }

        [Fact]
        public void GetReport_AddOneMaterialToOneWarehouse_SingleMaterialAndWarehouse()
        {
            bulder.AddMaterial("ma", "mId", new List<(string warehouse, long quantity)>() {("w1",1 )  });
            bulder.GetReport();
            Assert.Single(buildedTestData.Warehouses);
            Assert.Single(buildedTestData.Materials);
        }

        [Fact]
        public void GetReport_AddOneMaterialToManyWarehouses_SingleMaterialManyWarehouse()
        {
            bulder.AddMaterial("ma", "mId", new List<(string warehouse, long quantity)>() { ("w1", 1) });
            bulder.AddMaterial("ma", "mId", new List<(string warehouse, long quantity)>() { ("w2", 1) });
            bulder.GetReport();
            Assert.Equal(2,buildedTestData.Warehouses.Count);
            Assert.Single(buildedTestData.Materials);
        }

        [Fact]
        public void GetReport_AddOneMaterialToManyWarehousesInSingleAction_SingleMaterialManyWarehouse()
        {
            bulder.AddMaterial("ma", "mId", new List<(string warehouse, long quantity)>() { ("w1", 1), ("w2", 1) });            
            bulder.GetReport();
            Assert.Equal(2, buildedTestData.Warehouses.Count);
            Assert.Single(buildedTestData.Materials);
        }

        [Fact]
        public void GetReport_AddManyMaterialToManyWarehouses_ManyMaterialManyWarehouse()
        {
            bulder.AddMaterial("ma1", "mId1", new List<(string warehouse, long quantity)>() { ("w1", 1) });
            bulder.AddMaterial("ma2", "mId2", new List<(string warehouse, long quantity)>() { ("w2", 1), ("w1", 1) });
            bulder.GetReport();
            Assert.Equal(2, buildedTestData.Warehouses.Count);
            Assert.Equal(2,buildedTestData.Materials.Count);
        }

        //public void GetEmpty
        public string Generate(StockDataStructure stockDataStructure)
        {
            this.buildedTestData = stockDataStructure;
            return null;
        }
    }
}
