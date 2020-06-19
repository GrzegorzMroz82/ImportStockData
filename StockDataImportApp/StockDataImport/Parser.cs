using System;
using System.Collections.Generic;
using System.Linq;

namespace StockDataImport
{
    public class Parser
    {
        private IStockReportBuilder builder;
        int lineNr = 0;
        List<string> errors = new List<string>();

        public Parser(IStockReportBuilder builder)
        {
            this.builder = builder;
        }

        public void Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;
            var lines = input.Split(new string[]{ Environment.NewLine,"\n","\r"},StringSplitOptions.RemoveEmptyEntries);
            ParseLines(lines);
        }

        private void ParseLines(string[] lines)
        {
            foreach (var l in lines)
            {
                if (l.StartsWith("#")) 
                {
                    lineNr++;
                    continue;
                }                   
                ParseMaterialLine(l);
                lineNr++;
            }
        }

        private void ParseMaterialLine(string line)
        {
            var materialInfoParts = line.Split(';');
            if (materialInfoParts.Length != 3)
            {
                AddErrorMsg("invalid format ");
                return;
            }
            var materialName = materialInfoParts[0].Trim();
            var materialId = materialInfoParts[1].Trim();

            List<(string warehouse, long quantity)> loactionAndQuantity = ParseLoactionAndQuantity(materialInfoParts);
            if (loactionAndQuantity != null)
            {
                builder.AddMaterial(materialName, materialId, loactionAndQuantity);
            }
        }

        private List<(string warehouse, long quantity)> ParseLoactionAndQuantity(string[] materialInfoParts)
        {
            List<(string warehouse, long quantity)> loactionAndQuantity = new List<(string warehouse, long quantity)>();
            var warehouses = materialInfoParts[2].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (warehouses.Length == 0)
            {
                AddErrorMsg("invalid format");
                return null;
            }
            foreach (var w in warehouses)
            {
                var wt = w.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                long quantity;
                if (wt.Length != 2 || !long.TryParse(wt[1].Trim(), out quantity))
                {
                    AddErrorMsg("invalid format");
                    return null;
                }
                string warehouse = wt[0].Trim();
                loactionAndQuantity.Add((warehouse: warehouse, quantity: quantity));
            }

            return loactionAndQuantity;
        }

        private void AddErrorMsg(string errorMsg)
        {
            errors.Add("Line: " +lineNr + ":" + errorMsg);
        }

        public IEnumerable<string> GetErrorlist()
        {
            return errors.ToList();
        }

        public int GetLineParsedCount()
        {
            return lineNr;
        }
    }
}