using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Xerox.SSOComponents;
using Xerox.SSOComponents.Models;
using Xerox.SSOComponents.Models.Enumerations;
using CollateralCreator.Business;

namespace CollateralCreator.Web.Templates
{
    public partial class PortalHeader : XeroxControl
    {
        public IEnumerable<Application> Applications
        {
            get; set;
        }

        protected IEnumerable<string> Reports
        {
            get;
            set;
        }

        public string PortalUrl
        {
            get;
            set;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lnkHome.HRef = string.Format("{0}Home/ChangeApp?appTag=0", this.PortalUrl);
            this.lnkHome.Title = GetGlobalResourceObject("MainText", "IMPACTHome").ToString();
            this.lnkHome.InnerText = GetGlobalResourceObject("MainText", "IMPACTHome").ToString();

            this.lnkAppHome.HRef = VirtualPathUtility.ToAbsolute("~/Home.aspx");
            this.lnkAppHome.Title = GetGlobalResourceObject("MainText", "Home").ToString();
            this.lnkAppHome.InnerText = GetGlobalResourceObject("MainText", "Home").ToString();

            this.lnkProfile.HRef = string.Format("{0}Account/Profile", this.PortalUrl);
            this.lnkProfile.Title = GetGlobalResourceObject("MainText", "MyProfile").ToString();
            this.lnkProfile.InnerText = GetGlobalResourceObject("MainText", "MyProfile").ToString();

            this.lnkTutorial.HRef = string.Format("{0}Help/Tutorial", this.PortalUrl);
            this.lnkTutorial.Title = GetGlobalResourceObject("MainText", "Tutorial").ToString();
            this.lnkTutorial.InnerText = GetGlobalResourceObject("MainText", "Tutorial").ToString();

            this.lnkHelp.HRef = string.Format("mailto:business.resource.center@xerox.com");
            this.lnkHelp.Title = GetGlobalResourceObject("MainText", "Feedback").ToString();
            this.lnkHelp.InnerText = GetGlobalResourceObject("MainText", "Feedback").ToString();
        }
    }
}