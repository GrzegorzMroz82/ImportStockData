using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockDataImport
{
    public class StockReportGenerator : IStockReportGenerator
    {

        class ReportSection
        {
            public string Name { get; set; }
            public long Total { get; set; }
            public Dictionary<string, long> Materials { get; set; }

        }
        public string Generate(StockDataStructure stockDataStructure)
        {
            StringBuilder sb = new StringBuilder();


            if (stockDataStructure == null || stockDataStructure.Warehouses == null)
                return sb.ToString();

            List<ReportSection> sections = GenerateOrderedReportSections(stockDataStructure);

            foreach (var s in sections)
            {
                if (sb.Length > 0)
                    sb.AppendLine();
                AddWarehouseHeader(sb, s);
                AddWarehouseMaterials(sb, s);
            }

            return sb.ToString().Trim();
        }
        private List<ReportSection> GenerateOrderedReportSections(StockDataStructure stockDataStructure)
        {
            List<ReportSection> sections = new List<ReportSection>();
            foreach (var w in stockDataStructure.Warehouses)
            {
                sections.Add(new ReportSection { Name = w.Key, Total = w.Value.materials.Sum(x => x.Value), Materials = w.Value.materials });
            }

            return sections.OrderByDescending(x => x.Total).ThenByDescending(x => x.Name).ToList();
        }

        private void AddWarehouseHeader(StringBuilder sb, ReportSection s)
        {
            sb.AppendLine($"{s.Name} (total {s.Total})");
        }

        private void AddWarehouseMaterials(StringBuilder sb, ReportSection s)
        {
            foreach (var m in s.Materials.OrderBy(x => x.Key))
            {
                sb.AppendLine($"{m.Key}: {m.Value}");
            }
        }
    }
}
