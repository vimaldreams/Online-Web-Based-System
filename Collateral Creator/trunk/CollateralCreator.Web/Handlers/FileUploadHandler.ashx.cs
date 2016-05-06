using CollateralCreator.SQLProvider;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;

namespace CollateralCreator.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;

    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler
    {
        private HttpContext Context;
        private HttpRequest Request;
        private HttpResponse Response;

        private string mode { set; get; }
        private string sImageAreaID { set; get; }
        private int iDocumentID { set; get; }
        private string sLoginId { set; get; }

        public void ProcessRequest(HttpContext context)
        {
            Context = context;
            Request = Context.Request;
            Response = Context.Response;

            ProcessVars();

            switch (mode.ToLower())
            {
                case "documentimage":
                    ChangeDocumentImage();
                    break;
                case "defaultimage":
                    ChangeDefaultImage();
                    break;
            }
            
        }

        private void ChangeDefaultImage()
        {
            Xerox.SSOComponents.Models.Image image = null;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                byte[] imgbyte = new Byte[file.ContentLength];
                file.InputStream.Read(imgbyte, 0, file.ContentLength);

                //Create an image object from the uploaded file
                System.Drawing.Image UploadedImage = System.Drawing.Image.FromStream(file.InputStream);

                //Determine width and height of uploaded image
                int UploadedImageWidth = Convert.ToInt16(UploadedImage.PhysicalDimension.Width);
                int UploadedImageHeight = Convert.ToInt16(UploadedImage.PhysicalDimension.Height);

                if (ChannelPartner.Images.Count != 0)
                {
                    image = ChannelPartner.Images[0];
                    image.image = imgbyte;
                    image.Height = UploadedImageHeight;
                    image.Width = UploadedImageWidth;

                    ChannelPartnerService.UpdateImage(image);
                }
                else
                {
                    image = new Xerox.SSOComponents.Models.Image(0, ChannelPartner.LoginId, imgbyte, UploadedImageWidth, UploadedImageHeight);
                    ChannelPartnerService.CreateImage(image);
                }

                string strFileName = "your logo saved successfully !!";
                string msg = "{";
                msg += string.Format("error:'{0}',\n", string.Empty);
                msg += string.Format("msg:'{0}'\n", strFileName);
                msg += "}";
                Response.Write(msg);
            }
        }

        private void ChangeDocumentImage()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                byte[] imgbyte = new Byte[file.ContentLength];
                file.InputStream.Read(imgbyte, 0, file.ContentLength);
                
                if (sImageAreaID != string.Empty)
                {
                    CollateralCreatorRepository.DocumentUploadImage(sImageAreaID, imgbyte);

                    string strFileName = "your logo saved successfully !!";
                    string msg = "{";
                    msg += string.Format("error:'{0}',\n", string.Empty);
                    msg += string.Format("msg:'{0}'\n", strFileName);
                    msg += "}";
                    Response.Write(msg);
                }
            }
        }

        private void ProcessVars()
        {
            if (Request.QueryString["mode"] != null)
                mode = Request.QueryString["mode"];
            
            if (Request.QueryString["imageid0"] != null)
                sImageAreaID += Request.QueryString["imageid0"] + ",";

            if (Request.QueryString["imageid1"] != null)
                sImageAreaID += Request.QueryString["imageid1"] + ",";

            if (Request.QueryString["imageid2"] != null)
                sImageAreaID += Request.QueryString["imageid2"] + ",";

            if (Request.QueryString["imageid0"] != null || Request.QueryString["imageid1"] != null || Request.QueryString["imageid2"] != null)
                sImageAreaID = sImageAreaID.TrimEnd(',');
            
            if (Request.QueryString["documentid"] != null)
                iDocumentID = Convert.ToInt32(Request.QueryString["documentid"]);
            
            if (Request.QueryString["loginid"] != null)
                sLoginId = Request.QueryString["loginid"];
        }
       
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        ChannelPartner _channelPartner = null;
        private ChannelPartner ChannelPartner
        {
            get
            {
                if (_channelPartner != null)
                {
                    return _channelPartner;
                }

                if (!string.IsNullOrEmpty(Context.User.Identity.Name))
                {
                    if (_channelPartnerService == null) _channelPartnerService = new ChannelPartnerService(new ChannelPartnerRepository(ConfigurationManager.ConnectionStrings["XeroxPortal"].ToString()));

                    _channelPartner = _channelPartnerService.Retrieve(Context.User.Identity.Name);

                    //if (_channelPartner != null)
                    //{
                    //    this._displayLanguageId = _channelPartner.LanguageId;
                    //    this._displayCountryId = _channelPartner.CountryId;
                    //}
                }
                return _channelPartner;
            }
        }

        ChannelPartnerService _channelPartnerService = null;
        private ChannelPartnerService ChannelPartnerService
        {
            get
            {
                if (_channelPartnerService != null)
                {
                    return _channelPartnerService;
                }
                _channelPartnerService = new ChannelPartnerService(new ChannelPartnerRepository(ConfigurationManager.ConnectionStrings["XeroxPortal"].ToString()));
                return _channelPartnerService;
            }
        }
    }
}