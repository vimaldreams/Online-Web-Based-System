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
    public partial class AddressControl : XeroxControl
    {
      
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

                LoadContactDetails();
            }
            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void BuildJavascript()
        {
            #region Code
            litJavaScript.Text = "<script type=\"text/javascript\" language=\"javascript\">\n";
            litJavaScript.Text += "var sContactTitle =\"" + GetGlobalResourceObject("MainText", "ContactdialogTitle").ToString() + "\";\n";
            litJavaScript.Text += "var sCustomTitle =\"" + GetGlobalResourceObject("MainText", "CustomDetailsLabel").ToString() + "\";\n";
            litJavaScript.Text += "var sContactSuccessMsg =\"" + GetGlobalResourceObject("MainText", "ContactUpdateSuccessMsg").ToString() + "\";\n";
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
            this.lblContactdetails.Text =  GetGlobalResourceObject("MainText", "ContactDetailsLabel").ToString() ;
            this.lblContactDesc.Text = GetGlobalResourceObject("MainText", "ContactDetailsDescText").ToString() ;
            this.lblCustomdetails.Text = GetGlobalResourceObject("MainText", "CustomDetailsLabel").ToString() ;
            this.lblCustomdescription.Text = GetGlobalResourceObject("MainText", "CustomDetailsDescText").ToString() ;

            this.lblAddress1.Text = GetGlobalResourceObject("MainText", "Address1Label").ToString() ;
            this.lblAddress2.Text = GetGlobalResourceObject("MainText", "Address2Label").ToString();
            this.lblAddress3.Text = GetGlobalResourceObject("MainText", "Address3Label").ToString() ;
            this.lblTown.Text = GetGlobalResourceObject("MainText", "TownLabel").ToString() ;
            this.lblState.Text = GetGlobalResourceObject("MainText", "StateLabel").ToString() ;
            this.lblCountry.Text = GetGlobalResourceObject("MainText", "CountryLabel").ToString() ;
            this.lblPostcode.Text = GetGlobalResourceObject("MainText", "PostcodeLabel").ToString() ;

            this.tblblCompanyname.Text = GetGlobalResourceObject("MainText", "CompanyLabel").ToString() ;
            this.tblblWebUrl.Text = GetGlobalResourceObject("MainText", "UrlLabel").ToString() ;
            this.tblblPhonenumber.Text = GetGlobalResourceObject("MainText", "PhoneLabel").ToString() ;
            this.DialogBoxCustomSaveButton.Value = GetGlobalResourceObject("MainText", "SavebtnText").ToString();
            this.DialogBoxAddressSaveButton.Value = GetGlobalResourceObject("MainText", "SavebtnText").ToString();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateButtons()
        {
            #region Code
            this.litButtonUpdateContactDetails.Text = "<input id=\"btnUpdateContactDetails\" type=\"button\" value=\"" + GetGlobalResourceObject("MainText", "UpdatebtnText").ToString() + "\" class=\"button\" onclick=\"UpdateAddressDialog();\"/>";
            this.litButtonUpdateCustomDetails.Text = "<input id=\"btnUpdateCustomDetails\" type=\"button\" value=\"" + GetGlobalResourceObject("MainText", "UpdatebtnText").ToString()  + "\" class=\"button\" onclick=\"UpdateCustomDialog();\"/>";
            this.litDialogBoxAddressCancelButton.Text = "<input id=\"DialogBoxAddressCancelButton\" type=\"button\" class=\"dialogbutton\" onclick=\"CancelAddressDialog();\" value=\"" +  GetGlobalResourceObject("MainText", "CancelbtnText").ToString()  + "\" />";
            this.litDialogBoxCustomCancelButton.Text = "<input id=\"DialogBoxCustomCancelButton\" type=\"button\" class=\"dialogbutton\" onclick=\"CancelCustomDialog();\" value=\"" + GetGlobalResourceObject("MainText", "CancelbtnText").ToString()   + "\" />";
            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void LoadContactDetails()
        {
            #region Code
            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0) a = this.ChannelPartner.Addresses[0];

            this.hiddentxtLoginID.Value = this.ChannelPartner.LoginId;

            StringBuilder address = new StringBuilder(1000);
            if (a != null)
            {
                if (a.AddressLine1 != null && a.AddressLine1 != string.Empty)
                {
                    address.Append(a.AddressLine1);
                    hiddentxtAddressline1.Value = a.AddressLine1;
                }

                if (a.AddressLine2 == string.Empty && a.AddressLine3 == string.Empty && a.Town.Length == 2)
                    address.Append(", " + a.Town);

                if (a.AddressLine2 != null && a.AddressLine2 != string.Empty)
                {
                    address.AppendFormat("\r\n");
                    address.Append(a.AddressLine2);
                    hiddentxtAddressline2.Value = a.AddressLine2;
                }

                if (a.AddressLine3 == string.Empty && a.Town.Length == 2)
                    address.Append(", " + a.Town);

                if (a.AddressLine3 != null && a.AddressLine3 != string.Empty)
                {
                    address.AppendFormat("\r\n");
                    address.Append(a.AddressLine3);
                    hiddentxtAddressline3.Value = a.AddressLine3;
                }
                if (a.Town != null && a.Town != string.Empty && a.Town.Length > 2)
                {
                    address.AppendFormat("\r\n");
                    address.Append(a.Town);
                    hiddentxtTown.Value = a.Town;
                }

                if (a.State != string.Empty && a.State.Length == 2)
                     address.Append(", " + a.State);

                if (a.State != null && a.State != string.Empty && a.State.Length > 2)
                {
                    address.AppendFormat("\r\n");
                    address.Append(a.State);
                    hiddentxtState.Value = a.State;
                }

                if (a.Country != string.Empty && a.Country.Length == 2)
                    address.Append(", " + a.Country);

                if (a.Country != null && a.Country != string.Empty && a.Country.Length > 2)
                {
                    address.AppendFormat("\r\n");
                    address.Append(a.Country);
                    hiddentxtCountry.Value = a.Country;
                }
                if (a.PostCode != null && a.PostCode != string.Empty)
                {
                    address.AppendFormat("\r\n");
                    address.Append(a.PostCode);
                    hiddentxtPostcode.Value = a.PostCode;
                }

                txtContactDetails.Value = address.ToString();
            }
            
            //get the custom info details
            StringBuilder custominfo = new StringBuilder(1000);
            if (ChannelPartner.LocationName != null && ChannelPartner.LocationName != string.Empty)
            {
                custominfo.Append(ChannelPartner.LocationName);
                hiddenCompanyname.Value = ChannelPartner.LocationName;
            }
            
            if (ChannelPartner.Info.Count > 0)
            {
                foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                {
                    hiddenChannelPartnerInfoID.Value += inf.ChannelPartnerInfoID + ", ";

                    if (inf.Name.ToLower().Contains("phone"))
                    {
                        custominfo.AppendFormat("\r\n");
                        custominfo.Append(inf.Value);
                        hiddenPhonenumber.Value = inf.Value;
                    }
                        
                    if (inf.Name.ToLower().Contains("website"))
                    {
                        custominfo.AppendFormat("\r\n");
                        custominfo.Append(inf.Value);
                        hiddenWebUrl.Value = inf.Value;
                    }
                }
            }
            
            txtCustomDetails.Value = custominfo.ToString();
            #endregion

        }
        #endregion
    }
}