using CollateralCreator.SQLProvider;

namespace CollateralCreator.Web
{
    using System;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Sc2 image upload handler fired from the image sc2 check box - uploads a blank image to any image that cannot be uploaded via the custom upload dialog
    /// </summary>
    public class SrcBlankImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            #region code
            try
            {
                //get the image ID
                string dbImageId = "";
                if (context.Request.QueryString["imageid"] != null)
                {
                    dbImageId = context.Request.QueryString["imageid"].ToString();
                }

                string filePath = context.Request.PhysicalApplicationPath.ToString() + "\\images\\logos\\Blank.jpg";
                //get the blank image from logos
                using (FileStream imageFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {

                    byte[] imgbytes = new Byte[imageFile.Length];

                    int numBytesToRead = (int)imageFile.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead. 
                        int n = imageFile.Read(imgbytes, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached. 
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = imgbytes.Length;

                    CollateralCreatorRepository.DocumentUploadImage(dbImageId, imgbytes);
                }
                string msg = "{";
                msg += string.Format("error:'{0}',\n", string.Empty);
                msg += string.Format("msg:'{0}'\n", string.Empty);
                msg += "}";
                context.Response.Write(msg);
            }
            catch
            {
                string msg = "{";
                msg += string.Format("error:'{0}',\n", "1");
                msg += string.Format("msg:'{0}'\n", "Error uploading file");
                msg += "}";
                context.Response.Write(msg);
            }
           

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