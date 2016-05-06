using iTextSharp.text;
using iTextSharp.text.pdf;
using CollateralCreator.Business;
using CollateralCreator.Data;
using CollateralCreator.SQLProvider;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;

/// <summary>
/// Class to generate Document within the application.
/// </summary>
namespace CollateralCreator.Web 
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;
    
    public class CreateDocument : XeroxControl
    {
        #region Member Variables

        private string lines = string.Empty;

        #endregion
            
        #region Methods

        /// <summary>
        /// Creates the document using itextsharp
        /// </summary>
        /// <param name="sActionMode"></param>
        /// <param name="objdocument"></param>
        /// <param name="pdf"></param>
        /// <param name="sFileNameNew"></param>
        /// <param name="sTemplatename"></param>
        /// <param name="sErrormessage"></param>
        /// <param name="sDefaultLogo"></param>
        /// <param name="ChannelPartner"></param>
        public void DocumentCreation(string actionMode, CollateralCreator.Data.Document objDocument, ref PdfManager pdf, ref string fileNameNew, ref string templateName, ref string errorMessage, ref string defaultLogo, ChannelPartnerLogin channelPartner)
        {
            #region code
            
            //instance of local PdfDoc class
            pdf.DocumentOpen(fileNameNew, objDocument.PageSize);

            string sourceFilePath = GetSourceFilePath(objDocument.TemplateID, objDocument.PartnerBranded);

            templateName = objDocument.Name;

            //if the template doesn't exists return 
            if (!File.Exists(sourceFilePath))
            {
                errorMessage = "File Not Found";
                return;
            }

            if (objDocument != null && objDocument.Pages.Count != 0)
            {
                int pageNumber = 1;              
  
                //loop thru pages in objdocument.Pages
                foreach (Page page in objDocument.Pages)
                {
                    //create page document
                    CreatePageDocument(sourceFilePath, page, pageNumber, actionMode, ref pdf, ref errorMessage, ref defaultLogo, channelPartner);
                    
                    pageNumber++;
                }

                pdf.DocumentClose();

                //pdf.SetCompression(GetDocumentFilePath(fileNameNew), fileNameNew);
            }

            #endregion
        }

        /// <summary>
        /// Adds page to the document
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="page"></param>
        /// <param name="pageNumber"></param>
        /// <param name="actionMode"></param>
        /// <param name="pdf"></param>
        /// <param name="errorMessage"></param>
        /// <param name="defaultLogo"></param>
        /// <param name="channelPartner"></param>
        private void CreatePageDocument(string sourceFilePath, Page page, int pageNumber, string actionMode, ref PdfManager pdf, ref string errorMessage, ref string defaultLogo, ChannelPartnerLogin channelPartner)
        {
            #region code

            //create page if the page is available in the document 
            if (pageNumber == 1)
            {
                pdf.AddTemplateFirstPage(sourceFilePath);
            }
            else
            {
                pdf.AddTemplateNextPage(sourceFilePath, pageNumber);
            }

            if (page.CustomizableAreas.Count > 0)
            {
                //Customizable area doc in objdocument.Pages.CustomizableArea
                foreach (CustomizableArea customArea in page.CustomizableAreas)
                {
                    CustomAreaDocument(customArea, actionMode, ref pdf, ref errorMessage, ref defaultLogo, channelPartner);
                }
            }

            #endregion
        }


        /// <summary>
        /// Add Custom Area to the PDF Document
        /// </summary>
        /// <param name="customArea"></param>
        /// <param name="actionMode"></param>
        /// <param name="pdf"></param>
        /// <param name="errorMessage"></param>
        /// <param name="defaultLogo"></param>
        /// <param name="channelPartner"></param>
        private void CustomAreaDocument(CustomizableArea customArea, string actionMode, ref PdfManager pdf, ref string errorMessage, ref string defaultLogo, ChannelPartnerLogin channelPartner)
        {
            #region code

            if (customArea.TextArea != null)
            {                
                int fontCount = 1;

                int fontSize = 0;

                //Get the TextArea font for a Customizable Area
                foreach (CollateralCreator.Data.Font customFont in customArea.TextArea.Fonts)
                {
                    string textFontName = GetFontName(customFont.Name);

                    CustomCreateFontDocument(actionMode, ref pdf, channelPartner, customArea, customFont, textFontName, Convert.ToInt16(customFont.Size), GetFontFmaily(customFont.Bold), fontCount);

                    fontCount++;
                    fontSize = Convert.ToInt32(customFont.Size);
                }

                //appends text to the placeholder area
                AddTextInPlaceHolder(actionMode, customArea.TextArea, channelPartner, ref errorMessage);
                
                //creates textarea to the document                
                CustomTextAreaDocument(actionMode, customArea, ref pdf, fontSize);
               
                //updates text area after replacing text in placeholder
                CollateralCreatorRepository.DocumentUpdateTextArea(customArea.TextArea.AreaID, customArea.TextArea.TextAreaID, lines);
            }
            if (customArea.ImageArea != null)
            {
                CustomImageAreaDocument(actionMode, customArea, ref pdf, ref defaultLogo);
            }

            #endregion
        }

        /// <summary>
        /// Append text to the place holder copy
        /// </summary>
        /// <param name="actionMode"></param>
        /// <param name="textArea"></param>
        /// <param name="channelPartner"></param>
        /// <param name="errorMessage"></param>
        private void AddTextInPlaceHolder(string actionMode, TextArea textArea, ChannelPartnerLogin channelPartner, ref string errorMessage)
        {
            #region code 
            
            if (actionMode == "edit" || actionMode == "clone")
            {
                //search and replace the text area fields
                lines = textArea.Text;
                SearchReplace(ref lines, textArea.CharsPerLine, textArea.Lines, ref errorMessage);
            }
            else
            {
                lines = string.Empty;
                LoadDefaultDetails(ref lines, textArea.Text, textArea.CharsPerLine, textArea.Lines, ref errorMessage, channelPartner);
            }

            #endregion
        }

        /// <summary>
        ///  Add Custom TextArea to the PDF Document
        /// </summary>
        /// <param name="actionMode"></param>
        /// <param name="customArea"></param>
        /// <param name="pdf"></param>
        /// <param name="alignment"></param>
        /// <param name="fontSize"></param>
        private void CustomTextAreaDocument(string actionMode, CustomizableArea customArea, ref PdfManager pdf, int fontSize)
        {
            #region code

            if (actionMode == "smartcentre") //~MPE overload to keep the functions for sc and xct seperate for now
            {
                AddTextArea(actionMode, customArea, ref pdf, iTextSharp.text.Element.ALIGN_LEFT, fontSize);
            }
            else
            {
                AddTextArea(customArea, ref pdf, iTextSharp.text.Element.ALIGN_LEFT, fontSize);            
            }

            #endregion
        }


        /// <summary>
        /// Creates Custom Font for the textarea
        /// </summary>
        /// <param name="actionMode"></param>
        /// <param name="pdf"></param>
        /// <param name="channelPartner"></param>
        /// <param name="customArea"></param>
        /// <param name="customFont"></param>
        /// <param name="textFontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontCount"></param>
        private void CustomCreateFontDocument(string actionMode, ref PdfManager pdf, ChannelPartnerLogin channelPartner, CustomizableArea customArea, Data.Font customFont, string textFontName, int fontSize, int fontFamily, int fontCount)
        {
            #region code

            if (actionMode == "smartcentre")
            {
                pdf.CreateFont(channelPartner, customArea.TextArea.Text, textFontName, fontSize, fontFamily, customFont.Color, fontCount);
            }
            else
            {
                pdf.CreateFont(customArea.TextArea.Text, textFontName, fontSize, fontFamily, customFont.Color, fontCount);
            }

            #endregion
        }

        /// <summary>
        /// Returns font family from itextsharp
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        private int GetFontFmaily(bool font)
        {
            #region code

            if (font)
            {
                return iTextSharp.text.Font.BOLD;
            }
            else
            {
                return iTextSharp.text.Font.NORMAL;
            }

            #endregion
        }

        /// <summary>
        /// Add Custom Area to the PDF Document
        /// </summary>
        /// <param name="actionMode">which render mode, SC2 or NARS collateral view</param>
        /// <param name="customArea">Custom area object</param>
        /// <param name="pdf">pdf object</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="fontSize">Font size</param>
        private void AddTextArea(string actionMode, CustomizableArea customArea, ref PdfManager pdf, int alignment, int fontSize)
        {
            #region code

            //if (actionMode == "smartcentre") // this method is already overloaded for smartcentre with actionMode parameter
            //{
            pdf.AddSimpleColumnText(customArea.TextArea.Text, customArea.TextArea.CharsPerLine, customArea.TextArea.Lines, customArea.XPos, customArea.YPos, customArea.Width, customArea.Height, alignment, customArea.Rotation, customArea.TextArea.LineSpacing, fontSize);
            //}
            //else
            //{
            //    pdf.AddText(customArea.TextArea.Text, customArea.TextArea.CharsPerLine, customArea.TextArea.Lines, customArea.XPos, customArea.YPos, customArea.Width, customArea.Height, alignment, customArea.Rotation, customArea.TextArea.LineSpacing, fontSize);
            //}
            #endregion
        }
        
        /// <summary>
        /// Add Custom Area to the PDF Document
        /// </summary>
        /// <param name="customArea"></param>
        /// <param name="pdf"></param>
        /// <param name="alignment"></param>
        /// <param name="fontSize"></param>
        private void AddTextArea(CustomizableArea customArea, ref PdfManager pdf, int alignment, int fontSize)
        {
            #region code

            pdf.AddText(customArea.TextArea.Fonts, lines, customArea.TextArea.CharsPerLine, customArea.TextArea.Lines, customArea.XPos, customArea.YPos, customArea.Width, customArea.Height, alignment, customArea.Rotation, customArea.TextArea.LineSpacing, fontSize);
            
            #endregion
        }
        
        /// <summary>
        /// Add Custom Image to the PDF Document
        /// </summary>
        /// <param name="actionMode"></param>
        /// <param name="customArea"></param>
        /// <param name="pdf"></param>
        /// <param name="defaultLogo"></param>
        private void CustomImageAreaDocument(string actionMode, CustomizableArea customArea, ref PdfManager pdf, ref string defaultLogo)
        {
            #region code

            if (actionMode == "edit" || actionMode == "smartcentre")
            {
                AddImage(customArea, ref pdf);                
            }
            else
            {
                //Load default image from Channel Partner
                Xerox.SSOComponents.Models.Image img = null;

                if (this.ChannelPartner.Images.Count > 0)
                {
                    img = ChannelPartner.Images[0];
                    defaultLogo = "DefaultLogo";

                    //update the document image 
                    CollateralCreatorRepository.DocumentUploadImage(customArea.ImageArea.ImageAreaID.ToString(), img.image);

                    customArea.ImageArea.Image = img.image;
                    
                    AddImage(customArea, ref pdf);      
                }
            }

             #endregion
        }

        /// <summary>
        /// Add Custom Image to the PDF Document
        /// </summary>
        /// <param name="customArea"></param>
        /// <param name="pdf"></param>
        private void AddImage(CustomizableArea customArea, ref PdfManager pdf)
        {
            #region code

            pdf.AddImage(customArea.ImageArea.Image, customArea.ImageArea.Width, customArea.ImageArea.Height, customArea.XPos, customArea.YPos, customArea.Rotation);

            #endregion
        }

        /// <summary>
        /// Returns the correct fontname extension
        /// </summary>
        /// <param name="textFontName"></param>
        /// <returns></returns>
        private string GetFontName(string textFontName)
        {
            #region code

            string filePath = GetFontFilePath1(textFontName);

            if (File.Exists(filePath))
            {
                return textFontName + ".otf";
            }
            else 
            {
                return textFontName + ".ttf";
            }

            #endregion
        }

        /// <summary>
        /// Returns .otf font file  path
        /// </summary>
        /// <param name="textFontName"></param>
        /// <returns></returns>
        private string GetFontFilePath1(string textFontName)
        {
            #region code

            return HttpContext.Current.Server.MapPath("~/images/fonts/" + textFontName + ".otf");

            #endregion
        }

        /// <summary>
        /// Returns .ttf font file  path
        /// </summary>
        /// <param name="textFontName"></param>
        /// <returns></returns>
        private string GetFontFilePath2(string textFontName)
        {
            #region code

            return HttpContext.Current.Server.MapPath("~/images/fonts/" + textFontName + ".ttf");

            #endregion
        }
        
        /// <summary>
        /// Returns the source file name 
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="partnerBrand"></param>
        /// <returns></returns>
        private string GetSourceFilePath(int templateID, bool partnerBrand)
        {
            #region code

            if (partnerBrand)
                return HttpContext.Current.Server.MapPath("~/documents/" + GeneratePartnerBrandDocumentFile(templateID));
            else
                return HttpContext.Current.Server.MapPath("~/documents/" + GenerateXeroxBrandDocumentFile(templateID));

            #endregion
        }

        /// <summary>
        /// Returns generated document file path
        /// </summary>
        /// <param name="documentName"></param>
        /// <returns></returns>
        private string GetDocumentFilePath(string documentName)
        {
            #region

            return HttpContext.Current.Server.MapPath("~/temp/" + documentName);               

            #endregion
        }

        /// <summary>
        /// Returns the source file name for partner brand
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        private string GeneratePartnerBrandDocumentFile(int templateID)
        {
            #region code

            return "Template_" + templateID + "_pb.PDF";

            #endregion
        }

        /// <summary>
        /// Returns the source file name for xerox brand 
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        private string GenerateXeroxBrandDocumentFile(int templateID)
        {
            #region code
            
            return "Template_" + templateID + ".PDF";
            
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sActionMode"></param>
        /// <param name="objdocument"></param>
        /// <param name="pdf"></param>
        /// <param name="sFileNameNew"></param>
        /// <param name="sTemplatename"></param>
        /// <param name="sErrormessage"></param>
        /// <param name="sDefaultLogo"></param>
        public void Document(string actionMode, CollateralCreator.Data.Document objDocument, ref PdfManager pdf, ref string fileNameNew, ref string templateName, ref string errorMessage, ref string defaultLogo, ChannelPartnerLogin channelPartner)
        {
            #region code

            DocumentCreation(actionMode, objDocument, ref pdf, ref fileNameNew, ref templateName, ref errorMessage, ref defaultLogo, channelPartner);

            #endregion
        }

        /// <summary>
        /// appends the default channel partner details to the placeholder
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sTextArea"></param>
        /// <param name="a"></param>
        /// <param name="iMaxcharsPerline"></param>
        /// <param name="iMaxlines"></param>
        /// <param name="sErrorMessage"></param>
        /// <param name="ChannelPartner"></param>
        private void LoadDefaultDetails(ref string lines, string textArea, int maxCharsPerline, int maxlines, ref string errorMessage, ChannelPartnerLogin channelPartner)
        {
            #region code
            
            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0) 
                a = this.ChannelPartner.Addresses[0];

            if (a != null && lines == string.Empty && (textArea.Contains("<address_line1>") || textArea.Contains("<address_line2>") || textArea.Contains("<address_line3>")
                || textArea.Contains("<address_city>") || textArea.Contains("<address_state>") || textArea.Contains("<address_country>") || textArea.Contains("<address_postcode>")))
            {
                if (a.AddressLine1 != null && a.AddressLine1 != string.Empty)
                    Append(ref lines, "", a.AddressLine1);

                if (a.AddressLine2 == string.Empty && a.AddressLine3 == string.Empty && a.Town.Length == 2)
                    Append(ref lines, ", ", a.Town);

                if (a.AddressLine2 != null && a.AddressLine2 != string.Empty)
                    Append(ref lines, "\r\n", a.AddressLine2);

                if (a.AddressLine3 == string.Empty && a.Town.Length == 2)
                    Append(ref lines, ", ", a.Town);

                if (a.AddressLine3 != null && a.AddressLine3 != string.Empty)
                    Append(ref lines, "\r\n", a.AddressLine3);

                if (a.Town != null && a.Town != string.Empty && a.Town.Length > 2)
                    Append(ref lines, "\r\n", a.Town);

                if (a.State != string.Empty && a.State.Length == 2)
                    Append(ref lines, ", ", a.State);

                if (a.State != null && a.State != string.Empty)                    
                    Append(ref lines, "\r\n", a.State);

                if (a.Country != string.Empty && a.Country.Length == 2)
                    Append(ref lines, ", ", a.Country);

                if (a.Country != null && a.Country != string.Empty)
                    Append(ref lines, "\r\n", a.Country);

                if (a.PostCode != null && a.PostCode != string.Empty)
                    Append(ref lines, "\r\n", a.PostCode);
            }
            //append smart centre optional parameters to the document if present
            else if (a == null && channelPartner != null && (textArea.Contains("<address_line1>") || textArea.Contains("<address_line2>") || textArea.Contains("<address_line3>")
                || textArea.Contains("<address_city>") || textArea.Contains("<address_state>") || textArea.Contains("<address_country>") || textArea.Contains("<address_postcode>")))
            {
                if (!String.IsNullOrEmpty(channelPartner.AddressLine1))
                    Append(ref lines, "", channelPartner.AddressLine1);

                if (!String.IsNullOrEmpty(channelPartner.AddressLine2))
                    Append(ref lines, "\r\n", channelPartner.AddressLine2);

                if (!String.IsNullOrEmpty(channelPartner.AddressLine3))
                    Append(ref lines, "\r\n", channelPartner.AddressLine3);

                if (!String.IsNullOrEmpty(channelPartner.AddressLine4))
                    Append(ref lines, "\r\n", channelPartner.AddressLine4);

                if (!String.IsNullOrEmpty(channelPartner.State))
                    Append(ref lines, "\r\n", channelPartner.State);

                if (!String.IsNullOrEmpty(channelPartner.ZipCode))
                    Append(ref lines, "\r\n", channelPartner.ZipCode);
            }
            //logic for promo text without editable content
            else if (a != null && lines == string.Empty && (!textArea.Contains("<")) && (!textArea.Contains(">"))) 
            {
                Append(ref lines, "", textArea);
            }

            //append smart centre text to the document
            if (a == null && channelPartner != null && lines == string.Empty && (textArea.Contains("<")) && (textArea.Contains(">")))
            {
                Append(ref lines, "", textArea);
            }

            if (lines == string.Empty && (textArea.Contains("<company_name>") || textArea.Contains("<web_site>") || textArea.Contains("<phone_number>")))
            {
                lines = textArea;

                if (textArea.Contains("<company_name>")) 
                {
                    if (this.ChannelPartner.LocationName != string.Empty)
                    {
                        Replace(ref lines, "<company_name>", this.ChannelPartner.LocationName);
                    }
                    //append smart centre optional parameters to the document if present
                    if (this.ChannelPartner.LocationName == string.Empty && channelPartner != null)
                    {
                        Replace(ref lines, "<company_name>", channelPartner.CompanyName);
                    }
                }


                if (textArea.Contains("<web_site>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in this.ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("website"))
                        {
                            Replace(ref lines, "<web_site>", inf.Value);
                        }
                    }

                    //append smart centre optional parameters to the document if present
                    if (this.ChannelPartner.Info.Count == 0 && channelPartner != null) 
                    {
                        Replace(ref lines, "<web_site>", channelPartner.CompanyWebUrl);
                    }
                }
                if (textArea.Contains("<phone_number>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in this.ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("phone"))
                            Replace(ref lines, "<phone_number>", inf.Value);
                    }

                    //append smart centre optional parameters to the document if present
                    if (this.ChannelPartner.Info.Count == 0 && channelPartner != null)
                    {
                        Replace(ref lines, "<phone_number>", channelPartner.PhoneNumber);
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Appends
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="placeholder"></param>
        /// <param name="placeholderText"></param>
        private void Append(ref string lines, string separator, string textToAppend)
        {
            #region code

            if (separator == string.Empty)
            {
                lines = lines + textToAppend;
            }
            else
            {
                lines = lines + separator + textToAppend;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="a"></param>
        /// <param name="iMaxcharsPerline"></param>
        /// <param name="iMaxlines"></param>
        /// <param name="sErrorMessage"></param>
        /// <param name="ChannelPartner"></param>
        private void SearchReplace(ref string lines, int maxCharsPerline, int maxlines, ref string errorMessage)
        {
            #region code

            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0) a = this.ChannelPartner.Addresses[0];

            if (a != null)
            {
                if (lines.Contains("<reseller_name>") && this.ChannelPartner.FirstName != string.Empty)
                    Replace(ref lines, "<reseller_name>", ChannelPartner.FirstName);
                    
                if (lines.Contains("<address_line1>") && a.AddressLine1 != string.Empty)
                    Replace(ref lines, "<address_line1>", a.AddressLine1);
                
                if (lines.Contains("<address_line2>") && a.AddressLine2 != string.Empty)
                    Replace(ref lines, "<address_line2>", a.AddressLine3);

                if (lines.Contains("<address_line3>") && a.AddressLine3 != string.Empty)
                    Replace(ref lines, "<address_line3>", a.AddressLine3);

                if (lines.Contains("<address_city>") && a.Town != string.Empty)
                    Replace(ref lines, "<address_city>", a.Town);

                if (lines.Contains("<address_state>") && a.State != string.Empty)
                    Replace(ref lines, "<address_state>", a.State);

                if (lines.Contains("<address_country>") && a.Country != string.Empty)
                    Replace(ref lines, "<address_country>", a.Country);

                if (lines.Contains("<address_postcode>") && a.PostCode != string.Empty)
                    Replace(ref lines, "<address_postcode>", a.PostCode);
            }
            if (lines.Contains("<company_name>") || lines.Contains("<web_site>") || lines.Contains("<phone_number>"))
            {
                if (lines.Contains("<company_name>"))
                    if (this.ChannelPartner.LocationName != string.Empty)
                        Replace(ref lines, "<company_name>", this.ChannelPartner.LocationName);

                if (lines.Contains("<web_site>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in this.ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("website"))
                            Replace(ref lines, "<web_site>", inf.Value);
                    }
                }
                if (lines.Contains("<phone_number>"))
                {
                    foreach (Xerox.SSOComponents.Models.Info inf in this.ChannelPartner.Info)
                    {
                        if (inf.Name.ToLower().Contains("phone"))
                            Replace(ref lines, "<phone_number>", inf.Value);
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Replaces the placeholder text
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="placeholder"></param>
        /// <param name="placeHolderText"></param>
        private void Replace(ref string lines, string placeholder, string placeHolderText)
        {
            #region code

            lines = lines.Replace(placeholder, placeHolderText);

            #endregion
        }

        #endregion
    }
  
}