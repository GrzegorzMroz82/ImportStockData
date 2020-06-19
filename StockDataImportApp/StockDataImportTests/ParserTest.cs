using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StockDataImport
{
    public class ParserTest : IStockReportBuilder
    {
        List<string> testResults = new List<string>();
        Parser parser;
        public ParserTest()
        {
            parser = new Parser(this);
        }

        [Fact]
        public void Parse_IgnoredLine()
        {
            parser.Parse("#");
            Assert.Empty(testResults);            
        }
        [Fact]
        public void Parse_TwoIgnoredLine()
        {
            parser.Parse("#" + Environment.NewLine + "#");
            Assert.Empty(testResults);
            Assert.Equal(2,parser.GetLineParsedCount());
        }
        [Fact]
        public void Parse_WrongFormatLine()
        {
            parser.Parse("a");
            Assert.NotEmpty(parser.GetErrorlist());
        }
        [Fact]
        public void Parse_IgnorLineWrongFormatLine()
        {
            parser.Parse("#" + Environment.NewLine + "a");
            Assert.NotEmpty(parser.GetErrorlist());
        }
        [Fact]
        public void Parse_LineWithQuantityNotANumber()
        {
            parser.Parse("mn; mid; w, X");
            Assert.NotEmpty(parser.GetErrorlist());
        }

        [Fact]
        public void Parse_LineWithOneWarehouse()
        {
            parser.Parse("mn; mid; w, 1");
            Assert.Empty(parser.GetErrorlist());
            Assert.Equal(1, parser.GetLineParsedCount());
            Assert.Single(testResults);
        }

        [Fact]
        public void Parse_LineWithManyWarehouseWithWhiteSpaces()
        {
            parser.Parse("mn; mid; w1, 1 |    w2 , 10 | w3,    100");
            Assert.Empty(parser.GetErrorlist());
            Assert.Equal(1, parser.GetLineParsedCount());
            Assert.Equal("mn; mid; w1,1|w2,10|w3,100", testResults[0]);
        }

        [Fact]
        public void Parse_ManyLineWithManyWarehouse()
        {
            
            parser.Parse("mn1; mid1; w1, 1|w2,10| w3,100" + '\n' + "mn2; mid2; w1, 10|w2,100| w3,  1" + '\n' + "mn3; mid3; w1, 100  |w2,10| w3,1");
            Assert.Empty(parser.GetErrorlist());
            Assert.Equal(3, parser.GetLineParsedCount());
            Assert.Equal("mn1; mid1; w1,1|w2,10|w3,100", testResults[0]);
            Assert.Equal("mn2; mid2; w1,10|w2,100|w3,1", testResults[1]);
            Assert.Equal("mn3; mid3; w1,100|w2,10|w3,1", testResults[2]);
        }

        public void AddMaterial(string materialName, string materialId, List<(string warehouse, long quantity)> warehouseData)
        {
            testResults.Add($"{materialName}; {materialId}; {string.Join('|', warehouseData.Select(x => x.warehouse + "," + x.quantity))}");
        }

        public string GetReport()
        {
            return null;
        }
    }
}
