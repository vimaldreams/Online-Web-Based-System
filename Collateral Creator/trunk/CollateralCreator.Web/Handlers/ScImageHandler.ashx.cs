using CollateralCreator.SQLProvider;

namespace CollateralCreator.Web
{
    using System.Web;

    /// <summary>
    /// Summary description for ScImageHandler
    /// </summary>
    public class ScImageHandler : IHttpHandler
    {
        /// <summary>
        /// Process http request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            #region Code

            string dbImageId = "";
            if (context.Request.QueryString["imageid"] != null)
            {
                dbImageId = context.Request.QueryString["imageid"].ToString();
            }
            //get the image byte array from the DB
            byte[] imageArray = CollateralCreatorRepository.DocumentDownloadImage(dbImageId);

            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;
            context.Response.OutputStream.Write(imageArray, 0, imageArray.Length);
          
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