using Signals.Translations;
using CollateralCreator.Web.Templates;
using CollateralCreator.Business;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;

/// <summary>
/// Base class for all pages to inherit from 
/// </summary>
namespace CollateralCreator.Web
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Configuration;
    using System.Threading;
    using System.Globalization;

    public class XeroxWebPage : Page
    {
        #region Member Variables

        private System.Collections.Hashtable translations = null;
        protected string GlobalVariable { get; set; }
        
        /// <summary>
        // These are country/language settings for translations
        // Set as english for default but will be updated from 
        // any valid EventWebsite settings
        // Possibly allow for Reseller override ?
        /// </summary>
        private int _displayCountryId = 1;

        protected int DisplayCountryId
        {
            get
            {
                return _displayCountryId;
            }
            set
            {
                _displayCountryId = value;
            }
        }

        private int _displayLanguageId = 1;

        protected int DisplayLanguageId
        {
            get
            {
                if (Request["overridelang"] != null)
                {
                    Int32.TryParse(Request["overridelanguage"], out _displayLanguageId);

                }

                return _displayLanguageId;
            }
            set
            {
                _displayLanguageId = value;
            }
        }

        public ChannelPartner channelPartnerPortal = null;

        protected ChannelPartner ChannelPartner
        {
            get
            {
                if (channelPartnerPortal != null)
                {
                    return channelPartnerPortal;
                }

                if (!string.IsNullOrEmpty(Context.User.Identity.Name))
                {
                    if (_channelPartnerService == null) _channelPartnerService = new ChannelPartnerService(new ChannelPartnerRepository(ConfigurationManager.ConnectionStrings["XeroxPortal"].ToString()));

                    channelPartnerPortal = _channelPartnerService.Retrieve(Context.User.Identity.Name);

                    if (channelPartnerPortal != null)
                    {
                        this._displayLanguageId = channelPartnerPortal.LanguageId;
                        this._displayCountryId = channelPartnerPortal.CountryId;
                    }
                }
                return channelPartnerPortal;
            }
            set 
            {
                channelPartnerPortal = value; 
            }
        }

        public ChannelPartnerService _channelPartnerService = null;

        protected ChannelPartnerService ChannelPartnerService
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

        private int redirect = 0;

        public int Redirect
        {
            get
            {
                if (Request["redirect"] != null)
                {
                    Int32.TryParse(Request["redirect"], out redirect);

                }

                return redirect;
            }
            set
            {
                redirect = value;
            }
        }
               
        /// Detecting Session Timeout And Redirect To Login Page      
        private int SessionLengthMinutes
        {
            get { return Session.Timeout; }
        }

        private string SessionExpireDestinationUrl
        {
            get 
            {
                return VirtualPathUtility.ToAbsolute("~/Shared/NotAuthenticated.aspx"); 
            }
        }

        private bool OnSecureIp
        {
           get
           {
               var found = false;

               if (AppSettings.WhiteList.Length > 0)
               {
                   var ip = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

                   if (!string.IsNullOrEmpty(ip))
                   {
                       foreach (string s in AppSettings.WhiteList.Split(','))
                       {
                           if (s == ip)
                           {
                               found = true;
                               break;
                           }
                       }
                   }
               }

               return found;
           }
       }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_PreInit(object sender, EventArgs e)
        {
            #region code

            //Check for Admin & Normal Users
            if (this.ChannelPartner == null || this.ChannelPartner.Backpack == null ||
                !this.ChannelPartner.Backpack.ContainsKey("featuresetregion") ||
                this.ChannelPartner.Backpack["featuresetregion"] != "NARS") return;

            if (this.ChannelPartner.Backpack.ContainsKey("catalog_group") &&
                ChannelPartner.Backpack["catalog_group"].ToLower() == "us_res_dirresp")
            {
                // Normal

            }
            else
            {
                this.MasterPageFile = "~/templates/Portal.master";
                LoadPortalStuff();
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageInit(object sender, System.EventArgs e)
        {
            #region code

            BuildSiteLinks();

            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            #region code

            base.OnPreRender(e);
            this.Page.Controls.Add(new LiteralControl(
                String.Format("<meta http-equiv='refresh' content='{0};url={1}'>",
                SessionLengthMinutes * 60, SessionExpireDestinationUrl)));

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageLoad(object sender, System.EventArgs e)
        {
            #region code
            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        protected XeroxWebPage()
        {
            #region code

            this.PreInit += new System.EventHandler(this.Page_PreInit);
            this.Init += new System.EventHandler(this.PageInit);
            this.Load += new System.EventHandler(this.PageLoad);
            this.Error += new System.EventHandler(this.PageError);
            
            this.translations = new System.Collections.Hashtable();

            #endregion
        }
                
        /// <summary>
        ///  On the page InitializeCulture event, check to see if the session culture has been set via a backpack call
        /// </summary>
        protected override void InitializeCulture()
        {
            #region code
           
            if (Session["CustomCultureCode"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CustomCultureCode"].ToString(), true);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CustomCultureCode"].ToString(), true);
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadPortalStuff()
        {
            #region code

            ApplicationService applicationService = new ApplicationService(ConfigurationManager.ConnectionStrings["XeroxPortal"].ToString());

            PortalHeader lit = (PortalHeader)FindControlRecursive(this.Master, "PortalHeader");
            
            if (lit != null)
            {
                // Get list of applications from portal service
                lit.Applications = applicationService.GetChannelPartnerApplications(ChannelPartner.LoginId, ChannelPartner.Backpack) as IEnumerable<Application>;

                // Get portal url from appSettings.config
                if (ConfigurationManager.AppSettings["PortalUrl"] != null)
                {
                    lit.PortalUrl = ConfigurationManager.AppSettings["PortalUrl"];
                }
            }

            Literal litHeaderText = (Literal)FindControlRecursive(this.Master, "litHeaderText");
            if (litHeaderText != null)
            {
                litHeaderText.Text = "<span class=\"title\" id=\"xrx_bnr_partner_title\">" + GetGlobalResourceObject("MainText", "XeroxTitle").ToString() + "</span>";
            }

            Literal litState = (Literal)FindControlRecursive(this.Master, "litState");

            if (litState != null)
            {

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append("<script type=\"text/javascript\">").AppendLine();
                stringBuilder.AppendFormat("var message = '{0}';", "Warning, by leaving this page now you will lose any changes you have made").AppendLine();
                stringBuilder.Append("window.onbeforeunload = function() { ");
                stringBuilder.Append(" if(typeof checkLeavePage == 'function') { if (!checkLeavePage()) { return message; } }");
                stringBuilder.Append("};").AppendLine();

                stringBuilder.Append("</script>").AppendLine();

                litState.Text = stringBuilder.ToString();
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageError(object sender, System.EventArgs e)
        {
            #region code

            Exception currentError = Server.GetLastError();

            HttpContext context = HttpContext.Current;

            #endregion

        }

        /// <summary>
        /// Check to see if the current user is on an ip which
        /// is deemed as secure to view administration pages.
        /// User is redirected if they are not within the 
        /// allowed list.
        /// </summary>
        public void RedirectIfNotSecure()
        {
            #region code

            if (!OnSecureIp)
            {
                Response.Redirect("~/401");
            }

            #endregion
        }             
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="CountryId"></param>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        protected TranslationGroup GetTranslationGroup(string GroupName, int CountryId, int LanguageId)
        {
            #region code

            var tg = new TranslationGroup();
            string key = GroupName + "|" + CountryId + "|" + LanguageId;

            if (translations.ContainsKey(key))
            {
                tg = (TranslationGroup)translations[key];
            }
            else
            {
                tg = TranslationManager.Instance(AppSettings.DbConnectionString).GetTranslations(GroupName,
                                                                                                 CountryId,
                                                                                                 LanguageId);
                translations.Add(key, tg);
            }

            return tg;

            #endregion
        }
               
        /// <summary>
        /// 
        /// </summary>
        private void BuildSiteLinks()
        {
            #region code

            TranslationGroup tg = GetTranslationGroup("Content", DisplayCountryId, DisplayLanguageId);

            HtmlAnchor lnkHome = (HtmlAnchor)FindControlRecursive(this, "lnkHome");
            if (lnkHome != null)
            {
                lnkHome.HRef = "javascript:history.back(1);";
                lnkHome.InnerText = "Home";
            }

            if (ChannelPartner == null) return;

            string sSysManagers = ConfigurationManager.AppSettings.Get("adminUsers");
            HtmlAnchor lnkAdmin = (HtmlAnchor)FindControlRecursive(this, "lnkAdmin");
            
            Literal litHeaderText = (Literal)FindControlRecursive(this.Master, "litHeaderText");
            if (litHeaderText != null)
            {
                if ((Request["CollateralID"] != null) || (Request["Type"] != null && Request["Type"] == "SmartCentre"))
                {
                    litHeaderText.Text = "<span class=\"title\" id=\"xrx_bnr_partner_title\">" + GetGlobalResourceObject("MainText", "SmartCentreTitle").ToString() + "</span>";
                }
                else
                {
                    litHeaderText.Text = "<span class=\"title\" id=\"xrx_bnr_partner_title\">" + GetGlobalResourceObject("MainText", "XeroxTitle").ToString() + "</span>";
                }
            }
            
            //HtmlAnchor lnkAdminline = (HtmlAnchor)FindControlRecursive(this, "lnkAdminline");
            //if (lnkAdminline != null)
            //    lnkAdminline.Visible = true;

            //if ((lnkAdmin != null) && (ChannelPartner.Email != null) && (ChannelPartner.Email != string.Empty))
            //{
            //    if (sSysManagers.ToLower().Contains(ChannelPartner.Email.ToLower()))
            //    {
            //        lnkAdmin.Visible = false;
            //        lnkAdmin.HRef = VirtualPathUtility.ToAbsolute("~/Portal/AdminOrderScreen.aspx");
            //    }
            //    //if ((lnkAdminline != null))
            //    //{
            //    //    lnkAdmin.Visible = false;
            //    //    lnkAdminline.Visible = false;
            //    //}
            //}
            //else
            //{
            //    lnkAdmin.Visible = false;
            //    lnkAdminline.Visible = false;
            //}

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Control FindControlRecursive(Control root, string id)
        {
            #region code

            if (root.ID == id)
            {
                return root;
            }

            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, id);

                if (t != null)
                {
                    return t;
                }
            }

            return null;

            #endregion
        }

        #endregion
    }
}
