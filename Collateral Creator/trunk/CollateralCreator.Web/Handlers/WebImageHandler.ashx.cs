using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using CollateralCreator.SQLProvider;

namespace CollateralCreator.Web.Handlers
{
    /// <summary>
    /// Summary description for WebImageHandler
    /// </summary>
    public class WebImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            #region code

            string imageAreaID = string.Empty; string imageUrl = string.Empty;

            if (context.Request.QueryString["imageAreaID"] != null)
            {
                imageAreaID = context.Request.QueryString["imageAreaID"].ToString();
            }

            if (context.Request.QueryString["imageUrl"] != null)
            {
                imageUrl = context.Request.QueryString["imageUrl"].ToString();
            }

            //downloadimage as bytes
            var webClient = new WebClient();

            byte[] imageBytes = webClient.DownloadData(imageUrl);

            //upload image into the database
            CollateralCreatorRepository.DocumentUploadImage(imageAreaID, imageBytes);
            
            string msg = "Image uploaded successfully. Click update preview to view the changes."; 

            context.Response.Write(msg);

            #endregion
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