using CollateralCreator.Business;
using Xerox.SSOComponents.Models;

/// <summary>
/// Class to represent a recent activity page within a Xerox application.
/// </summary>
namespace CollateralCreator.Web
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class RecentActivity : XeroxWebPage
    {
        #region Member Variables

        private int ecount = 1;

        #endregion

        #region Events

        /// <summary>
        /// Fires on page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Code

            if (!IsPostBack)
            {
                LoadPageTranslations();
                BreadCrumb();
                BuildJavascript();
                CreateButtons();
            }

            #endregion
        }

        #endregion

        #region Methods
        /// <summary>
        /// Create bread crumb trail
        /// </summary>
        private void BreadCrumb()
        {
            #region Code
            this.lblbreadcrumbdesc.Text = GetLocalResourceObject("BreadCrumbTxt").ToString();
            SiteMapNode node = SiteMap.CurrentNode;
            do
            {
                Literal link = new Literal();
                if (ecount == 1)
                    link.Text = "<span class=\"currentbreadcrumlink\">" + node.Title + "</span>";
                else
                    link.Text = "<a href=\"" + node.Url + "\" class=\"breadcrumlink\">" + node.Title + "</a>";
                    //link.Text = "<span class=\"breadcrumlink\">" + node.Title + "</span>";
                SiteHierarchy.Controls.AddAt(0, link);

                Label label = new Label();
                label.Text = " > ";
                SiteHierarchy.Controls.AddAt(0, label);

                node = node.ParentNode;
                ecount++;
            }
            while (node != null);
            #endregion
        }

        /// <summary>
        /// Create UI Buttons - Javascript
        /// </summary>
        private void CreateButtons()
        {
            #region Code
            this.litDialogBoxCancelButton.Text = "<a id=\"DialogBoxCancelButton\" class=\"dialogclosebutton\" href=\"javascript:CancelPreviewDialog();\">" + GetLocalResourceObject("ClosebtnText").ToString() + "</a>";
            #endregion
        }
        
        /// <summary>
        /// Method to create custom javascript block to create button text on jquery function in menu
        /// </summary>
        private void BuildJavascript()
        {
            #region code

            litJavaScript.Text = "<script type=\"text/javascript\" language=\"javascript\">\n";
            litJavaScript.Text += "var sNoDetailsFlag =\"\";\n";
            litJavaScript.Text += "var sBtnView  =\"" + GetLocalResourceObject("ViewbtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sBtnEdit =\"" + GetLocalResourceObject("EditbtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sBtnClone =\"" + GetLocalResourceObject("ClonebtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sBtnDelete =\"" + GetLocalResourceObject("DeleteBtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sDocumentStatus =\"" + GetLocalResourceObject("GoneToprntText").ToString() + "\";\n";
            litJavaScript.Text += "var sConfirmDeleteMsg =\"" + GetLocalResourceObject("ConfirmDeleteText").ToString() + "\";\n";
            litJavaScript.Text += "var sTodaysDateHeader  =\"" + GetLocalResourceObject("TodayHeaderText").ToString() + "\";\n";
            litJavaScript.Text += "var sLastWeekDateHeader  =\"" + GetLocalResourceObject("LastweekHeaderText").ToString() + "\";\n";
            litJavaScript.Text += "var sPreviousDateHeader  =\"" + GetLocalResourceObject("PreviousHeaderText").ToString() + "\";\n";
            litJavaScript.Text += "var sXerox  =\"" + GetLocalResourceObject("XeroxBrandText").ToString() + "\";\n";
            litJavaScript.Text += "var sPartner  =\"" + GetLocalResourceObject("PartnerBrandText").ToString() + "\";\n";
            litJavaScript.Text += "var sNoDetailMessage =\"" + GetLocalResourceObject("NoDetailsLabel").ToString() + "\";\n";
            litJavaScript.Text += "var sCountry =\"" + this.ChannelPartner.CompanyCountry.ToString().ToLower() + "\";\n";
              
            //check whether the address details are empty
            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0) a = this.ChannelPartner.Addresses[0];
            if (a == null)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";

            //check whether the custom details are empty
            if (ChannelPartner.LocationName == null && ChannelPartner.LocationName == string.Empty)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";
            if (ChannelPartner.Info.Count == 0)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";

            //check whether the logo is default
            Xerox.SSOComponents.Models.Image img = null;
            if (ChannelPartner.Images.Count > 0) img = this.ChannelPartner.Images[0];
            if (img == null) litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";
            litJavaScript.Text += "</script>\n";

            #endregion
        }

        /// <summary>
        ///  Loads page translations for the corresponding language from UI Culture
        /// </summary>
        private void LoadPageTranslations()
        {
            #region code

            this.lblRecentActicityHeader.Text = GetLocalResourceObject("RecentText").ToString();
            this.lblcreatenewCollateral.Text = GetLocalResourceObject("CreateCollateralText").ToString();
            this.lblmyrecentactivity.Text = GetLocalResourceObject("RecentActivityText").ToString();
            this.lblviewmore.Text = GetLocalResourceObject("ShowAllText").ToString();
            this.lblviewless.Text = GetLocalResourceObject("ShowLessText").ToString();
            this.lblpviewmore.Text = GetLocalResourceObject("ShowAllText").ToString();
            this.lblpviewless.Text = GetLocalResourceObject("ShowLessText").ToString();
            this.lblmyprinthouse.Text = GetLocalResourceObject("PrintHouseText").ToString();

            #endregion
        }
        #endregion
    }
    
}