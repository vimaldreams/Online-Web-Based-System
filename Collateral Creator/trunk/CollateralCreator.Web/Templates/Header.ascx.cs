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
    public partial class Header : XeroxControl
    {
        //public IEnumerable<Application> Applications
        //{
        //    get;
        //    set;
        //}

        //protected IEnumerable<string> Reports
        //{
        //    get;
        //    set;
        //}

        //public string PortalUrl
        //{
        //    get;
        //    set;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.lnkAppHome.HRef = VirtualPathUtility.ToAbsolute("~/Home.aspx");
            //this.lnkAppHome.Title = "Home";
            //this.lnkAppHome.InnerText = "Home";
        }
    }
}