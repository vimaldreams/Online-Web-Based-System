using System;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace CollateralCreator.Business
{
    public class AppSettings
    {
        private static string GetConfigSetting(string sSetting)
        {
            return ConfigurationManager.AppSettings[sSetting];
        }

        public static string DbConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            }
        }

        public static string WhiteList
        {
            get
            {
                if (!string.IsNullOrEmpty(GetConfigSetting("WhiteList")))
                {
                    return GetConfigSetting("WhiteList");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static CultureInfo GetLikelyCulture(string countryID, string languageID, string defaultCulture)
        {
            switch (languageID)
            {
                case "1":
                    switch (countryID)
                    {
                        case "1":
                            return new CultureInfo("en-GB", true);
                        case "19":
                            return new CultureInfo("en-IE", true);
                        default:
                            System.Diagnostics.Debug.WriteLine("Unable to get culture for user");
                            return new CultureInfo("en-GB", true);
                    }
                case "2":
                    switch (countryID)
                    {
                        case "2":
                            return new CultureInfo("fr-BE", true);
                        case "5":
                            return new CultureInfo("fr-FR", true);
                        default:
                            System.Diagnostics.Debug.WriteLine("Unable to get culture for user");
                            return new CultureInfo("en-GB", true);
                    }
                case "3":
                    return new CultureInfo("it-IT", true);
                case "4":
                    switch (countryID)
                    {
                        case "4":
                            return new CultureInfo("de-DE", true);
                        case "13":
                            return new CultureInfo("de-AT", true);
                        case "18":
                            return new CultureInfo("de-CH", true);
                        default:
                            System.Diagnostics.Debug.WriteLine("Unable to get culture for user");
                            return new CultureInfo("en-GB", true);
                    }
                case "5":
                    return new CultureInfo("nl-NL", true);
                case "7":
                    return new CultureInfo("nn-NO", true);
                case "8":
                    return new CultureInfo("sv-SE", true);
                case "9":
                    return new CultureInfo("fi-FI", true);
                case "10":
                    return new CultureInfo("es-ES", true);
                case "11":
                    return new CultureInfo("da-DK", true);
                case "13":
                    return new CultureInfo("nl-BE", true);
                case "17":
                    return new CultureInfo("pt-PT", true);
                default:
                    System.Diagnostics.Debug.WriteLine("Unable to get culture for user");
                    return new CultureInfo("en-GB", true);
            }
        }

        public static CultureInfo GetLikelyCulture(int CountryId, int LanguageId, string defaultCulture)
        {
            CultureInfo c = new CultureInfo(defaultCulture, true);

            switch (CountryId)
            {
                case 1:
                    switch (LanguageId)
                    {
                        case 1: c = new CultureInfo("en-GB", true); break;
                        case 19: c = new CultureInfo("en-IE", true); break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Unable to get culture for user"); break;
                    }
                    break;
                case 2:
                    switch (LanguageId)
                    {
                        case 2: c = new CultureInfo("fr-BE", true); break;
                        case 5: c = new CultureInfo("fr-FR", true); break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Unable to get culture for user"); break;
                    }
                    break;
                case 3: c = new CultureInfo("it-IT", true); break;
                case 4:
                    switch (LanguageId)
                    {
                        case 4: c = new CultureInfo("de-DE", true); break;
                        case 13: c = new CultureInfo("de-AT", true); break;
                        case 18: c = new CultureInfo("de-CH", true); break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Unable to get culture for user"); break;
                    }
                    break;
                case 5: c = new CultureInfo("nl-NL", true); break;
                case 7: c = new CultureInfo("nn-NO", true); break;
                case 8: c = new CultureInfo("sv-SE", true); break;
                case 9: c = new CultureInfo("fi-FI", true); break;
                case 10: c = new CultureInfo("es-ES", true); break;
                case 11: c = new CultureInfo("da-DK", true); break;
                case 12: c = new CultureInfo("el-GR", true); break;
                case 13: c = new CultureInfo("nl-BE", true); break;
                case 17: c = new CultureInfo("pt-PT", true); break;
                case 170: c = new CultureInfo("en-US", true); break;
                case 172: c = new CultureInfo("en-CA", true); break;
                //Need to set more for Canada

                default:
                    System.Diagnostics.Debug.WriteLine("Unable to get culture for user");
                    break;
            }

            return c;
        }
        
        public static string GetApplicationRoot
        {
            get
            {
                string Port = "";
                if (HttpContext.Current.Request.Url.Port != 80 && HttpContext.Current.Request.Url.Port != 443)
                {
                    Port = ":" + HttpContext.Current.Request.Url.Port;
                }

                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    return HttpContext.Current.Request.Url.Scheme +
                           "://" +
                           HttpContext.Current.Request.Url.Host +
                           Port +
                           "/";
                }
                else
                {
                    return HttpContext.Current.Request.Url.Scheme +
                           "://" +
                           HttpContext.Current.Request.Url.Host +
                           Port +
                           HttpContext.Current.Request.ApplicationPath +
                           "/";
                }
            }
        }
        
        public static string PortalAutoSignInUrl
        {
            get
            {
                return GetConfigSetting("PortalAutoSignInUrl");
            }
        }

        public static string PortalUrl
        {
            get
            {
                return GetConfigSetting("PortalUrl");
            }
        }
    }
}
