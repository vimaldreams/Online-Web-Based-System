using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace CollateralCreator.Business
{
    public class EmailManager
    {
        #region Properties
        public string DefaultFrom { get; set; }
        public string DefaultTo { get; set; }
        public string[] DefaultCC { get; set; }
        public string[] DefaultBCC { get; set; }
        public string DefaultSubject { get; set; }
        public string DefaultBody { get; set; }
        #endregion

        #region Methods

        public EmailManager()
        {
            // Get default values from web.config
            if (ConfigurationManager.AppSettings["fromEmail"] != null &&
                ConfigurationManager.AppSettings["fromEmail"].Length > 0)
            {
                this.DefaultFrom = ConfigurationManager.AppSettings["fromEmail"];
            }
            if (ConfigurationManager.AppSettings["toEmail"] != null &&
                ConfigurationManager.AppSettings["toEmail"].Length > 0)
            {
                this.DefaultTo = ConfigurationManager.AppSettings["toEmail"];
            }
            if (ConfigurationManager.AppSettings["ccEmail"] != null &&
                ConfigurationManager.AppSettings["ccEmail"].Length > 0)
            {
                this.DefaultCC = ConfigurationManager.AppSettings["ccEmail"].Split(',');
            }
            if (ConfigurationManager.AppSettings["bccEmail"] != null &&
                ConfigurationManager.AppSettings["bccEmail"].Length > 0)
            {
                this.DefaultBCC = ConfigurationManager.AppSettings["bccEmail"].Split(',');
            }
        }

        public void Post(String subject, String body, Boolean isBodyHtml)
        {
            Post(this.DefaultFrom, this.DefaultTo, this.DefaultCC, this.DefaultBCC, subject, body, isBodyHtml);
        }

        public void Post(String to, String subject, String body, Boolean isBodyHtml)
        {
            Post(this.DefaultFrom, to, this.DefaultCC, this.DefaultBCC, subject, body, isBodyHtml);
        }

        public void Post(String from, String to, String[] cc, String[] bcc, String subject, String body, Boolean IsBodyHtml)
        {
            try
            {
                if (from == string.Empty)
                {
                    from = this.DefaultFrom;
                }
                
                using (MailMessage message = new MailMessage(from, to, subject, body))
                {
                    if (cc != null)
                    {
                        foreach (String address in cc)
                        {
                            message.CC.Add(new MailAddress(address.Trim()));
                        }
                    }

                    if (bcc != null)
                    {
                        foreach (String address in bcc)
                        {
                            message.Bcc.Add(new MailAddress(address.Trim()));
                        }
                    }

                    message.IsBodyHtml = IsBodyHtml;
                    message.Priority = MailPriority.High;

                    var client = new SmtpClient();

                    //var ctx = System.Web.HttpContext.Current;
                    // If the server is localhost (ie development environment)
                    // Switch host ip to localhost
                    //if (ctx.Request.ServerVariables["SERVER_NAME"] == "localhost")
                    //{
                    //    //client.Host = "127.0.0.1";
                    //    client.Host = "192.168.16.8";
                    //}

                    client.Host = ConfigurationManager.AppSettings["MailServer"];

                    // Send the built email
                    client.Send(message);
                }
            }
            catch (InvalidOperationException ex)
            {
                // Write error to debug window
                System.Diagnostics.Debug.Write(ex.Message);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                // Write error to debug window
                System.Diagnostics.Debug.Write(ex.Message);
            }
            catch (SmtpException ex)
            {
                // Write error to debug window
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }

        #endregion

        #region support functions

        private static string MailHelper_HostAddress { get { return ConfigurationManager.AppSettings["MailHelper.HostAddress"]; } }

        private static int MailHelper_HostPort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["MailHelper.HostPort"]); } }

        public string concat(string title, string text)
        {
            return string.Concat("<p>", "<strong>" + title + "</strong>", text, "</p>");
        }

        public string text(string text)
        {
            return string.Concat("<p>", text, "</p>");
        }

        #endregion
    }
}
