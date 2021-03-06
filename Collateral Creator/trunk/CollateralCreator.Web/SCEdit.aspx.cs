﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.Configuration;
using CollateralCreator.Data;
using CollateralCreator.Business;
using CollateralCreator.SQLProvider;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.Repositories;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CollateralCreator.Web
{
    /// <summary>
    /// Edit page that has been specifically built for the reduced Smart Centre 2 functions 
    /// </summary>
    public partial class SCEdit : XeroxWebPage
    {
        #region Public Member Variables

        public string ImageAreaID1 = "";
        public string ImageAreaID2 = "";
        public string ImageAreaID3 = "";
        public string ImageAreaID4 = "";
        public string ImageAreaID5 = "";
        public string ImageAreaID6 = "";
        private string languageCode { get; set; }

        #endregion

        #region Member Variables


        private int documentId = 0;
        private Int16 templateId = 0;
        private Boolean partnerBrand = false;
        private string action = string.Empty;
        private string fileNameNew = string.Empty;
        private string templateName = string.Empty;
        private string lines = string.Empty;
        private int totalTextarea = 1;
        private int altImage = 1;
        private string errorMessage = string.Empty;
        private string flagAttr = string.Empty;
        private string defaultLogoMsg = string.Empty;

        private Int16 collateralId { get; set; }
        private Boolean brandFlag { get; set; }
        private string languageId { get; set; }
        private string channelPartnerId { get; set; }
        private string menuHierarchy { get; set; }
        private string appMode { get; set; }
        private string companyId { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// Get the document ID
        /// </summary>
        public string DocumentId
        {
            get { return documentId.ToString(); }
        }

        /// <summary>
        /// Get the template ID
        /// </summary>
        public string TemplateId
        {
            get { return templateId.ToString(); }
        }

        /// <summary>
        /// Get the template name
        /// </summary>
        public string TemplateName
        {
            get { return templateName; }
        }


      


        #endregion


        #region Events

        /// <summary>
        /// Fires on page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region code

            //bool IsPageRefresh = false;

            if (!IsPostBack)
            {
                LoadPageTranslations();

               
                    ProcessSsQueryStringData();


                    orderbuttons.Style.Add("margin-left", "200px");
                    orderbuttons.Style.Add("margin-top", "-25px");

                    this.litCustomizeMessage.Text = GetLocalResourceObject("SmartCustomizeMsgText").ToString();

                    //display custom no detail msg when no address details, phone or email in the channel partner
                    if (this.ChannelPartner.Addresses.Count == 0 || this.ChannelPartner.Info.Count == 0)
                    {
                        this.litsmartcentreMessage.Text = GetLocalResourceObject("SmartNoDetailMsgText").ToString();
                    }

                   
               

                //BuildJavascript();

              
            }

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a local resource string from the given key
        /// </summary>
        /// <param name="key">Key name in resx file</param>
        /// <returns>Resource text</returns>
        protected string GetResourcesString(string key)
        {
            return (string)GetLocalResourceObject(key);
        }

        /// <summary>
        /// Process the query string paramters from the Smart Centre Ektron page request
        /// </summary>
        private void ProcessSsQueryStringData()
        {
            #region code

            ChannelPartnerLogin channelPartner = new ChannelPartnerLogin();

            //check mandatory query string parameters
            if (Request["CollateralID"] != null)
            {
                //get mandatory query string parameters
                if (Request["CollateralID"] != null)
                {
                    collateralId = Convert.ToInt16(Request["CollateralID"]);
                }
                if (Request["BrandFlag"] != null)
                {
                    if (Request["BrandFlag"] == "true")
                        brandFlag = true;
                    else
                        brandFlag = false;
                }
                if (Request["LanguageID"] != null)
                {
                    languageId = Request["LanguageID"];
                }
                if (Request["ChannelPartnerID"] != null)
                {
                    channelPartnerId = Request["ChannelPartnerID"];
                }

                if (Request["CompanyID"] != null)
                {
                    companyId = Request["CompanyID"];
                }
              

                //get optional query string parameters
                if (Request["MenuHierarchy"] != null)
                {
                    menuHierarchy = Request["MenuHierarchy"];
                }
                if (Request["CompanyName"] != null)
                {
                    channelPartner.CompanyName = Request["CompanyName"];
                }
                if (Request["CompanyWebsiteURL"] != null)
                {
                    channelPartner.CompanyWebUrl = Request["CompanyWebsiteURL"];
                }
                if (Request["AddressL1"] != null)
                {
                    channelPartner.AddressLine1 = Request["AddressL1"];
                }
                if (Request["AddressL2"] != null)
                {
                    channelPartner.AddressLine2 = Request["AddressL2"];
                }
                if (Request["AddressL3"] != null)
                {
                    channelPartner.AddressLine3 = Request["AddressL3"];
                }
                if (Request["AddressL4"] != null)
                {
                    channelPartner.AddressLine4 = Request["AddressL4"];
                }
                if (Request["State"] != null)
                {
                    channelPartner.State = Request["State"];
                }
                if (Request["ZipCode"] != null)
                {
                    channelPartner.ZipCode = Request["ZipCode"];
                }
                if (Request["SalesContactPhoneNumber"] != null)
                {
                    channelPartner.PhoneNumber = Request["SalesContactPhoneNumber"];
                }
                if (Request["SalesContactEmailAddress"] != null)
                {
                    channelPartner.Email = Request["SalesContactEmailAddress"];
                }
                if (Request["LanguageID"] != null)
                {
                    channelPartner.Language = Request["LanguageID"];
                }

                appMode = "Hybrid";

                BuildDocument(collateralId, brandFlag, languageId, appMode, channelPartner);

            }
            //else if (Request["Type"] != null && Request["Type"] == "SmartCentre" && Request["documentid"] != null)
            //{
            //    int.TryParse(Request.QueryString["documentid"], out documentId);

            //    BuildDocument(documentId, channelPartner);
            //}
          

            #endregion
        }

        

      

        /// <summary>
        /// Get Document object 
        /// </summary>
        /// <param name="documentid"></param>
        private void BuildDocument(int documentId, ChannelPartnerLogin channelPartner)
        {
            #region code

            //get the Document object for the template and brand provided
            CollateralCreator.Data.Document objdocument = DocumentManager.GetDocument(documentId);

            templateName = objdocument.Name;

            if (objdocument == null)
                Response.Redirect(VirtualPathUtility.ToAbsolute("~/shared/ServerError.aspx"), true);

            //Populate the customizable area data into their corresponding fields
            BuildCustomizableAreas(objdocument, channelPartner);

            //pass the document id,name & brand to the Preview Document usercontrol property
            PreviewDocument.DocumentId = objdocument.DocumentID;
            PreviewDocument.TemplateId = objdocument.TemplateID;
            PreviewDocument.PartnerBrand = partnerBrand;

            //pass templateid only for primary collateral creation
            if (Request["Type"] == null)
                templateId = Convert.ToSByte(objdocument.TemplateID);

            partnerBrand = objdocument.PartnerBranded;
            documentId = objdocument.DocumentID;

            #endregion
        }

        /// <summary>
        /// Get Document object 
        /// </summary>
        /// <param name="documentid"></param>
        private void BuildDocument(int documentId)
        {
            #region code

            //get the Document object for the template and brand provided
            CollateralCreator.Data.Document objdocument = DocumentManager.GetDocument(documentId);

            templateName = objdocument.Name;

            if (objdocument == null)
                Response.Redirect(VirtualPathUtility.ToAbsolute("~/shared/ServerError.aspx"), true);

            //Populate the customizable area data into their corresponding fields
            //BuildCustomizableAreas(objdocument);

            //pass the document id,name & brand to the Preview Document usercontrol property
            PreviewDocument.DocumentId = objdocument.DocumentID;
            PreviewDocument.TemplateId = objdocument.TemplateID;
            PreviewDocument.PartnerBrand = partnerBrand;

            //pass templateid only for primary collateral creation
            if (Request["Type"] == null)
            {
                templateId = Convert.ToInt16(objdocument.TemplateID); //Convert.SByte
            }

            partnerBrand = objdocument.PartnerBranded;
            documentId = objdocument.DocumentID;

            #endregion
        }


        /// <summary>
        /// Build document from template and brand - Overloaded method used for Smart Centre 2 integration and display
        /// </summary>
        /// <param name="appmode"></param>
        /// <param name="bpartnerbrand"></param>
        /// <param name="channelPartner"></param>
        /// <param name="itemplateid"></param>
        /// <param name="slanguage"></param>
        private void BuildDocument(int templateId, bool partnerBrand, string language, string appMode, ChannelPartnerLogin channelPartner)
        {
            #region code
            //alter page title
            this.Title = GetLocalResourceObject("SmartCentrePageTitle").ToString();
            this.LblSc2Title.Text = GetLocalResourceObject("SmartCentrePageTitle").ToString();
            CollateralCreator.Data.Document objdocument = null;

           
            objdocument = DocumentManager.CreateEktronDocument(templateId, partnerBrand, language, this.ChannelPartner.ChannelPartnerId, this.ChannelPartner.LoginId, this.ChannelPartner.Email);
           
            if (objdocument == null)
                Response.Redirect(VirtualPathUtility.ToAbsolute("~/shared/ServerError.aspx"), true);

            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0)
            {
                a = this.ChannelPartner.Addresses[0];
                CollateralCreatorRepository.DocumentUpdateAddressDetails(objdocument.DocumentID, 0, false, string.Empty, this.ChannelPartner.LocationName, string.Empty,
                                                                            a.AddressLine1, a.AddressLine2, a.Town, a.State,
                                                                            a.PostCode, this.ChannelPartner.FirstName,
                                                                            this.ChannelPartner.LastName, string.Empty,
                                                                            this.ChannelPartner.Email, string.Empty, string.Empty);
            }


            //to create doc in the project folder and returns back the name
            PdfManager pdf = new PdfManager();
            pdf.GenerateDocument(ref fileNameNew, objdocument);

            CreateDocument cd = new CreateDocument();
          
            cd.Document("smartcentre", objdocument, ref pdf, ref fileNameNew, ref templateName, ref errorMessage, ref defaultLogoMsg, channelPartner);
           
            //once the document is created, now register a log history in the table
            CollateralCreatorRepository.CreateDocumentHistory(objdocument.DocumentID, "Edit");

            //Populate the customizable area data into their corresponding fields
            BuildCustomizableAreas(objdocument, channelPartner);

            //pass the document id,name & brand to the Preview Document usercontrol property
            PreviewDocument.DocumentId = objdocument.DocumentID;
            PreviewDocument.TemplateId = objdocument.TemplateID;
            PreviewDocument.PartnerBrand = partnerBrand;

            documentId = objdocument.DocumentID;

          
            #endregion
        }

        /// <summary>
        /// Builds Customizable text areas for the document in UI
        /// </summary>
        /// <param name="objdocument"></param>
        /// <param name="channelPartner"></param>
        private void BuildCustomizableAreas(CollateralCreator.Data.Document objDocument, ChannelPartnerLogin channelPartner)
        {
            #region code

            Address a = null; string ImageAreaIDs = string.Empty;
            if (ChannelPartner.Addresses.Count > 0) a = ChannelPartner.Addresses[0];

            int imgCount = 0;

            if (objDocument != null && objDocument.Pages.Count != 0)
            {
                int PageNum = 1;
                string sImagePages = string.Empty;

                //Page docpage in objdocument.Pages
                foreach (CollateralCreator.Data.Page t in objDocument.Pages)
                {
                    if (t.CustomizableAreas.Count > 0)
                    {
                        //Customizable area doc in objdocument.Pages.CustomizableArea
                        foreach (CustomizableArea ca in t.CustomizableAreas)
                        {
                            if (ca.TextArea != null)
                            {
                                //search and replace the text area fields
                                lines = ca.TextArea.Text;
                                Search_Replace(ref lines, a, ref flagAttr, channelPartner);
                                string[] fTextArea = Regex.Split(lines, "\r\n");
                                BuildCustomizableTextArea(objDocument.Name, ca.Name, ca.TextArea.AreaID, ca.TextArea.TextAreaID, PageNum, fTextArea, ca.TextArea.CharsPerLine, ca.TextArea.Text.Length, ref flagAttr, ca.TextArea.Lines, ref totalTextarea);
                            }


                            
                            if (ca.ImageArea != null)
                            {
                                BuildScCustomizableImageArea(ca.Name, ca.ImageArea.ImageAreaID, PageNum, ca.ImageArea.Image, imgCount, ca.ImageArea.PartnerBranded, objDocument.DocumentID);
                                imgCount++;
                            }
                           
                        }
                    }

                    PageNum++;
                }

                //add scroll bar to the editable text area div
                if (totalTextarea > 6)
                {
                    customArea.Style.Add("height", "650px");
                    customArea.Style.Add("width", "525px");
                    customArea.Style.Add("overflow", "scroll");
                    customArea.Style.Add("overflow-x", "hidden");
                }

              
            }

            #endregion
        }

        /// <summary>
        /// Builds the custom image area for a smart centre piece of collateral
        /// </summary>
        /// <param name="areaName">The area name title heading</param>
        /// <param name="pageNumber">Page number in which the image occurs</param>
        /// <param name="imageData">Byte array of image data</param>
        /// <param name="canEdit">Flag to indicate if the user can change the logo</param>
        /// <param name="documentId">Main document ID</param>
        /// <param name="imageAreaId">DB ID of the image</param>
        /// <param name="imageCount">which order the image has been assigned</param>
        private void BuildScCustomizableImageArea(string areaName, int imageAreaId, int pageNumber, byte[] imageData, int imageCount, bool canEdit, int documentId)
        {
            #region Code
            try
            {

                //alter Cirque du Soleil competition text second line - doesn't make sense as text taken from marking store
                if (areaName.Contains("If you do not want to to promote the competition please select the second box."))
                {
                    areaName = areaName.Replace("If you do not want to to promote the competition please select the second box.", "If you do not want to to promote the competition please click the check box below.");
                }
                if (imageCount == 0)
                {
                    PnlImage1.Visible = true;
                    LabelImageArea1.Text = areaName;
                    CustomImage1.Src = "data:image/jpg;base64," + Convert.ToBase64String(imageData);
                    SubLabelImage1.Text = GetLocalResourceObject("CustomizeImgText1").ToString();

                    ResizeImage(imageData, CustomImage1, SubLabelImage1, areaName);
                    InputChangeLogo1.Value = GetLocalResourceObject("ChangebtnText1").ToString();


                    InputChangeLogo1.Visible = canEdit;
                    if (canEdit)
                    {
                        SpanImageRemoval1.Visible = false;
                    }
                    InputChangeLogo1.Attributes.Add("onclick", "ChangeLogoDialog(" + imageAreaId.ToString() + "," + documentId.ToString() + ", 'CustomImage1' )");
                    ImageAreaID1 = imageAreaId.ToString();
                    ChkImageRemoval1.Attributes.Add("onclick", "UploadBlankImage(" + imageAreaId.ToString() + ")");
                }
                else if (imageCount == 1 && !areaName.Contains("Custom Graph"))
                {
                    PnlImage2.Visible = true;
                    LabelImageArea2.Text = areaName;
                    CustomImage2.Src = "data:image/jpg;base64," + Convert.ToBase64String(imageData);

                    //SubLabelImage2.Text = GetLocalResourceObject("CustomizeImgText1").ToString();

                    ResizeImage(imageData, CustomImage2, SubLabelImage2, areaName);

                    InputChangeLogo2.Value = GetLocalResourceObject("ChangebtnText1").ToString();
                    InputChangeLogo2.Visible = canEdit;
                    if (canEdit)
                    {
                        SpanImageRemoval2.Visible = false;
                    }
                    InputChangeLogo2.Attributes.Add("onclick", "ChangeLogoDialog(" + imageAreaId.ToString() + "," + documentId.ToString() + ", 'CustomImage2')");
                    ImageAreaID2 = imageAreaId.ToString();
                    ChkImageRemoval2.Attributes.Add("onclick", "UploadBlankImage(" + imageAreaId.ToString() + ")");

                }
                else if (imageCount == 2)
                {
                    PnlImage3.Visible = true;
                    LabelImageArea3.Text = areaName;
                    CustomImage3.Src = "data:image/jpg;base64," + Convert.ToBase64String(imageData);
                    //SubLabelImage3.Text = GetLocalResourceObject("CustomizeImgText1").ToString();

                    ResizeImage(imageData, CustomImage3, SubLabelImage3, areaName);

                    InputChangeLogo3.Value = GetLocalResourceObject("ChangebtnText1").ToString();
                    InputChangeLogo3.Visible = canEdit;
                    if (canEdit)
                    {
                        SpanImageRemoval3.Visible = false;
                    }
                    InputChangeLogo3.Attributes.Add("onclick", "ChangeLogoDialog(" + imageAreaId.ToString() + "," + documentId.ToString() + ", 'CustomImage3')");
                    ImageAreaID3 = imageAreaId.ToString();
                    ChkImageRemoval3.Attributes.Add("onclick", "UploadBlankImage(" + imageAreaId.ToString() + ")");

                }
                else if (imageCount == 3)
                {
                    PnlImage4.Visible = true;
                    LabelImageArea4.Text = areaName;
                    CustomImage4.Src = "data:image/jpg;base64," + Convert.ToBase64String(imageData);
                    //SubLabelImage4.Text = GetLocalResourceObject("CustomizeImgText1").ToString();

                    ResizeImage(imageData, CustomImage4, SubLabelImage4, areaName);

                    InputChangeLogo4.Value = GetLocalResourceObject("ChangebtnText1").ToString();
                    InputChangeLogo4.Visible = canEdit;
                    if (canEdit)
                    {
                        SpanImageRemoval4.Visible = false;
                    }
                    InputChangeLogo4.Attributes.Add("onclick", "ChangeLogoDialog(" + imageAreaId.ToString() + "," + documentId.ToString() + ", 'CustomImage4')");
                    ImageAreaID4 = imageAreaId.ToString();
                    ChkImageRemoval4.Attributes.Add("onclick", "UploadBlankImage(" + imageAreaId.ToString() + ")");

                }
                else if (imageCount == 4)
                {
                    PnlImage5.Visible = true;
                    LabelImageArea5.Text = areaName;
                    CustomImage5.Src = "data:image/jpg;base64," + Convert.ToBase64String(imageData);
                    //SubLabelImage4.Text = GetLocalResourceObject("CustomizeImgText1").ToString();

                    ResizeImage(imageData, CustomImage5, SubLabelImage5, areaName);

                    InputChangeLogo5.Value = GetLocalResourceObject("ChangebtnText1").ToString();
                    InputChangeLogo5.Visible = canEdit;
                    if (canEdit)
                    {
                        SpanImageRemoval5.Visible = false;
                    }
                    InputChangeLogo5.Attributes.Add("onclick", "ChangeLogoDialog(" + imageAreaId.ToString() + "," + documentId.ToString() + ", 'CustomImage5')");
                    ImageAreaID5 = imageAreaId.ToString();
                    ChkImageRemoval5.Attributes.Add("onclick", "UploadBlankImage(" + imageAreaId.ToString() + ")");

                }
                else if (imageCount == 5)
                {
                    PnlImage6.Visible = true;
                    LabelImageArea6.Text = areaName;
                    CustomImage6.Src = "data:image/jpg;base64," + Convert.ToBase64String(imageData);
                    //SubLabelImage4.Text = GetLocalResourceObject("CustomizeImgText1").ToString();

                    ResizeImage(imageData, CustomImage6, SubLabelImage6, areaName);

                    InputChangeLogo6.Value = GetLocalResourceObject("ChangebtnText1").ToString();
                    InputChangeLogo6.Visible = canEdit;
                    if (canEdit)
                    {
                        SpanImageRemoval6.Visible = false;
                    }
                    InputChangeLogo6.Attributes.Add("onclick", "ChangeLogoDialog(" + imageAreaId.ToString() + "," + documentId.ToString() + ", 'CustomImage6')");
                    ImageAreaID6 = imageAreaId.ToString();
                    ChkImageRemoval6.Attributes.Add("onclick", "UploadBlankImage(" + imageAreaId.ToString() + ")");

                }
            }
            catch { }
            #endregion
        }

        /// <summary>
        /// Reduces the image size to 30 %
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="CustomImage"></param>
        private void ResizeImage(byte[] imageData, HtmlImage customImage, Label subLabelImage, string areaName)
        {
            #region code

            System.Drawing.Image imageToRender = ByteToImage(imageData);

            if (imageToRender.Height > 600 && imageToRender.Width > 600)
            {
                customImage.Style.Add("height", "70%");
                customImage.Style.Add("width", "70%");

                if (areaName.Contains("Custom Image"))
                {
                    string customMsgText = GetLocalResourceObject("CustomizeImgText2").ToString();
                    customMsgText = customMsgText.Replace("<width>", imageToRender.Width.ToString());
                    customMsgText = customMsgText.Replace("<height>", imageToRender.Height.ToString());

                    subLabelImage.Text = customMsgText;
                }
            }

            #endregion
        }

        /// <summary>
        /// Converts binary to Image 
        /// </summary>
        /// <param name="bytearr"></param>
        /// <returns></returns>
        private System.Drawing.Image ByteToImage(byte[] bytearr)
        {
            #region Code

            System.Drawing.Image newImage = null;
            MemoryStream ms = new MemoryStream(bytearr, 0, bytearr.Length);
            ms.Write(bytearr, 0, bytearr.Length);
            newImage = System.Drawing.Image.FromStream(ms, true);
            return newImage;

            #endregion
        }



 

        /// <summary>
        /// Builds Customizable text area messages for the document in UI - Includes Smart centre 2 information
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <param name="areaName">customizable area name</param>
        /// <param name="areaId">Area ID</param>
        /// <param name="textAreaId">Text area ID</param>
        /// <param name="pageNumber">Page ID</param>
        /// <param name="textArea">Default text area</param>
        /// <param name="maxlines">Max number of lines for text box</param>
        /// <param name="maxcharsperline">Max number of characters per line</param>
        /// <param name="maxChars">Max number of characters</param>
        /// <param name="flagattr">?</param>
        private void BuildCustomizableTextArea(string templateName, string areaName, int areaId, int textAreaId, int pageNumber, string[] textArea, int maxCharsPerLine, int currentChars, ref string flagattr, int maxLines, ref int totalTextarea)
        {
            #region code
            string[] wordCountTemplates = ConfigurationManager.AppSettings["TemplateWordCount"].ToString().Split(';');
            bool wordCount = false;
            int maxChars = maxCharsPerLine * maxLines;
            //get translations
            string custommsg1 = GetLocalResourceObject("CustomizeMsgText1").ToString();
            string custommsg3 = GetLocalResourceObject("CustomizeMsgText3").ToString();
            string custommsg4 = GetLocalResourceObject("CustomizeMsgText4").ToString();
            string custommsg5 = GetLocalResourceObject("CustomizeMsgText5").ToString();
            string custommsg6 = "/" + maxChars.ToString() + " " + GetLocalResourceObject("CustomizeMsgText6");

            //investigate whether any of the template names in the above parameter occur in the template name
            for (int i = 0; i < wordCountTemplates.Length; i++)
            {
                if (templateName.Contains(wordCountTemplates[i]))
                {
                    wordCount = true;
                }
            }


            //Build the address to display in the customizable Area
            StringBuilder custTextArea = new StringBuilder(1000);

            custTextArea.Append("<div class=\"divtextareasmartcentre\">");

            custTextArea.Append("<label id=\"lblCoverAddress\" style='display:block;width:370px;font-size:1.1em;font-weight:bold'>" + areaName + "</label>");

            //~MPE 11/09/2013 word count for sc2 collateral cannot be relied on, switched off for time being
            int iUsedChars = 0;
            if (!wordCount)
            {
                custTextArea.Append("<textarea id=\"textareasc" + totalTextarea + "\"  areaid=\"" + areaId + "\" rows=\"" + maxLines + "\" textareaid=\"" + textAreaId + "\"   maxlength=\"" + maxChars + "\"  page=\"" + pageNumber + "\" class=\"txtcustomizeareastyle\">");
            }
            else
            {
                custTextArea.Append("<textarea id=\"textareasc" + totalTextarea + "\"  onkeyup=\"charcounter('textareasc" + totalTextarea + "', '" + currentChars.ToString() + "', 'span" + textAreaId + "')\" areaid=\"" + areaId + "\" rows=\"" + maxLines + "\" textareaid=\"" + textAreaId + "\"   maxlength=\"" + maxChars + "\"  page=\"" + pageNumber + "\" class=\"txtcustomizeareastyle\">");
            }
            foreach (string strarea in textArea)
            {
                iUsedChars += strarea.Length;
                custTextArea.Append(strarea);
                custTextArea.AppendFormat("\r\n");
            }


            custTextArea.Append("</textarea>");
            custTextArea.Append("<br/>");
            //~MPE 11/09/2013 word count issue
            if (wordCount)
            {
                custTextArea.Append("<div class=\"textareanote\"><span id='span" + textAreaId + "'>" + currentChars.ToString() + "</span>" + custommsg6 + "</div>");
            }
            custTextArea.Append("<input id=\"hiddentextarea" + totalTextarea + "\" type='hidden' runat='server'  value='" + areaId + "' />");
            custTextArea.Append("<input id=\"hiddentextareaId" + totalTextarea + "\" type='hidden' runat='server'  value='" + textAreaId + "' />");
            custTextArea.Append("</div>");

            litCustomizableTextArea.Text += custTextArea.ToString();
            totalTextarea = totalTextarea + 1;

            #endregion
        }

        /// <summary>
        /// search for the element tag and replace the corresponding value
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="a"></param>
        /// <param name="flagattr"></param>
        private void Search_Replace(ref string lines, Address a, ref string flagAttr, ChannelPartnerLogin channelPartner)
        {
            #region code

            if (lines.Contains("<address_line1>") || lines.Contains("<address_line2>") || lines.Contains("<address_line3>")
                || lines.Contains("<address_city>") || lines.Contains("<address_state>") || lines.Contains("<address_country>") || lines.Contains("<address_postcode>"))
            {
                if (lines.Contains("<reseller_name>") && this.ChannelPartner.FirstName != string.Empty)
                {
                    lines = lines.Replace("<reseller_name>", this.ChannelPartner.FirstName);
                }

                if (a != null)
                {
                    if (lines.Contains("<address_line1>") && a.AddressLine1 != string.Empty)
                        lines = lines.Replace("<address_line1>", a.AddressLine1);

                    if (lines.Contains("<address_line2>") && a.AddressLine2 != string.Empty)
                        lines = lines.Replace("<address_line2>", a.AddressLine2);

                    if (lines.Contains("<address_line3>") && a.AddressLine3 != string.Empty)
                        lines = lines.Replace("<address_line3>", a.AddressLine3);

                    if (lines.Contains("<address_city>") && a.Town != string.Empty)
                        lines = lines.Replace("<address_city>", a.Town);

                    if (lines.Contains("<address_state>") && a.State != string.Empty)
                        lines = lines.Replace("<address_state>", a.State);

                    if (lines.Contains("<address_country>") && a.Country != string.Empty)
                        lines = lines.Replace("<address_country>", a.Country);

                    if (lines.Contains("<address_postcode>") && a.PostCode != string.Empty)
                        lines = lines.Replace("<address_postcode>", a.PostCode);
                }
                //append smart centre optional parameters to the document if present
                else if (a == null && channelPartner != null)
                {
                    if (lines.Contains("<address_line1>") && channelPartner.AddressLine1 != string.Empty)
                        lines = lines.Replace("<address_line1>", channelPartner.AddressLine1);

                    if (lines.Contains("<address_line2>") && channelPartner.AddressLine2 != string.Empty)
                        lines = lines.Replace("<address_line2>", channelPartner.AddressLine2);

                    if (lines.Contains("<address_line3>") && channelPartner.AddressLine3 != string.Empty)
                        lines = lines.Replace("<address_line3>", channelPartner.AddressLine3);

                    if (lines.Contains("<address_city>") && channelPartner.AddressLine4 != string.Empty)
                        lines = lines.Replace("<address_city>", channelPartner.AddressLine4);

                    if (lines.Contains("<address_state>") && channelPartner.State != string.Empty)
                        lines = lines.Replace("<address_state>", channelPartner.State);

                    if (lines.Contains("<address_postcode>") && channelPartner.ZipCode != string.Empty)
                        lines = lines.Replace("<address_postcode>", channelPartner.ZipCode);
                }


                if (lines.Contains("<phone_number>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("phone"))
                            lines = lines.Replace("<phone_number>", inf.Value);
                    }

                    //append smart centre optional parameters to the document if present
                    if (ChannelPartner.Info.Count == 0 && channelPartner != null)
                    {
                        lines = lines.Replace("<phone_number>", channelPartner.PhoneNumber);
                    }
                }

                if (lines.Contains("<web_site>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("website"))
                            lines = lines.Replace("<web_site>", inf.Value);
                    }

                    //append smart centre optional parameters to the document if present
                    if (ChannelPartner.Info.Count == 0 && channelPartner != null)
                    {
                        lines = lines.Replace("<web_site>", channelPartner.CompanyWebUrl);
                    }
                }

                flagAttr = "addressarea";
            }

            if (lines.Contains("<company_name>") && (!lines.Contains("<web_site>") && !lines.Contains("<phone_number>")))
            {
                if (lines.Contains("<company_name>"))
                {
                    if (this.ChannelPartner.LocationName != string.Empty)
                    {
                        lines = lines.Replace("<company_name>", this.ChannelPartner.LocationName);
                    }

                    //append smart centre optional parameters to the document if present
                    if (this.ChannelPartner.LocationName == string.Empty && channelPartner != null)
                    {
                        lines = lines.Replace("<company_name>", channelPartner.CompanyName);
                    }
                }

                flagAttr = "accreditationarea";
            }

            if ((lines.Contains("<company_name>") && (lines.Contains("<web_site>") || lines.Contains("<phone_number>"))))
            {
                if (lines.Contains("<company_name>"))
                {
                    if (this.ChannelPartner.LocationName != string.Empty)
                    {
                        lines = lines.Replace("<company_name>", this.ChannelPartner.LocationName);
                    }
                    //append smart centre optional parameters to the document if present
                    if (this.ChannelPartner.LocationName == string.Empty && channelPartner != null)
                    {
                        lines = lines.Replace("<company_name>", channelPartner.CompanyName);
                    }
                }

                if (lines.Contains("<web_site>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("website"))
                            lines = lines.Replace("<web_site>", inf.Value);
                    }

                    //append smart centre optional parameters to the document if present
                    if (ChannelPartner.Info.Count == 0 && channelPartner != null)
                    {
                        lines = lines.Replace("<web_site>", channelPartner.CompanyWebUrl);
                    }
                }
                if (lines.Contains("<phone_number>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("phone"))
                            lines = lines.Replace("<phone_number>", inf.Value);
                    }

                    //append smart centre optional parameters to the document if present
                    if (ChannelPartner.Info.Count == 0 && channelPartner != null)
                    {
                        lines = lines.Replace("<phone_number>", channelPartner.PhoneNumber);
                    }
                }
                flagAttr = "customarea";
            }

            if ((lines.Contains("<web_site>") && lines.Contains("<phone_number>")) && (!lines.Contains("<company_name>")))
            {
                if (lines.Contains("<web_site>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("website"))
                            lines = lines.Replace("<web_site>", inf.Value);
                    }

                    //append smart centre optional parameters to the document if present
                    if (ChannelPartner.Info.Count == 0 && channelPartner != null)
                    {
                        lines = lines.Replace("<web_site>", channelPartner.CompanyWebUrl);
                    }

                }
                if (lines.Contains("<phone_number>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("phone"))
                            lines = lines.Replace("<phone_number>", inf.Value);
                    }

                    //append smart centre optional parameters to the document if present
                    if (ChannelPartner.Info.Count == 0 && channelPartner != null)
                    {
                        lines = lines.Replace("<phone_number>", channelPartner.PhoneNumber);
                    }
                }
                flagAttr = "contactarea";
            }

            #endregion
        }

        /// <summary>
        /// Builds Javascript variables and translations for the UI
        /// </summary>
        private void BuildJavascript()
        {
            #region code


            litJavaScript.Text = "<script type=\"text/javascript\" language=\"javascript\">\n";
            litJavaScript.Text += "var sNoDetailsFlag =\"\";\n";
            litJavaScript.Text += "var fileDocumentId=\"" + documentId + "\";\n";
            litJavaScript.Text += "var sTemplateId=\"" + templateId + "\";\n";
            litJavaScript.Text += "var sTemplateBrand=\"" + partnerBrand + "\";\n";
            litJavaScript.Text += "var customizingTemplate=\"" + templateName + "\";\n";
            litJavaScript.Text += "var sContactTitle =\"" + GetLocalResourceObject("CustomizeAreaTxt").ToString() + "\";\n";
            litJavaScript.Text += "var sConfirmMsg =\"" + GetLocalResourceObject("ConfirmBackText").ToString() + "\";\n";
            litJavaScript.Text += "var sbtnUploadText =\"" + GetLocalResourceObject("DialogUploadbtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sCLoginId =\"" + this.ChannelPartner.LoginId + "\";\n";
            litJavaScript.Text += "var sMaxLineErrorMsg =\"" + GetLocalResourceObject("MaxLineErrorMessage").ToString() + "\";\n";
            litJavaScript.Text += "var sMaxCharErrorMsg =\"" + GetLocalResourceObject("MaxCharErrorMessage").ToString() + "\";\n";
            litJavaScript.Text += "var sNoDetailMessage =\"" + GetLocalResourceObject("NoDetailsLabel").ToString() + "\";\n";
            litJavaScript.Text += "var sType=\"SmartCentre\";\n";
          

            //check whether the address details are empty
            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0)
                a = this.ChannelPartner.Addresses[0];

            if (a == null)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";

            //check whether the custom details are empty
            if (ChannelPartner.LocationName == null && ChannelPartner.LocationName == string.Empty)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";

            if (ChannelPartner.Info.Count == 0)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";

            //check whether the logo is default
            Xerox.SSOComponents.Models.Image img = null;
            if (ChannelPartner.Images.Count > 0)
                img = this.ChannelPartner.Images[0];

            if (img == null) litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";


            litJavaScript.Text += "</script>\n";

            #endregion
        }

        /// <summary>
        /// Gets translations for the corresponding language from UI Culture
        /// </summary>
        private void LoadPageTranslations()
        {
            #region code

            this.lblEditHeader.Text = GetLocalResourceObject("EditHeaderLabel").ToString();
            this.lblDocumentName.Text = GetLocalResourceObject("EditDocumentMsg").ToString();
            this.btnCancelLogo.Text = GetLocalResourceObject("DialogCancelbtnText").ToString();
            this.btnUpdatePreview.Text = GetLocalResourceObject("btnUpdatePreviewText").ToString();
            this.btnDownload.Text = GetLocalResourceObject("btnDownloadText").ToString();
            this.litCustomizeMessage.Text = GetLocalResourceObject("CustomizeMsgText").ToString();
            //this.SpanSaving.InnerText = GetLocalResourceObject("PreparingReport").ToString();
            this.StrongLoading.InnerText = GetLocalResourceObject("PreparingReport").ToString();

            #endregion
        }

       

        #endregion

    }
}