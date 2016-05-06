using CollateralCreator.Business;
using CollateralCreator.Data;
using CollateralCreator.SQLProvider;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Models;
using Xerox.SSOComponents.Models.Enumerations;

namespace CollateralCreator.Web.services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Web.Script.Services;
    using System.Web.Services;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>
    /// Summary description for CollateralHome
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CollateralHome : XeroxWebPage
    {
        private string status = "{ \"status\" : 0 }";
        private string sErrormessage = string.Empty;
        private string sDefaultLogoMsg = string.Empty;
        private string sTemplatename = string.Empty;

        [WebMethod]
        public string GetDefaultLogoImage()
        {
            string sImage = string.Empty;

            Xerox.SSOComponents.Models.Image img = null;
            if (ChannelPartner.Images.Count > 0) img = this.ChannelPartner.Images[0];

            if (img == null){
                sImage = "Image : [\"default\" ], ";
                status = "{ \"status\" : 1 , " + sImage + "}";
            }
            else{
                sImage = "Image : [\"" + this.ChannelPartner.Images[0].LoginId + "\" ], ";
                status = "{ \"status\" : 1 , " + sImage + "}";
            }
            
            return status;
        }

        [WebMethod]
        public string GetEditPageLogoImage(int iDocumentID)
        {
            string sImages = string.Empty;

            CollateralCreator.Data.Document objdocument = DocumentManager.GetDocument(iDocumentID);
           
            sImages = " Image : [";
            if (objdocument != null && objdocument.Pages.Count != 0)
            {
                 //Page docpage in objdocument.Pages
                foreach (CollateralCreator.Data.Page t in objdocument.Pages)
                {
                    if (t.CustomizableAreas.Count > 0)
                    {
                        //Customizable area doc in objdocument.Pages.CustomizableArea
                        foreach (CustomizableArea ca in t.CustomizableAreas)
                        {

                            if(ca.ImageArea != null)
                            {
                                sImages += "[" + ca.ImageArea.ImageAreaID + "], ";
                            }

                        }
                    }
                }
            }
            sImages += "]";

            if (sImages == string.Empty) return status;

            status = "{ \"status\" : 1 , " + sImages + "}";
            return status;
        }

        [WebMethod]
        public string GetChannelPartnerDetails()
        {
            string sProfileDetail = string.Empty;

            string sTelephoneNumber = string.Empty;
            string sCompanyUrl = string.Empty;
            foreach (Xerox.SSOComponents.Models.Info info in ChannelPartner.Info)
            {
                if (info.Name == "phone")
                    sTelephoneNumber = info.Value;
                if (info.Name == "website")
                    sCompanyUrl = info.Value;
            }

            sProfileDetail = "ProfileDetail : [\"" + this.ChannelPartner.FirstName + "\", \"" + this.ChannelPartner.LastName + "\", \"" + this.ChannelPartner.LocationName + "\", \"" + sCompanyUrl + "\", \"" + sTelephoneNumber + "\", \"" + this.ChannelPartner.LoginId + "\" ],";

            if (sProfileDetail == string.Empty) return status;

            status = "{ \"status\" : 1 , " + sProfileDetail + "}";
            return status;
        }

        [WebMethod]
        public string UpdateChannelPartner(string sFirstname, string sLastname, string sCompanyname, string sCompanyurl, string sTelephonenumber,
                                           string sFBaccount, string sTWTaccount, string sLNKaccount)
        {
            ChannelPartner channelpartner = new ChannelPartner()
            {
                ChannelPartnerId = this.ChannelPartner.ChannelPartnerId,
                Credentials = this.ChannelPartner.Credentials,
                LoginId = this.ChannelPartner.LoginId,
                FirstName = sFirstname,
                LastName = sLastname,
                Email = this.ChannelPartner.Email,
                Language = this.ChannelPartner.Language,
                LanguageId = this.ChannelPartner.LanguageId,
                Principal = this.ChannelPartner.Principal,
                LocationId = this.ChannelPartner.Principal,
                LocationName = sCompanyname,
                CompanyId = this.ChannelPartner.CompanyId,
                CompanyBranding = this.ChannelPartner.CompanyBranding,
                CompanyCountry = this.ChannelPartner.CompanyCountry,
                CountryId = this.ChannelPartner.CountryId,
                CreateDate = this.ChannelPartner.CreateDate,
                ModifyDate = DateTime.Now,
                Backpack = this.ChannelPartner.Backpack
            };
            ChannelPartnerService.Update(channelpartner);

            //update the remaining custom info details
            Info custinfo = null;

            if (sTelephonenumber != string.Empty)
            {
                bool b = false;
                foreach (Xerox.SSOComponents.Models.Info info in ChannelPartner.Info)
                {
                    if (info.Name == "phone")
                    {
                        info.LoginId = this.ChannelPartner.LoginId;
                        info.Value = sTelephonenumber;
                        ChannelPartnerService.UpdateInfo(info);
                        b = true;
                    }
                }

                if (!b)
                {
                    custinfo = new Info() { LoginId = this.ChannelPartner.LoginId, Name = "phone", Value = sTelephonenumber };
                    ChannelPartnerService.CreateInfo(custinfo);
                }
            }

            if (sCompanyurl != string.Empty)
            {
                bool b = false;
                foreach (Xerox.SSOComponents.Models.Info info in ChannelPartner.Info)
                {
                    if (info.Name == "website")
                    {
                        info.Value = sCompanyurl;
                        ChannelPartnerService.UpdateInfo(info);
                        b = true;
                    }
                }

                if (!b)
                {
                    custinfo = new Info() { LoginId = this.ChannelPartner.LoginId, Name = "website", Value = sCompanyurl };
                    ChannelPartnerService.CreateInfo(custinfo);
                }
            }

            status = "{ \"status\" : 1 }";
            return status;
        }

        [WebMethod]
        public string UpdateContactDetails(string sAddressLine1, string sAddressLine2, string sAddressLine3, string sTown, string sState, string sCountry, string sPostCode)
        {
            string sAddress = string.Empty;
            JavaScriptSerializer js = new JavaScriptSerializer();
            Address address = null;

            if (sAddressLine1 != string.Empty || sAddressLine2 != string.Empty || sAddressLine3 != string.Empty || sTown != string.Empty ||
                sState != string.Empty || sCountry != string.Empty || sPostCode != string.Empty)
            {
                if (this.ChannelPartner.Addresses.Count > 0)
                {
                    address = new Address()
                                  {
                                      ChannelPartnerAddressID = this.ChannelPartner.Addresses[0].ChannelPartnerAddressID,
                                      LoginId = this.ChannelPartner.LoginId,
                                      AddressLine1 = sAddressLine1,
                                      AddressLine2 = sAddressLine2,
                                      AddressLine3 = sAddressLine3,
                                      Town = sTown,
                                      State = sState,
                                      Country = sCountry,
                                      PostCode = sPostCode
                                  };

                    ChannelPartnerService.UpdateAddress(address);
                }
                if (this.ChannelPartner.Addresses.Count == 0){

                     address = new Address() { LoginId = this.ChannelPartner.LoginId, AddressLine1 = sAddressLine1, AddressLine2  = sAddressLine2, 
                                               AddressLine3 = sAddressLine3, Town = sTown, State = sState, Country = sCountry,
                                               PostCode = sPostCode};
                     ChannelPartnerService.CreateUpdateDeleteAddress(address, DataProviderAction.Create);
                }
            }

            sAddress = js.Serialize(address);
            if (sAddress == string.Empty) return status;

            status = "{ \"status\": 1, \"Address\":" + sAddress + " }";
            return status;
        }

        [WebMethod]
        public string UpdateCustomInfoDetails(string sChannelPartnerInfoID, string sLoginID, string sCompanyName, string sWebPage, string sPhoneNumber)
        {
            string sCustomInfo = string.Empty;
            string[] infoId = sChannelPartnerInfoID.Split(',');

            //update the company name
            ChannelPartner channelpartner = new ChannelPartner()
            {
                ChannelPartnerId = this.ChannelPartner.ChannelPartnerId,
                Credentials = this.ChannelPartner.Credentials,
                LoginId = sLoginID,
                FirstName = this.ChannelPartner.FirstName,
                LastName = this.ChannelPartner.LastName,
                Email = this.ChannelPartner.Email,
                Language = this.ChannelPartner.Language,
                LanguageId = this.ChannelPartner.LanguageId,
                Principal = this.ChannelPartner.Principal,
                LocationId = this.ChannelPartner.LocationId,
                LocationName = sCompanyName,
                CompanyId = this.ChannelPartner.CompanyId,
                CompanyBranding = this.ChannelPartner.CompanyBranding,
                CompanyCountry = this.ChannelPartner.CompanyCountry,
                CountryId = this.ChannelPartner.CountryId,
                CreateDate = this.ChannelPartner.CreateDate,
                ModifyDate = this.ChannelPartner.ModifyDate,
                Backpack = this.ChannelPartner.Backpack
            };

            bool CPUpdate = false;
            if (!String.IsNullOrEmpty(sCompanyName)){
                                               
                CPUpdate = ChannelPartnerService.Update(channelpartner);
            }

            //update the remaining custom info details
            Info custinfo = null;
          
            if (sPhoneNumber != string.Empty)
            {
                bool b = false;
                foreach (Xerox.SSOComponents.Models.Info info in ChannelPartner.Info)
                {
                    if (info.Name == "phone")
                    {
                        info.LoginId = sLoginID;
                        info.Value = sPhoneNumber;
                        ChannelPartnerService.UpdateInfo(info);
                        b = true;
                    }
                }

                if (!b)
                {
                    custinfo = new Info() { LoginId = sLoginID, Name = "phone", Value = sPhoneNumber };
                    ChannelPartnerService.CreateInfo(custinfo);
                }
            }

            if (sWebPage != string.Empty)
            {
                bool b = false;
                foreach (Xerox.SSOComponents.Models.Info info in ChannelPartner.Info)
                {
                    if (info.Name == "website")
                    {
                        info.Value = sWebPage;
                        ChannelPartnerService.UpdateInfo(info);
                        b = true;
                    }
                }

                if (!b)
                {
                    custinfo = new Info() { LoginId = sLoginID, Name = "website", Value = sWebPage };
                    ChannelPartnerService.CreateInfo(custinfo);
                }
            }


            status = "{ \"status\": 1 }";
            return status;
        }

        [WebMethod]
        public string UpdateDocumentContactDetails(int iTemplateID, bool bPartnerBrand, string sAttention, string sCompanyName, string sAddressLine1, string sAddressLine2, 
                                           string sCity, string sState, string sPostCode, string sFirstName, string sLastName, string sPhone, string sEmail)
        {
            CollateralCreatorRepository.DocumentUpdateAddressDetails(0, iTemplateID, bPartnerBrand, sAttention, sCompanyName, string.Empty, sAddressLine1, sAddressLine2, sCity, sState, sPostCode, sFirstName, sLastName, sPhone, sEmail, string.Empty, string.Empty);

            status = "{ \"status\": 1 }";
            return status;
        }

        [WebMethod]
        public string GetMenuTreeTemplateSection(int nodeID, string language)
        {
            string sTemplates = string.Empty;
            JavaScriptSerializer js = new JavaScriptSerializer();

            List<Template> customtemplates = CollateralCreatorManager.GetCustomTemplates_MenuTreeNode(nodeID, language);

            GetTemplateProvider(customtemplates);           
                       
            sTemplates = js.Serialize(customtemplates);
            if (sTemplates == string.Empty) return status;

            status = "{ \"status\" : 1 , \"CustomTemplates\":" + sTemplates + " }";
            return status;
        }

        [WebMethod]
        public string GetTemplateButtons(string sChannelPartnerLoginID)
        {
            string sTemplateButtons = string.Empty;
            JavaScriptSerializer js = new JavaScriptSerializer();

            //Now check the template has been customized today or not
            List<TemplateButton> ltemplates = TemplateProvider.GetTemplateButton(sChannelPartnerLoginID);

            sTemplateButtons = js.Serialize(ltemplates);
            if (sTemplateButtons == string.Empty) return status;

            status = "{ \"status\" : 1 , \"TemplateButtonDetails\":" + sTemplateButtons + " }";
            return status;
        }

        [WebMethod]
        public string UpdateTemplateButton(int iTemplateID, string sChannelPartnerLoginID, bool bCustomize, bool bPartnerBrand)
        {
            string sTemplateNode = string.Empty;

            int NodeID = CollateralCreatorRepository.UpdateTemplateButton(iTemplateID, sChannelPartnerLoginID, bCustomize, bPartnerBrand);

            sTemplateNode = "TemplateNodeID : [\"" + iTemplateID + "\", \"" + NodeID + "\" ],";

            status = "{ \"status\" : 1  ," + sTemplateNode + " }";
            return status;
        }

        public void CheckTemplateButton(Template template)
        {
            //Get channel partner details
            string sCompanyName = string.Empty;
            string sWebUrl = string.Empty;
            string sPhoneNumber = string.Empty;
            Address a = null;

            if (this.ChannelPartner.Addresses.Count > 0) 
                a = this.ChannelPartner.Addresses[0];

            if (this.ChannelPartner.LocationName != null && this.ChannelPartner.LocationName != string.Empty)
                sCompanyName = ChannelPartner.LocationName;

            if (ChannelPartner.Info.Count > 0)
            {
                foreach (Xerox.SSOComponents.Models.Info inf in ChannelPartner.Info)
                {
                    if (inf.Name.ToLower().Contains("phone"))
                        sPhoneNumber = inf.Value;
                    if (inf.Name.ToLower().Contains("website"))
                        sWebUrl = inf.Value;
                }
            }

            Xerox.SSOComponents.Models.Image img = null;

            if (ChannelPartner.Images.Count > 0) 
                img = this.ChannelPartner.Images[0];

            //Now Check with the template provider details whether it has got all the details required
            Template objtemplate = TemplateProvider.GetTemplate(template.TemplateID);

            if (objtemplate != null && objtemplate.Pages.Count != 0)
            {
                bool flag = false;
                foreach (Page t in objtemplate.Pages)
                {
                    if (flag) break;
                    if (t.CustomizableAreas.Count > 0)
                    {
                        //Customizable area doc in objtemplate.Pages.CustomizableArea
                        foreach (CustomizableArea ca in t.CustomizableAreas)
                        {
                            if (ca.TextArea != null)
                            {
                                //if the template area has got the address details then set the flag
                                if (ca.TextArea.Text.Contains("<address_line1>") ||
                                    ca.TextArea.Text.Contains("<address_line2>") ||
                                    ca.TextArea.Text.Contains("<address_line3>") ||
                                    ca.TextArea.Text.Contains("<address_city>") ||
                                    ca.TextArea.Text.Contains("<address_state>") ||
                                    ca.TextArea.Text.Contains("<address_country>") ||
                                    ca.TextArea.Text.Contains("<address_postcode>") ||
                                    ca.TextArea.Text.Contains("<reseller_name>") ||
                                    ca.TextArea.Text.Contains("<phone_number>") ||
                                    ca.TextArea.Text.Contains("<web_site>"))
                                    if (a != null) template.Flag = "Success";
                                    else
                                    {
                                        template.Flag = "Fail";
                                        flag = true;
                                        break;
                                    }

                                //if the template area has got the company name then set the flag
                                if (ca.TextArea.Text.Contains("<company_name>"))
                                    if (sCompanyName != string.Empty) template.Flag = "Success";
                                    else
                                    {
                                        template.Flag = "Fail";
                                        flag = true;
                                        break;
                                    }

                                //if the template area has got the company name then set the flag
                                if (ca.TextArea.Text.Contains("<phone_number>"))
                                    if (sPhoneNumber != string.Empty) template.Flag = "Success";
                                    else
                                    {
                                        template.Flag = "Fail";
                                        flag = true;
                                        break;
                                    }

                                //if the template area has got the company name then set the flag
                                if (ca.TextArea.Text.Contains("<web_site>"))
                                    if (sWebUrl != string.Empty) template.Flag = "Success";
                                    else
                                    {
                                        template.Flag = "Fail";
                                        flag = true;
                                        break;
                                    }
                            }

                            if (ca.ImageArea != null)
                                //if the template area has got the image then set the flag
                                if (ca.ImageArea.Image != null) template.Flag = "Success";
                                else
                                {
                                    template.Flag = "Fail";
                                    flag = true;
                                    break;
                                }
                        }
                        if (flag) break;
                    }
                }
            }

        }
              
        public void GetTemplateProvider(List<Template> customTemplates)
        {            
            foreach (Template template in customTemplates)
            {
                CheckTemplateButton(template);
            }
        }
                
        [WebMethod]
        public string GetLatestDocuments(string sCountry)
        {
            string slatestDocuments = string.Empty;
            JavaScriptSerializer js = new JavaScriptSerializer();

            List<CollateralCreator.Data.Document> latestDocuments = CollateralCreatorManager.GetAllDocuments(this.ChannelPartner.ChannelPartnerId, sCountry);

            slatestDocuments = js.Serialize(latestDocuments);
            if (slatestDocuments == string.Empty) return status;

            status = "{ \"status\" : 1 , \"LatestDocuments\":" + slatestDocuments + " }";
            return status;
        }

        [WebMethod]
        public string GetPrintHouseActivityDocuments(string sCountry)
        {
            string sprinthouseDocuments = string.Empty;
            JavaScriptSerializer js = new JavaScriptSerializer();

            List<CollateralCreator.Data.Document> printDocuments = CollateralCreatorManager.GetAllPrintHouseHistoryDocuments(this.ChannelPartner.ChannelPartnerId, sCountry);

            sprinthouseDocuments = js.Serialize(printDocuments);
            if (sprinthouseDocuments == string.Empty) return status;

            status = "{ \"status\" : 1 , \"PrintHouseDocuments\":" + sprinthouseDocuments + " }";
            return status;
        }

        [WebMethod]
        public string GenerateDocument(int iTemplateID, bool bPartnerBrand, string sLanguage)
        {
            string sDocumentDetail = string.Empty;

            string sDocument = string.Empty;
            string sFileNameNew = string.Empty;
            
            //get the Document object for the template and brand provided
            CollateralCreator.Data.Document objdocument = DocumentManager.CreateDocument(iTemplateID, bPartnerBrand, sLanguage, this.ChannelPartner.ChannelPartnerId, this.ChannelPartner.LoginId, this.ChannelPartner.Email);

            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0)
            {
                a = this.ChannelPartner.Addresses[0];

                CollateralCreatorRepository.DocumentUpdateAddressDetails(objdocument.DocumentID, 0, false, string.Empty, this.ChannelPartner.LocationName, string.Empty, a.AddressLine1, a.AddressLine2, a.Town, a.State, a.PostCode, this.ChannelPartner.FirstName, this.ChannelPartner.LastName, string.Empty, this.ChannelPartner.Email, string.Empty, string.Empty);
            }

            PdfManager pdf = new PdfManager();

            //to create doc in the project folder and returns back the name
            pdf.GenerateDocument(ref sFileNameNew, objdocument);

            CreateDocument cd = new CreateDocument();
            cd.Document("preview", objdocument, ref pdf, ref sFileNameNew, ref sTemplatename, ref sErrormessage, ref sDefaultLogoMsg, null);
            
            sDocumentDetail = "DocumentDetail : [\"" + sErrormessage + "\", \"" + objdocument.DocumentID + "\" ],";

            status = "{ \"status\" : 1 , " + sDocumentDetail + " }";
            return status;
        }

        [WebMethod]
        public string UpdateDocumentImage(string sImageAreaID)
        {
            sImageAreaID = sImageAreaID.TrimEnd(',');

            //load default image to customizable image areas
            Xerox.SSOComponents.Models.Image img = null;
            if (ChannelPartner.Images.Count > 0) img = this.ChannelPartner.Images[0];

            CollateralCreatorRepository.DocumentUploadImage(sImageAreaID, img.image);

            status = "{ \"status\" : 1 }";
            return status;
        }

        [WebMethod]
        public string UpdateDocument(int iDocumentId, string documentType)
        {
            string sDocumentDetail = string.Empty;

            string sFileNameNew = string.Empty;

            //get the Document object
            CollateralCreator.Data.Document objdocument = DocumentManager.GetDocument(iDocumentId);

            PdfManager pdf = new PdfManager();
            pdf.GenerateDocument(ref sFileNameNew, objdocument);

            CreateDocument cd = new CreateDocument();
            if (documentType == "smartcentre")
            {
                cd.Document("smartcentre", objdocument, ref pdf, ref sFileNameNew, ref sTemplatename, ref sErrormessage, ref sDefaultLogoMsg, null);
            }
            else
            {
                cd.Document("edit", objdocument, ref pdf, ref sFileNameNew, ref sTemplatename, ref sErrormessage, ref sDefaultLogoMsg, null);
            }

            CollateralCreatorRepository.ChangeDocumentStatusByID(objdocument.DocumentID, "Edit", 0, "NULL");

            sDocumentDetail = "DocumentDetail : [\"" + sErrormessage + "\", \"" + objdocument.DocumentID + "\" ],";

            status = "{ \"status\" : 1 , " + sDocumentDetail + " }";
            return status;
        }

        [WebMethod]
        public string UpdateDocumentTextArea(int iAreaId, int iTextAreaId, string iText)
        {
            CollateralCreatorRepository.DocumentUpdateTextArea(iAreaId, iTextAreaId, iText);
         
            status = "{ \"status\" : 1 }";
            return status;
        }
        
        [WebMethod]
        public string DeleteDocumentByID(int iDocumentID)
        {
            CollateralCreatorRepository.DeleteDocumentByID(iDocumentID);

            File.Delete(HttpContext.Current.Server.MapPath("~/temp/document" + iDocumentID + ".pdf"));

            status = "{ \"status\" : 1 }";
            return status;
        }

        [WebMethod]
        public string Document_DownloadLog(int iDocumentId)
        {
            CollateralCreatorRepository.Log_DocumentDownload(iDocumentId);

            status = "{ \"status\" : 1 }";
            return status;
        }

        [WebMethod]
        public string DocumentGetTemplateName(int iDocumentID)
        {
            string sTemplateDetail = string.Empty;

            string sTemplateName = CollateralCreatorRepository.GetDocumentTemplateName(iDocumentID, 0, false);

            sTemplateDetail = "TemplateName : [\"" + sTemplateName + "\"]";

            status = "{ \"status\" : 1 , " + sTemplateDetail + "}";
            return status;
        }

        [WebMethod]
        public string Document_UpdateName(int iDocumentId, string sDocumentName)
        {
            CollateralCreatorRepository.UpdateDocumentName(iDocumentId, sDocumentName);

            status = "{ \"status\" : 1 }";
            return status;
        }

        [WebMethod]
        public string GetDocumentSummary(int iDocumentID)
        {
            string sDocumentSummary = string.Empty;
            JavaScriptSerializer js = new JavaScriptSerializer();

            CollateralCreator.Data.Document summaryDocument = CollateralCreatorRepository.GetDocumentByID(iDocumentID);

            sDocumentSummary = js.Serialize(summaryDocument);
            if (sDocumentSummary == string.Empty) return status;

            status = "{ \"status\" : 1 , \"DocumentSummary\":" + sDocumentSummary + " }";
            return status;
        }
    }
}
