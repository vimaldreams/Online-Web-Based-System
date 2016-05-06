using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Net;
using System.Configuration;

namespace CollateralCreator.Web.Code
{
    /// <summary>
    /// Main code class, contains helper functions
    /// </summary>
    public class Main
    {
        #region Methods

        /// <summary>
        /// Gets a set of partner logos based on the users associated company ID - This is derived from a Xerox web service developed by Allan Jackson.
        /// </summary>
        /// <param name="companyId">Company ID, from the SC2 querystring</param>
        /// <returns>A generic list of defined partner logo urls</returns>
        public static List<string> GetComapnyIDPartnerBrandedLogos(string companyId)
        {
            #region Code

            List<string> logos = new List<string>();

            try
            {
                string url = ConfigurationManager.AppSettings["PartnerLogoUrl_BPID"].ToString();

                //Create Request
                HttpWebRequest xmlWebResponse = (HttpWebRequest)WebRequest.Create(url + companyId);
                xmlWebResponse.Method = "GET";

                //Get Response
                HttpWebResponse xmlHttpWebResponse = (HttpWebResponse)xmlWebResponse.GetResponse();

                //Now load the XML Document
                XmlDocument xml = new XmlDocument();

                //Load response stream into XMLReader
                XmlTextReader xmlReader = new XmlTextReader(xmlHttpWebResponse.GetResponseStream());

                xml.Load(xmlReader);

                if (xml != null)
                {
                    XmlNodeList xmlList = xml.SelectNodes("/RESELLER/ALL_LOGOS/LOGO");

                    if (xmlList != null)
                    {
                        foreach (XmlNode node in xmlList)
                        {
                            if (node == null) continue;

                            var xmlElement = node["URL"];
                            if (xmlElement != null)
                            {
                                //Do your own stuff here
                                logos.Add(xmlElement.InnerText);
                            }

                        }
                    }
                }

                return logos;
            }

            catch 
            {
                return logos; 
            }

            #endregion
        }

        /// <summary>
        /// Gets a set of partner logos based on the users associated channelpartner ID - This is derived from a Xerox web service developed by Allan Jackson.
        /// </summary>
        /// <param name="channelPartnerId">ChannelPartner ID</param>
        /// <returns>A generic list of defined partner logo urls</returns>
        public static List<string> GetChannelPartnerBrandedLogos(string channelPartnerId)
        {
            #region Code

            List<string> logos = new List<string>();

            try
            {
                string url = ConfigurationManager.AppSettings["PartnerLogoUrl_USER"].ToString();

                //Create Request
                HttpWebRequest xmlWebResponse = (HttpWebRequest)WebRequest.Create(url + channelPartnerId);
                xmlWebResponse.Method = "GET";

                //Get Response
                HttpWebResponse xmlHttpWebResponse = (HttpWebResponse)xmlWebResponse.GetResponse();

                //Now load the XML Document
                XmlDocument xml = new XmlDocument();

                //Load response stream into XMLReader
                XmlTextReader xmlReader = new XmlTextReader(xmlHttpWebResponse.GetResponseStream());

                xml.Load(xmlReader);

                if (xml != null)
                {
                    XmlNodeList xmlList = xml.SelectNodes("/RESELLER/ALL_LOGOS/LOGO");

                    if (xmlList != null)
                    {
                        foreach (XmlNode node in xmlList)
                        {
                            if (node == null) continue;

                            var xmlElement = node["URL"];
                            if (xmlElement != null)
                            {
                                //Do your own stuff here
                                logos.Add(xmlElement.InnerText);
                            }

                        }
                    }
                }

                return logos;
            }

            catch
            {
                return logos;
            }

            #endregion
        }

        #endregion
    }
}