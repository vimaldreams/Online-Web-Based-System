using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

using CollateralCreator.Business;
using CollateralCreator.SQLProvider;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;
using Xerox.SSOComponents.Models.Enumerations;

namespace CollateralCreator.Web
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        private HttpContext Context;
        private HttpRequest Request;
        private HttpResponse Response;

        private string mode { set; get; }
        private string sImageAreaID { set; get; }
        private string sLoginId { set; get; }

        public void ProcessRequest(HttpContext context)
        {
            Context = context;
            Request = Context.Request;
            Response = Context.Response;

            Response.Expires = -1;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

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
            Xerox.SSOComponents.Models.Image image = this.ChannelPartner.Images[0];

            MemoryStream strm = new MemoryStream((byte[])image.image);

            byte[] buffer = new byte[2048];
            int byteSeq = strm.Read(buffer, 0, 2048);
            
            Response.Clear();
            Response.ContentType = "image/bmp";

            while (byteSeq > 0)
            {
                Response.OutputStream.Write(buffer, 0, byteSeq);
                byteSeq = strm.Read(buffer, 0, 2048);
            }
            Response.Flush();
        }

        private void ChangeDocumentImage()
        {
            Response.ContentType = "image/jpeg";
            MemoryStream strm = new MemoryStream((byte[])CollateralCreatorRepository.DocumentDownloadImage(sImageAreaID));

            byte[] buffer = new byte[2048];
            int byteSeq = strm.Read(buffer, 0, 2048);

            Response.Clear();
            Response.ContentType = "image/bmp";

            while (byteSeq > 0)
            {
                Response.OutputStream.Write(buffer, 0, byteSeq);
                byteSeq = strm.Read(buffer, 0, 2048);
            }
            Response.Flush();
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