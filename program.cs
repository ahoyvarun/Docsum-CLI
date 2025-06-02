using System;
using System.IO;
using System.Threading.Tasks;

using Summarizers;
using ML;
using utils;
using tests;
class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("🧠 AI-Powered Document Summarizer");

            if (args.Contains("--help") || args.Contains("-h"))
            {
                Console.WriteLine("docsum-cli - AI-Powered Summarizer\n");
                Console.WriteLine("Usage:");
                Console.WriteLine("  --file <path>       Input document to summarize");
                Console.WriteLine("  --export            Export summary to PDF");
                Console.WriteLine("  --no-feedback       Skip feedback collection");
                Console.WriteLine("  --label <type>      Provide label for training (e.g., legal, business)");
                Console.WriteLine("  --output <filename> Output PDF filename");
                Console.WriteLine("  --local-only        Disable OpenAI API, use only ML.NET summarizer");
                Console.WriteLine("  --lang <code>       Translate summary to language (e.g., fr, de, es)");
                return;
            }

            if (args.Length < 2 || args[0] != "--file")
            {
                Console.WriteLine("Usage: dotnet run -- --file path_to_file.txt [--export] [--no-feedback] [--label label] [--output filename.pdf]");
                return;
            }

            string filePath = args[1];
            bool exportPdf = args.Contains("--export");
            bool skipFeedback = args.Contains("--no-feedback");
            string label = null;
            string outputFilename = "summary_output.pdf";

            bool localOnly = args.Contains("--local-only");
            string targetLang = null;

            for (int i = 2; i < args.Length; i++)
            {
                if (args[i] == "--label" && i + 1 < args.Length)
                {
                    label = args[i + 1];
                }
                if (args[i] == "--output" && i + 1 < args.Length)
                {
                    outputFilename = args[i + 1];
                }
                if (args[i] == "--lang" && i + 1 < args.Length)
                {
                    targetLang = args[i + 1];
                }
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine("❌ File not found.");
                return;
            }

            string content = File.ReadAllText(filePath);
            var summary = await SummaryRouter.GetSmartSummary(content, localOnly);

            if (!string.IsNullOrEmpty(targetLang))
            {
                summary = await Translator.Translate(summary, targetLang);
            }

            Console.WriteLine("\n=== Summary ===\n");
            Console.WriteLine(summary);

            if (exportPdf)
            {
                PdfExporter.ExportSummaryToPdf(summary, outputFilename);
            }
            else
            {
                Console.Write("\n🖨️ Would you like to export the summary to a PDF? (y/n): ");
                var exportResponse = Console.ReadLine().Trim().ToLower();
                if (exportResponse == "y")
                {
                    PdfExporter.ExportSummaryToPdf(summary, outputFilename);
                }
            }

            if (!skipFeedback)
            {
                if (string.IsNullOrEmpty(label))
                {
                    Console.Write("\n🤔 Was the predicted document type correct? (y/n): ");
                    var response = Console.ReadLine().Trim().ToLower();
                    if (response == "n")
                    {
                        Console.Write("Please enter the correct label (e.g., contract, note, report): ");
                        label = Console.ReadLine().Trim().ToLower();
                    }
                }

                if (!string.IsNullOrEmpty(label))
                {
                    ModelTrainer.TrainOrUpdateModel(content, label);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ An unexpected error occurred: {ex.Message}");
        }
    }
}