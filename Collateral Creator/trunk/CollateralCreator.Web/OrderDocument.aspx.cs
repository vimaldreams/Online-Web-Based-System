using iTextSharp.text;
using CollateralCreator.Data;
using CollateralCreator.Business;
using CollateralCreator.SQLProvider;
using Xerox.SSOComponents.Models;

/// <summary>
/// Class to represent a order page within a Xerox application.
/// </summary>
namespace CollateralCreator.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class OrderDocument : XeroxWebPage
    {
        #region Member Variables

        private Int16 templateId = 0;
        private int documentId = 0;
        private string documentName = string.Empty;
        private Boolean partnerBrand = false;
        private CollateralCreator.Data.Document objDocument = null;
        private string templateName = string.Empty;
        private string errorMessage = string.Empty;
        private string defaultLogoMsg = string.Empty;
        private string languageCode { get; set; }
        
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
                ProcessVars();
                LoadDataElements();
                BreadCrumb();
            }
            #endregion
        }


        /// <summary>
        /// Fires on click of submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            #region Code
            if (Page.IsValid)
            {
                CollateralCreator.Data.Document orderDocument = new CollateralCreator.Data.Document();
                if (ViewState["documentid"] != null)
                    orderDocument.DocumentID = Convert.ToInt32(ViewState["documentid"]);
                if (ViewState["documentname"] != null)
                    orderDocument.Name = ViewState["documentname"].ToString();
                orderDocument.Quantity = Convert.ToInt16(ddQuantity.SelectedItem.Text);
                orderDocument.Attention = txtBoxInput9.Text;
                orderDocument.CompanyName = txtBoxInput10.Text;
                orderDocument.CompanyID = this.ChannelPartner.CompanyId;
                orderDocument.AddressLine1 = txtBoxInput11.Text;
                orderDocument.AddressLine2 = txtBoxInput12.Text;
                orderDocument.City = txtBoxInput13.Text;
                orderDocument.State = txtBoxInput13a.Text;
                orderDocument.PostCode = txtBoxInput14.Text;
                orderDocument.FirstName = txtBoxInput15.Text;
                orderDocument.LastName = txtBoxInput16.Text;
                orderDocument.Phone = txtBoxInput17.Text;
                orderDocument.Email = txtBoxInput18.Text;
                orderDocument.Country = this.ChannelPartner.CompanyCountry;
                string ChannelPartnerPhone = string.Empty;
                if (ChannelPartner.Info.Count > 0)
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("phone"))
                        {
                            ChannelPartnerPhone = inf.Value;
                        }
                    }
                }
                orderDocument.ChannelPartnerPhone = ChannelPartnerPhone;
                Session["OrderInformation"] = orderDocument;

                if (ViewState["documentid"] != null && ViewState["defaultlogo"] != null)
                    Response.Redirect("OrderSummary.aspx?documentid=" + ViewState["documentid"] + "&logo=0", true);
                else if (ViewState["documentid"] != null && ViewState["defaultlogo"] == null)
                    Response.Redirect("OrderSummary.aspx?documentid=" + ViewState["documentid"] + "&logo=1", true);
            }
            #endregion
        }

        /// <summary>
        /// Fires on click of cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            #region Code
            if (Request.QueryString["documentid"] != null)
                Response.Redirect("Edit.aspx?documentid=" + ViewState["documentid"], true);

            else if (Request.QueryString["templateid"] != null && Request.QueryString["partnerbrand"] != null)
                Response.Redirect("Home.aspx", true);
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
            this.lblbreadcrumbdesc.Text = GetLocalResourceObject("BreadCrumbText").ToString();
            int odcount = 1;
            string editflag = string.Empty; 
            SiteMapNode node = SiteMap.CurrentNode;
            do
            {
                Literal link = new Literal();
                if (odcount == 1 || odcount == 2)
                {
                    if (Request.QueryString["templateid"] != null && Request.QueryString["partnerbrand"] != null)
                    {
                        if (node.Url.Contains("OrderDocument.aspx"))
                            link.Text = "<span class=\"currentbreadcrumlink\">" + node.Title + "</span>";
                        else if (node.Url.Contains("Edit.aspx"))
                            editflag = "NoEditPage";
                    }
                    else if (Request.QueryString["documentid"] != null)
                    {
                        if (node.Url.Contains("OrderDocument.aspx"))
                            link.Text = "<span class=\"currentbreadcrumlink\">" + node.Title + "</span>";
                        else if (node.Url.Contains("Edit.aspx"))
                            link.Text = "<a href=\"" + node.Url + "?documentid=" + documentId + "\" class=\"breadcrumlink\">" + node.Title + "</a>";
                            //link.Text = "<span class=\"breadcrumlink\">" + node.Title + "</span>";
                    }
                }
                else if (odcount == 3)
                    link.Text = "<a href=\"" + node.Url + "\" class=\"breadcrumlink\">" + node.Title + "</a>";
                    //link.Text = "<span class=\"breadcrumlink\">" + node.Title + "</span>";
                
                SiteHierarchy.Controls.AddAt(0, link);

                if (editflag == string.Empty)
                {
                    Label label = new Label();
                    label.Text = " > ";
                    SiteHierarchy.Controls.AddAt(0, label);
                }

                if (node.PreviousSibling != null && Request.QueryString["templateid"] == null && Request.QueryString["partnerbrand"] == null && GlobalVariable == string.Empty)
                {
                    Literal plink = new Literal();
                    //plink.Text = "<span class=\"breadcrumlink\">" + node.PreviousSibling.Title + "</span>";
                    plink.Text = "<a href=\"" + node.PreviousSibling.Url + "\" class=\"breadcrumlink\">" + node.PreviousSibling.Title + "</a>";
                    SiteHierarchy.Controls.AddAt(0, plink);

                    Label nlabel = new Label();
                    nlabel.Text = " > ";
                    SiteHierarchy.Controls.AddAt(0, nlabel);
                    GlobalVariable = "NoRecentPage";
                }
                
                node = node.ParentNode;
                odcount++;
            }
            while (node != null);
            #endregion
        }

        /// <summary>
        /// Parse query string and builds logic to create the document
        /// </summary>
        private void ProcessVars()
        {
            #region Code

            string fileNameNew = string.Empty;
            //int documentId = 0;

            if (Request.QueryString["templateid"] != null && Request.QueryString["partnerbrand"] != null)
            {
                Int16.TryParse(Request.QueryString["templateid"], out templateId);
                Boolean.TryParse(Request.QueryString["partnerbrand"], out partnerBrand);

                //building dropdown order numbers logic
                BuildOrderDropDown(templateId);

                //get the Document object for the template and brand provided
                if (Session["CustomCultureCode"] != null)
                {
                    if (Session["CustomCultureCode"].ToString().ToLower() == "en-ca")
                    {
                        languageCode = "fr-ca";
                    }
                    else
                    {
                        languageCode = Session["CustomCultureCode"].ToString().ToLower();
                    }

                    objDocument = DocumentManager.CreateDocument(templateId, partnerBrand, languageCode, this.ChannelPartner.ChannelPartnerId, this.ChannelPartner.LoginId, this.ChannelPartner.Email);
                }

                if (objDocument == null)
                {
                    Response.Redirect("/shared/ServerError.aspx", true);
                }

                Address a = null;
                if (this.ChannelPartner.Addresses.Count > 0) a = this.ChannelPartner.Addresses[0];

                CollateralCreatorRepository.DocumentUpdateAddressDetails(objDocument.DocumentID, 0, false, string.Empty, this.ChannelPartner.LocationName, string.Empty, a.AddressLine1, a.AddressLine2, a.Town, a.State, a.PostCode, this.ChannelPartner.FirstName, this.ChannelPartner.LastName, string.Empty, this.ChannelPartner.Email, string.Empty, string.Empty);

                PdfManager pdf = new PdfManager();
                //to create doc in the project folder and returns back the name
                pdf.GenerateDocument(ref fileNameNew, objDocument);

                CreateDocument cd = new CreateDocument();
                cd.Document("order", objDocument, ref pdf, ref fileNameNew, ref templateName, ref errorMessage, ref defaultLogoMsg, null);
                
                if(errorMessage != string.Empty)
                    ShowAlertMessage(errorMessage);

                if (defaultLogoMsg == "DefaultLogo")
                    ViewState["defaultlogo"] = defaultLogoMsg;

                //once the document is created, now register a log history in the table
                CollateralCreatorRepository.CreateDocumentHistory(objDocument.DocumentID, "New");

                //pass the document id,name & brand to the Preview Document usercontrol property
                PreviewDocument.DocumentId = objDocument.DocumentID;
                PreviewDocument.TemplateId = objDocument.TemplateID;
                PreviewDocument.DocumentName = objDocument.Name;
                PreviewDocument.PartnerBrand = partnerBrand;

                documentId = objDocument.DocumentID;
                documentName = objDocument.Name;

                Response.Redirect("OrderDocument.aspx?documentid=" + documentId, true);
            }
            else if (Request.QueryString["documentid"] != null)
            {
                int.TryParse(Request.QueryString["documentid"], out documentId);

                //get the Document object for the template and brand provided
                objDocument = DocumentManager.GetDocument(documentId);

                //building dropdown order numbers logic
                BuildOrderDropDown(objDocument.TemplateID);

                if (objDocument == null)
                    Response.Redirect("/shared/ServerError.aspx", true);
                
                //pass the document id,name & brand to the Preview Document usercontrol property
                PreviewDocument.DocumentId = objDocument.DocumentID;
                PreviewDocument.TemplateId = objDocument.TemplateID;
                PreviewDocument.DocumentName = objDocument.Name;
                PreviewDocument.PartnerBrand = objDocument.PartnerBranded;

                documentName = objDocument.Name;
            }

            ViewState["documentid"] = documentId;
            ViewState["documentname"] = documentName;

            #endregion
        }

        /// <summary>
        ///  Build dropdown for quantity based on products
        /// </summary>
        /// <param name="templateid"></param>
        private void BuildOrderDropDown(int templateId)
        {
            #region Code
            int[] BrochureTemplates = {1,4,7,10,13,16,19,22,25,28,31,34}; //??hard coded ~MPE

            if(BrochureTemplates.Contains(templateId))
            {
                ddQuantity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select"));
                ddQuantity.Items.Insert(1, new System.Web.UI.WebControls.ListItem("100"));
                ddQuantity.Items.Insert(2, new System.Web.UI.WebControls.ListItem("250"));
                ddQuantity.Items.Insert(3, new System.Web.UI.WebControls.ListItem("500"));
            }
            else
            {
                ddQuantity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select"));
                ddQuantity.Items.Insert(1, new System.Web.UI.WebControls.ListItem("100"));
                ddQuantity.Items.Insert(2, new System.Web.UI.WebControls.ListItem("250"));
                ddQuantity.Items.Insert(3, new System.Web.UI.WebControls.ListItem("500"));
                ddQuantity.Items.Insert(4, new System.Web.UI.WebControls.ListItem("750"));
                ddQuantity.Items.Insert(5, new System.Web.UI.WebControls.ListItem("1000"));
                ddQuantity.Items.Insert(6, new System.Web.UI.WebControls.ListItem("1500"));
                ddQuantity.Items.Insert(7, new System.Web.UI.WebControls.ListItem("2000"));
            }
            #endregion
        }
        
        /// <summary>
        /// Loads UI elements
        /// </summary>
        private void LoadDataElements()
        {
            #region Code

            if (objDocument != null)
            {
                lblPrintJobText.Text = CollateralCreatorRepository.GetDocumentTemplateName(0, templateId, partnerBrand);
                if (objDocument.Duplex)
                    txtBoxInput4.Text =  GetLocalResourceObject("GeoLabelDuplexText").ToString();
                else
                    txtBoxInput4.Text =  GetLocalResourceObject("GeoLabelSimplexText").ToString();

                txtBoxInput5.Text = objDocument.PaperOption;
                txtBoxInput6.Text = objDocument.PaperSize;
                txtBoxInput7.Text = objDocument.ColorCorrection;
                txtBoxInput8.Text = objDocument.PrintMode;
                txtBoxInput9.Text = "";
                txtBoxInput10.Text = objDocument.CompanyName;
                txtBoxInput11.Text = objDocument.AddressLine1;
                txtBoxInput12.Text = objDocument.AddressLine2;
                txtBoxInput13.Text = objDocument.City;
                txtBoxInput13a.Text = objDocument.State;
                txtBoxInput14.Text = objDocument.PostCode;
                txtBoxInput15.Text = objDocument.FirstName;
                txtBoxInput16.Text = objDocument.LastName;
                txtBoxInput17.Text = "";
                txtBoxInput18.Text = objDocument.Email;
            }

            #endregion
        }

        /// <summary>
        /// Loads page translations for the corresponding language from UI Culture
        /// </summary>
        private void LoadPageTranslations()
        {
            #region Code
          
            this.lblHeaderText.Text =  GetLocalResourceObject("OrderHeaderLabel").ToString();

            this.lblSubHeaderText1.Text =  GetLocalResourceObject("DeliveryMethodLabel").ToString();
            this.lblSubHeaderText2.Text =  GetLocalResourceObject("PrintedCollateralLabel").ToString();

            this.lblSubHeaderText3.Text =  GetLocalResourceObject("PrintOptionsLabel").ToString();
            this.lblFieldText1.Text =  GetLocalResourceObject("QuantityLabel").ToString();
            this.lblFieldText2.Text = GetLocalResourceObject("PrintJobLabel").ToString().Trim();
            this.lblFieldText3.Text = "";
            this.lblFieldText4.Text = GetLocalResourceObject("GeometryLabel").ToString().Trim();
            this.lblFieldText5.Text =  GetLocalResourceObject("PaperOptionsLabel").ToString();
            this.lblFieldText6.Text =  GetLocalResourceObject("PaperSizeLabel").ToString();
            this.lblFieldText7.Text = GetLocalResourceObject("ColorCorrectionLabel").ToString(); 
            this.lblFieldText8.Text =  GetLocalResourceObject("PrintModeLabel").ToString();

            this.lblSubHeaderText4.Text =  GetLocalResourceObject("DeliveryAddressLabel").ToString(); 
            this.lblFieldText9.Text =  GetLocalResourceObject("AttentionLabel").ToString();
            this.lblFieldText10.Text = GetLocalResourceObject("CompanyNameLabel").ToString().Trim(); 
            this.lblFieldText11.Text =  GetLocalResourceObject("AddressLabel").ToString();
            this.lblFieldText12.Text = "";
            this.lblFieldText13.Text =  GetLocalResourceObject("CityLabel").ToString();
            this.lblFieldText13a.Text =  GetLocalResourceObject("StateLabel").ToString();
            this.lblFieldText14.Text = GetLocalResourceObject("PostalCodeLabel").ToString(); 

            this.lblSubHeaderText5.Text =  GetLocalResourceObject("OrderContactLabel").ToString();
            this.lblFieldText15.Text =  GetLocalResourceObject("FirstNameLabel").ToString(); 
            this.lblFieldText16.Text =  GetLocalResourceObject("LastNameLabel").ToString();
            this.lblFieldText17.Text =  GetLocalResourceObject("PhoneLabel").ToString();
            this.lblFieldText18.Text =  GetLocalResourceObject("EmailLabel").ToString();
 
            this.lblPrintPaperText.Text =  GetLocalResourceObject("PrintPaperText").ToString();
            this.btnCancel.Text =  GetLocalResourceObject("CancelbtnText").ToString();
            this.btnSubmit.Text =  GetLocalResourceObject("SubmitbtnText").ToString();

            this.ValidationSummary.HeaderText =  GetLocalResourceObject("ValidationHeaderText").ToString();
            this.QtyCompareValidator.ErrorMessage =  GetLocalResourceObject("QuantityErrorText").ToString();
            this.RequiredFieldValidatorAttention.ErrorMessage =  GetLocalResourceObject("AttentionErrorText").ToString();
            this.RequiredFieldValidatorCompanyName.ErrorMessage =  GetLocalResourceObject("CompanyErrorText").ToString();
            this.RequiredFieldValidatorAddressLine1.ErrorMessage =  GetLocalResourceObject("AddressErrorText").ToString();
            this.RequiredFieldValidatorCity.ErrorMessage =  GetLocalResourceObject("CityErrorText").ToString();
            this.RequiredFieldValidatorState.ErrorMessage =  GetLocalResourceObject("StateErrorText").ToString();
            this.RequiredFieldValidatorPostCode.ErrorMessage =  GetLocalResourceObject("PostalCodeErrorText").ToString();

            this.RequiredFieldValidatorFirstName.ErrorMessage =  GetLocalResourceObject("FirstNameErrorText").ToString();
            this.RequiredFieldValidatorLastName.ErrorMessage =  GetLocalResourceObject("LastNameErrorText").ToString();
            this.RequiredFieldValidatorPhone.ErrorMessage =  GetLocalResourceObject("PhoneErrorText").ToString();
            this.RequiredFieldValidatorEmail.ErrorMessage =  GetLocalResourceObject("EmailErrorText").ToString();
            this.RegularExpressionValidator.ErrorMessage =  GetLocalResourceObject("InvalidEmailErrorText").ToString();
            #endregion

        }
        
        /// <summary>
        /// Custom Javascript popup
        /// </summary>
        /// <param name="message"></param>
        private void ShowAlertMessage(string message)
        {
            #region code
            // Cleans the message to allow single quotation marks 
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page 
            System.Web.UI.Page page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page 
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Edit), "alert", script);
            }
            #endregion
        }

  
        #endregion
    }
}
