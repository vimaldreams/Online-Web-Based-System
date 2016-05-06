using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CollateralCreator.Business;
using Signals.Translations;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;

namespace CollateralCreator.Web
{
    public partial class LogoControl : XeroxControl
    {
        #region Member variables
        public string Message = string.Empty;
        //TranslationGroup tg;
        #endregion

        #region Events

        /// <summary>
        /// Page load event
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
            this.litButtonUpdateLogoDetails.Text = "<input id=\"buttonuploadlogo\" type=\"button\" value=\"" +  GetGlobalResourceObject("MainText", "UploadbtnText").ToString()  + "\" class=\"button\" onclick=\"UpdateLogoDialog();\"/>";
            this.litDialogBoxCancelButton.Text = "<input id=\"DialogBoxCancelButton\" type=\"button\" class=\"dialogbutton\" onclick=\"CancelLogoDialog();\" value=\"" +  GetGlobalResourceObject("MainText", "CancelbtnText").ToString() + "\" />";
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildJavascript()
        {
            #region Code
            litJavaScript.Text = "<script type=\"text/javascript\" language=\"javascript\">\n";
            litJavaScript.Text += "var sFileTypeErrorMsg =\"" + GetGlobalResourceObject("MainText", "ValidFileTypeMsg").ToString() + "\";\n";
            litJavaScript.Text += "var sSelectFileErrorMsg =\"" + GetGlobalResourceObject("MainText", "SelectFileMsg").ToString() + "\";\n";
            litJavaScript.Text += "var sbtnUploadText =\"" + GetGlobalResourceObject("MainText", "UploadbtnText").ToString()  + "\";\n";
            litJavaScript.Text += "</script>\n";
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadTranslations()
        {
            #region Code
            //tg = GetTranslationGroup("UI", DisplayCountryId, DisplayLanguageId);

            this.lblLogo.Text = GetGlobalResourceObject("MainText", "LogoLabel").ToString();
            this.lblLogoDesc.Text = GetGlobalResourceObject("MainText", "LogoLabelDescText").ToString();
            this.lblLogoSizeDesc.Text = GetGlobalResourceObject("MainText", "LogoSizeDescText").ToString();
            this.hiddenLoginID.Value = this.ChannelPartner.LoginId;
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void ShowAlertMessage(string error)
        {
            #region Code
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                error = error.Replace("'", "\'");
                ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');", true);
            }
            #endregion
        }
        #endregion
    }
}