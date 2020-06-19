using StockDataImport;
using System;
using System.IO;
using System.Text;

namespace StockDataImportApp
{
    public class Program
    {
        private const int tabSize = 4;
        private const string usageText = "Usage: StockDataImportApp inputfile.txt outputfile.txt";

        public static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(usageText);
                return 1;
            }

            try
            {

                StockReportBuilder reportBuilder = new StockReportBuilder();
                reportBuilder.SetStockReport(new StockReportGenerator());
                Parser parser = new Parser(reportBuilder);


                // Attempt to open output file.
                using (var writer = new StreamWriter(args[1]))
                {
                    using (var reader = new StreamReader(args[0]))
                    {
                        // Redirect standard output from the console to the output file.
                        Console.SetOut(writer);
                        // Redirect standard input from the console to the input file.
                        Console.SetIn(reader);
                        StringBuilder sb = new StringBuilder();
                        string line;
                        while ((line = Console.ReadLine()) != null)
                        {
                            sb.AppendLine(line);
                        }

                        parser.Parse(sb.ToString().Trim());
                        Console.Write(reportBuilder.GetReport());

                    }
                }
            }
            catch (IOException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                errorWriter.WriteLine(usageText);
                return 1;
            }

            // Recover the standard output stream so that a
            // completion message can be displayed.
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            Console.WriteLine($"StockDataImportApp has completed the processing of {args[0]}.");
            return 0;
        }
    }
}
