using LucreeAPP;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Winnovative.WnvHtmlConvert;

namespace Lucree.APP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult ShowPDF(string par1, string par2, string par3)
        {

            string url = string.Format("{0}/{1}/Home/Contrato?par1={2}&par2={3}&par3={4}", Request.Url.Authority, Request.ApplicationPath, par1, par2, par3);
            try
            {

                var pdf = GetPdfContractFromHtml(url);

                if (pdf != null)
                {
                    MemoryStream ms = new MemoryStream(pdf.ToArray());
                    FileStreamResult filestream = new FileStreamResult(ms, "application/pdf");
                    return filestream;
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("(" + url + ") - " + ex.ToString());
            }
        }

        public Byte[] GetPdfContractFromHtml(string url)
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

        [AllowAnonymous]
        public virtual ActionResult Contrato(string par1, string par2, string par3)
        {
            dynamic model = new ExpandoObject();
            model.um = par1;
            model.dois = par2;
            model.tres = par3;

            return View(model);
        }
    }
}