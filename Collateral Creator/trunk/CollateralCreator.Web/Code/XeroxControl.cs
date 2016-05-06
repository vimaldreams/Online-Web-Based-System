using System.Web.UI.WebControls;
using CollateralCreator.Web.Templates;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;
using Signals.Translations;

/// <summary>
/// Base class for all user controls to inherit from 
/// </summary>
namespace CollateralCreator.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Configuration;

    public class XeroxControl : System.Web.UI.UserControl
    {
        #region Member Variables

        private System.Collections.Hashtable translations = null;

        // These are country/language settings for translations
        // Set as english for default but will be updated from 
        // any valid EventWebsite settings
        // Possibly allow for Reseller override ?
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

        ChannelPartner _channelPartner = null;

        protected ChannelPartner ChannelPartner
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

                    if (_channelPartner != null)
                    {
                        this._displayLanguageId = _channelPartner.LanguageId;
                        this._displayCountryId = _channelPartner.CountryId;
                    }
                }
                return _channelPartner;
            }
            set
            {
                _channelPartner = value;
            }
        }

        ChannelPartnerService _channelPartnerService = null;

        public ChannelPartnerService ChannelPartnerService
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

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageInit(object sender, System.EventArgs e)
        {
            /*LoadCCSSInfo();
            BuildSiteLinks();
            /*CreateNavigationTabs();
            CreatePageIntroduction();*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageLoad(object sender, System.EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageError(object sender, System.EventArgs e)
        {
            Exception currentError = Server.GetLastError();

            HttpContext context = HttpContext.Current;

        }
        
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        protected XeroxControl()
        {
            #region code
            this.Init += new System.EventHandler(this.PageInit);
            this.Load += new System.EventHandler(this.PageLoad);
            this.Error += new System.EventHandler(this.PageError);
            this.translations = new System.Collections.Hashtable();
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
                tg = TranslationManager.Instance(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()).GetTranslations(GroupName,
                                                                                                 CountryId,
                                                                                                 LanguageId);
                translations.Add(key, tg);
            }

            return tg;

            #endregion
        }

        #endregion
    }
}