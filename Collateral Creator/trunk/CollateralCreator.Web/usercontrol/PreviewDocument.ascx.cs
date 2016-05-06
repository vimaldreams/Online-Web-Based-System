using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;

using Signals.Translations;

namespace CollateralCreator.Web
{
    public partial class PreviewDocument : XeroxControl
    {
        #region Member variables
        public static string DocumentName { get; set; }
        public static int DocumentId { get; set; }
        public static int TemplateId { get; set; }
        public static bool PartnerBrand { get; set; }
        #endregion

        #region Events

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Code
            if (!IsPostBack)
            {
                LoadTranslations();

                BuildJavascript();

                CreateButtons();
            }
            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void CreateButtons()
        {
            #region Code
            if (PartnerBrand)
                this.litPreviewImage.Text = "<img src=\"" + VirtualPathUtility.ToAbsolute("~/images/templates/partnerbrand/thumbnail_" + TemplateId + ".jpg?v=" + DateTime.Now.ToString("yyyyMMdd")) + "\" class=\"imgdocument\" alt=\"brand_template\"/>";
            else
                this.litPreviewImage.Text = "<img src=\"" + VirtualPathUtility.ToAbsolute("~/images/templates/xeroxbrand/thumbnail_" + TemplateId + ".jpg?v=" + DateTime.Now.ToString("yyyyMMdd")) + "\" class=\"imgdocument\" alt=\"brand_template\"/>";

            this.litPreviewButton.Text = "<input id=\"btnviewproof\" type=\"button\" class=\"button\" value=\"" + GetGlobalResourceObject("MainText", "ViewPrrofbtnText").ToString() + "\" onclick=\"javascript:PreviewDocument(" + DocumentId + ", '" + DocumentName + "');\"/>";
            this.litDialogBoxCancelButton.Text = "<a id=\"DialogBoxCancelButton\" class=\"dialogclosebutton\" href=\"javascript:CancelPreviewDialog();\">" + GetGlobalResourceObject("MainText", "ClosebtnText").ToString() + "</a>";
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildJavascript()
        {
            #region Code
            litJavaScript.Text = "<script type=\"text/javascript\" language=\"javascript\">\n";
            //litJavaScript.Text += "var sframewindow =\"" + DocumentName + "\";\n";
            litJavaScript.Text += "</script>\n";
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadTranslations()
        {
            #region Code
            //tg = GetTranslationGroup("OrderDocument", DisplayCountryId, DisplayLanguageId);

            this.lblproofheader.Text = GetGlobalResourceObject("MainText", "ProofHeaderLabel").ToString();
            this.lblproofdescription.Text = GetGlobalResourceObject("MainText", "ProofDescLabel").ToString(); 
            this.lbladobeheader.Text = GetGlobalResourceObject("MainText", "AdobeHeaderLabel").ToString(); 
            this.lbladobedescription.Text = GetGlobalResourceObject("MainText", "AdobeDescLabel").ToString();
            #endregion
        }

        #endregion
    }
}