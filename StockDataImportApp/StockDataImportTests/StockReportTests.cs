using StockDataImport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StockDataImportTests
{
    public class StockReportTests
    {
        StockReportGenerator report = new StockReportGenerator();
        [Fact]
        public void Generate_Empty()
        {
            
            string output = report.Generate(new StockDataStructure());
            Assert.NotNull(output);
        }
        [Fact]
        public void Generate_SmalestRport()
        {
            var sd = new StockDataStructure() { Warehouses = new Dictionary<string, WarehouseStock> {
                {"wName",new WarehouseStock() { materials = new Dictionary<string, long> { {"mID",1 } } } }
            } };
            
            string output = report.Generate(sd);

            Assert.NotEmpty(output);
            Assert.Equal( "wName (total 1)"+Environment.NewLine + "mID: 1", output);
        }

        [Fact]
        public void Generate_ForOneWitManyMaterialsRport()
        {
            var sd = new StockDataStructure()
            {
                Warehouses = new Dictionary<string, WarehouseStock> {
                    {"wName",new WarehouseStock() { materials = new Dictionary<string, long> {
                        {"mID100",1 }, {"mID1",10 }, {"mID10",100}
                    } } }
                }
            };

            string output = report.Generate(sd);

            Assert.NotEmpty(output);
            Assert.Equal("wName (total 111)" + Environment.NewLine +
                "mID1: 10" + Environment.NewLine +
                "mID10: 100" + Environment.NewLine +
                "mID100: 1"
                , output);
        }

        [Fact]
        public void Generate_ForManyWarehousWithDifferenQuantity()
        {
            var sd = new StockDataStructure()
            {
                Warehouses = new Dictionary<string, WarehouseStock> {
                {"wName1",new WarehouseStock() { materials = new Dictionary<string, long> { {"mID",1 } } } },
                {"wName2",new WarehouseStock() { materials = new Dictionary<string, long> { {"mID",100 } } } },
                {"wName3",new WarehouseStock() { materials = new Dictionary<string, long> { {"mID",10 } } } }
            }
            };

            string output = report.Generate(sd);

            Assert.NotEmpty(output);
            Assert.Equal(
                "wName2 (total 100)" + Environment.NewLine + "mID: 100" + Environment.NewLine + Environment.NewLine +
                "wName3 (total 10)" + Environment.NewLine + "mID: 10" + Environment.NewLine + Environment.NewLine +
                "wName1 (total 1)" + Environment.NewLine + "mID: 1"
                , output);
        }

        [Fact]
        public void Generate_ForManyWarehousWithSameQuantity()
        {
            var sd = new StockDataStructure()
            {
                Warehouses = new Dictionary<string, WarehouseStock> {
                {"wName1",new WarehouseStock() { materials = new Dictionary<string, long> { {"mID",10 } } } },
                {"wName3",new WarehouseStock() { materials = new Dictionary<string, long> { {"mID",10 } } } },
                {"wName2",new WarehouseStock() { materials = new Dictionary<string, long> { {"mID",10 } } } }
            }
            };

            string output = report.Generate(sd);

            Assert.NotEmpty(output);
            Assert.Equal(
                "wName3 (total 10)" + Environment.NewLine + "mID: 10" + Environment.NewLine + Environment.NewLine +
                "wName2 (total 10)" + Environment.NewLine + "mID: 10" + Environment.NewLine + Environment.NewLine +
                "wName1 (total 10)" + Environment.NewLine + "mID: 10"
                , output);
        }
    }
}
