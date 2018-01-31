using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Winnovative.WnvHtmlConvert;

namespace LucreeAPP.Models
{
    public class PdfGenerator
    {
        public static Byte[] GetPdfContractFromHtml(string url)
        {
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.LicenseKey = Resource.License;
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;
            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            pdfConverter.PdfDocumentOptions.ShowFooter = false;
            pdfConverter.PdfDocumentOptions.SinglePage = true;
            pdfConverter.PdfDocumentOptions.FitHeight = true;
            pdfConverter.PdfDocumentOptions.FitWidth = true;
            pdfConverter.PdfDocumentOptions.FitWidth = false;
            pdfConverter.PdfDocumentOptions.EmbedFonts = false;
            pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
            pdfConverter.ConversionDelay = 0;
            pdfConverter.ScriptsEnabled = true;
            pdfConverter.RightToLeftEnabled = true;
            byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(url);


            //if (System.IO.File.Exists("C:\\Users\\edward.silva\\Desktop\\aTestFile.pdf"))
            //    System.IO.File.Delete("C:\\Users\\edward.silva\\Desktop\\aTestFile.pdf");

            //using (FileStream fs = System.IO.File.Create("C:\\Users\\edward.silva\\Desktop\\aTestFile.pdf"))
            //{
            //    fs.Write(pdfBytes, 0, (int)pdfBytes.Length);
            //}

            return pdfBytes;

        }

        public static byte[] Convert(Uri url, string wkhtmlToPdfExePath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = wkhtmlToPdfExePath,
                WorkingDirectory = @"C:\",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var args = new StringBuilder();
            args.Append("-q  ");
            args.Append("--footer-left"); args.Append("  \"Converted by Wkhtml To Pdf Written By Sm.Abdullah".PadRight(90) + DateTime.Now.ToString("dd-MMM-yy") + "\"  ");
            args.Append("--footer-font-size"); args.Append(" 9   ");
            args.Append("--footer-right"); args.Append(" [page]/[toPage]  ");
            args.Append("--footer-line  ");
            args.Append("--outline-depth"); args.Append(" 0  ");
            args.Append("--enable-javascript  ");
            args.Append("--no-stop-slow-scripts  ");
            args.Append("--javascript-delay"); args.Append(" 3500  ");
            args.Append("--page-size"); args.Append(" A4  ");
            args.Append("\"" + url + "\"" + " -");

            processStartInfo.Arguments = args.ToString();

            Process process = Process.Start(processStartInfo);

            byte[] buffer = new byte[32768];
            byte[] file;
            using (var memoryStream = new MemoryStream())
            {
                while (true)
                {
                    int read = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        break;
                    memoryStream.Write(buffer, 0, read);
                }
                file = memoryStream.ToArray();
            }

            process.StandardOutput.Close();
            // wait or exit
            process.WaitForExit(60000);

            // read the exit code, close process
            int returnCode = process.ExitCode;
            process.Close();
            process.Dispose();
            if (returnCode == 0 || returnCode == 1)
                return file;
            else
            {
                throw new Exception(string.Format("Could not create PDF, returnCode:{0}", returnCode));
            }
        }
    }
}