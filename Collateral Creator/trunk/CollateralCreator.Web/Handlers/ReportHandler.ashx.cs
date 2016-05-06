using CollateralCreator.Business;
using CollateralCreator.SQLProvider;

namespace CollateralCreator.Web
{
    using System;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Summary description for ReportHandler
    /// </summary>
    public class ReportHandler : IHttpHandler
    {
        private HttpContext Context;
        private HttpRequest Request;
        private HttpResponse Response;
        private int templateid { set; get; }
        private bool partnerbrand { set; get; }
        private string language { set; get; }
        private int channelpartnerid { set; get; }
        private string channelpartnerloginid { set; get; }
        private string channelpartneremail { set; get; }
        private string documentname { set; get; }
        private string mode { set; get; }

        private string sTemplatename = string.Empty;
        private string sErrormessage = string.Empty;
        private string sDefaultLogoMsg = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            Context = context;
            Request = Context.Request;
            Response = Context.Response;

            ProcessVars();

            switch (mode.ToLower())
            {
                case "generatepdf":
                    GeneratePDF(templateid, partnerbrand);
                    break;
                case "downloadpdf":
                    ResponseHandler(documentname);
                    break;
            }
        }

        private void ProcessVars()
        {
            if (Request["mode"] != null)
                mode = Request["mode"];
            if (Request["partnerbrand"] != null)
                partnerbrand = Convert.ToBoolean(Request["partnerbrand"]);
            if (Request["language"] != null)
                language = Request["language"];
            if (Request["templateid"] != null)
                templateid = Convert.ToInt32(Request["templateid"]);
            if (Request["documentname"] != null)
                documentname = Request["documentname"];
            if (Request["channelpartnerid"] != null)
                channelpartnerid = Convert.ToInt32(Request["channelpartnerid"]);
            if (Request["channelpartnerloginid"] != null)
                channelpartnerloginid = Request["channelpartnerloginid"];
            if (Request["channelpartneremail"] != null)
                channelpartneremail = Request["channelpartneremail"];

        }

        private void GeneratePDF(int TemplateId, bool IsPXbrand)
        {
            string sFileNameNew = string.Empty;

            //get the Document object for the template and brand provided           
            CollateralCreator.Data.Document objdocument = DocumentManager.CreateDocument(TemplateId, IsPXbrand, language, channelpartnerid, channelpartnerloginid, channelpartneremail);
            
            //to create doc in the project folder and returns back the name
            PdfManager pdf = new PdfManager();
            pdf.GenerateDocument(ref sFileNameNew, objdocument);

            CreateDocument cd = new CreateDocument();
            cd.Document("download", objdocument, ref pdf, ref sFileNameNew, ref sTemplatename, ref sErrormessage, ref sDefaultLogoMsg, null);
            
            //once the document is created, now register a log history in the table
            CollateralCreatorRepository.Log_DocumentDownload(objdocument.DocumentID);

            //write the response
            ResponseHandler(sFileNameNew);
        }

        private void ResponseHandler(string filename)
        {
            string filepath = Context.Server.MapPath("~/temp/" + filename);

            FileInfo myfile = new FileInfo(filepath);
            
            // Checking if file exists
            if (myfile.Exists)
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/pdf";

                // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                // Add the file size into the response header
                Context.Response.AddHeader("Content-Length", myfile.Length.ToString());

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Context.Response.TransmitFile(myfile.FullName);
               
                // End the response
                Context.Response.End(); 
              
            }
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}