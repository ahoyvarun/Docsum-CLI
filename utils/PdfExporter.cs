using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System;
using System.IO;
using System.Collections.Generic;

namespace utils
public static class PdfExporter
{
    public static void ExportSummaryToPdf(string summaryText, string outputFileName = "summary_output.pdf")
    {
        try
        {
            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 12, XFontStyle.Regular);

            double margin = 40;
            double yPoint = margin;
            double maxWidth = page.Width - 2 * margin;
            var lines = WrapText(summaryText, gfx, font, maxWidth);

            foreach (var line in lines)
            {
                if (yPoint + 20 > page.Height - margin)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin;
                }

                gfx.DrawString(line, font, XBrushes.Black,
                    new XRect(margin, yPoint, page.Width - margin, page.Height - margin),
                    XStringFormats.TopLeft);
                yPoint += 20;
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), outputFileName);
            document.Save(path);
            Console.WriteLine($"📄 PDF summary saved to: {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ PDF export failed: {ex.Message}");
        }
    }

    private static List<string> WrapText(string text, XGraphics gfx, XFont font, double maxWidth)
    {
        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = "";

        foreach (var word in words)
        {
            var testLine = string.IsNullOrEmpty(currentLine) ? word : $"{currentLine} {word}";
            var size = gfx.MeasureString(testLine, font);

            if (size.Width < maxWidth)
            {
                currentLine = testLine;
            }
            else
            {
                lines.Add(currentLine);
                currentLine = word;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
            lines.Add(currentLine);

        return lines;
    }
}