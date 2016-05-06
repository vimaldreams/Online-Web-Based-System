using iTextSharp.text;
using iTextSharp.text.pdf;
using CollateralCreator.Data;
using CollateralCreator.Business;

/// <summary>
/// Class to generate PDF within the application.
/// </summary>
namespace CollateralCreator.Web
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Drawing;
    using System.Threading;
    
    public class PdfManager : XeroxWebPage
    {
        #region Member Variables

        public static readonly float __MarginTop = 0F;
        public static readonly float __MarginBottom = 0F;
        public static readonly float __MarginLeft = 0F;
        public static readonly float __MarginRight = 0F;

        private float __PageWidth { get; set; }
        private float __PageHeight { get; set; }

        private iTextSharp.text.Rectangle __PageSize;

        private iTextSharp.text.Document _Doc;
        private PdfContentByte _ContentByte;
        private BaseFont _bfXerox;
        private iTextSharp.text.Font _fontXerox;
        private PdfWriter _PdfWriter;
        private bool _Rotation;
        private string sFilePath;
        private Paragraph sParagraph;
        private PdfContentByte cb { get { return this._ContentByte; } }
        private PdfWriter pw { get { return this._PdfWriter; } }
        private bool rot { get { return this._Rotation; } }

        private string sUrlFont;
        private float sUrlFontSize { get; set; }
        private int iUrlFontStyle { get; set; }
        private BaseColor UrlBaseColor { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFileNameNew"></param>
        /// <param name="objdocument"></param>
        public void GenerateDocument(ref string fileNameNew, CollateralCreator.Data.Document objDocument)
        {
            #region code

            if (objDocument.DocumentID != 0)
            {
                DateTime dt = DateTime.Now;
                long lTime = dt.Hour * 60 * 60 * 1000 + dt.Minute * 60 * 1000 + dt.Second * 1000 + dt.Millisecond;

                fileNameNew = "document" + objDocument.DocumentID + ".pdf";
            }           

            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFileNameNew"></param>
        public void DocumentOpen(string fileNameNew)
        {
            #region code

            sFilePath = HttpContext.Current.Server.MapPath("~/temp/" + fileNameNew);
            this._Doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate());
            this._PdfWriter = PdfWriter.GetInstance(this._Doc, new FileStream(sFilePath, FileMode.Create));
            this._Doc.Open();
            this._ContentByte = this._PdfWriter.DirectContent;
           
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFileNameNew"></param>
        /// <param name="sPageSize"></param>
        public void DocumentOpen(string fileNameNew, string pageSize)
        {
            #region code

            sFilePath = HttpContext.Current.Server.MapPath("~/temp/" + fileNameNew);

            switch (pageSize.ToLower())
            {
                case "a4":
                    __PageSize = iTextSharp.text.PageSize.A4;
                    __PageWidth = 595;
                    __PageHeight = 842F;
                    break;
                case "a4landscape":
                    __PageSize = iTextSharp.text.PageSize.A4.Rotate();
                    __PageWidth = 842F;
                    __PageHeight = 595;
                    break;
                case "letter":
                    __PageSize = iTextSharp.text.PageSize.LETTER;
                    __PageWidth = 612F;
                    __PageHeight = 792F;
                    break;
                case "letterlandscape":
                    __PageSize = iTextSharp.text.PageSize.LETTER.Rotate();
                    __PageWidth = 792F;
                    __PageHeight = 612F;
                    break;
                case "a3":
                    __PageSize = iTextSharp.text.PageSize.A3;
                    break;
                case "a3landscape":
                    __PageSize = iTextSharp.text.PageSize.A3.Rotate();
                    break;
                case "a5":
                    __PageSize = iTextSharp.text.PageSize.A5;
                    __PageWidth = 420F;
                    __PageHeight = 595F;                    
                    break;
                case "a5landscape":
                    __PageSize = iTextSharp.text.PageSize.A5.Rotate();
                    __PageWidth = 595F;
                    __PageHeight = 420F;     
                    break;
                case "postcard":
                    __PageSize = iTextSharp.text.PageSize.POSTCARD;
                    __PageWidth = 283F;
                    __PageHeight = 416F;
                    break;
                case "a2":
                    __PageSize = iTextSharp.text.PageSize.A2;
                    break;
                case "a2landscape":
                    __PageSize = iTextSharp.text.PageSize.A2.Rotate();
                    break;
                case "a1":
                    __PageSize = iTextSharp.text.PageSize.A1;
                    break;
                case "a1landscape":
                    __PageSize = iTextSharp.text.PageSize.A1.Rotate();
                    break;
                case "legal":
                    __PageSize = iTextSharp.text.PageSize.LEGAL;
                    break;
                case "legallandscape":
                    __PageSize = iTextSharp.text.PageSize.LEGAL_LANDSCAPE;
                    break;
                case "penguinbanner":
                    __PageSize = iTextSharp.text.PageSize.PENGUIN_LARGE_PAPERBACK;
                    __PageWidth = 365;
                    __PageHeight = 561F;
                    break;
                default:
                    __PageSize = iTextSharp.text.PageSize.A4;
                    break;
            }
            this._Doc = new iTextSharp.text.Document(__PageSize);
            this._PdfWriter = PdfWriter.GetInstance(this._Doc, new FileStream(sFilePath, FileMode.Create));
            this._Doc.Open();
            this._ContentByte = this._PdfWriter.DirectContent;
           

            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void DocumentClose()
        {
            #region code

            if (this._Doc != null)
            {
                this._Doc.Close();
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteImage"></param>
        /// <param name="fwidth"></param>
        /// <param name="fHeight"></param>
        /// <param name="fabsoluteX"></param>
        /// <param name="fabsoluteY"></param>
        /// <param name="iRotationDegree"></param>
        public void AddImage(byte[] byteImage, float width, float height, float absoluteX, float absoluteY, float rotationDegree)
        {
            #region code

            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(byteImage);

            //logic to increase the Area Size by 20%  --  + (width*0.2)   + (height * 0.2)
      
            //Resize picture to fit aspect ratio if its bigger
            int intMaxWidth = Convert.ToInt16(width); 
            int intMaxHeight = Convert.ToInt16(height);

            if (imgLogo.Height > intMaxHeight || imgLogo.Width > intMaxWidth)
            {
                double dblHeightRatio = Convert.ToDouble(intMaxHeight) / Convert.ToDouble(imgLogo.Height);
                double dblWidthRatio = Convert.ToDouble(intMaxWidth) / Convert.ToDouble(imgLogo.Width);
                double dblScaleRatio;

                //Use the smaller ratio
                if (dblHeightRatio > dblWidthRatio)
                {
                    dblScaleRatio = dblWidthRatio;
                }
                else
                {
                    dblScaleRatio = dblHeightRatio;
                }

                int intNewHeight = Convert.ToInt32(imgLogo.Height * dblScaleRatio);
                int intNewWidth = Convert.ToInt32(imgLogo.Width * dblScaleRatio);

                imgLogo.ScaleAbsolute((float)intNewWidth, (float)intNewHeight);
            }

            if (rotationDegree == 0)
            {
                imgLogo.SetAbsolutePosition(PositionX(absoluteX), PositionY(absoluteY + height));
            }
            else
            {
                imgLogo.SetAbsolutePosition(PositionX(absoluteX), PositionY(absoluteY - 30));
                imgLogo.RotationDegrees = rotationDegree;
            }

            //imgLogo.CompressionLevel = 9;

            cb.AddImage(imgLogo);

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <param name="rotationDegree"></param>
        public void AddWatermarkImage(byte[] byteImage, float width, float height, float absoluteX, float absoluteY, float rotationDegree)
        {
            #region code

            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(byteImage);

            //Resize picture to fit aspect ratio if its bigger
            int intMaxWidth = Convert.ToInt16(width);
            int intMaxHeight = Convert.ToInt16(height);

            if (imgLogo.Height > intMaxHeight || imgLogo.Width > intMaxWidth)
            {
                double dblHeightRatio = Convert.ToDouble(intMaxHeight) / Convert.ToDouble(imgLogo.Height);
                double dblWidthRatio = Convert.ToDouble(intMaxWidth) / Convert.ToDouble(imgLogo.Width);
                double dblScaleRatio;

                //Use the smaller ratio
                if (dblHeightRatio > dblWidthRatio)
                {
                    dblScaleRatio = dblWidthRatio;
                }
                else
                {
                    dblScaleRatio = dblHeightRatio;
                }

                int intNewHeight = Convert.ToInt32(imgLogo.Height * dblScaleRatio);
                int intNewWidth = Convert.ToInt32(imgLogo.Width * dblScaleRatio);

                imgLogo.ScaleAbsolute((float)intNewWidth, (float)intNewHeight);
            }

            if (rotationDegree == 0)
            {
                imgLogo.SetAbsolutePosition(PositionX(absoluteX), PositionY(absoluteY + height));
            }
            else
            {
                imgLogo.SetAbsolutePosition(PositionX(absoluteX), PositionY(absoluteY - 30));
                imgLogo.RotationDegrees = rotationDegree;
            }

            cb.AddImage(imgLogo);

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="sreference"></param>
        /// <param name="iAlignment"></param>
        /// <param name="fXcoordinate"></param>
        /// <param name="fYcoordinate"></param>
        /// <param name="iRotationDegree"></param>
        public void AddHyperLinkText(string text, string reference, int alignment, float xCoordinate, float yCoordinate, float rotationDegree)
        {
            #region code

            this.sParagraph = new Paragraph();
            Anchor anchor = new Anchor(text, FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL, SpotColor.DARK_GRAY));
            anchor.Reference = reference;
            this.sParagraph.Add(anchor);
            ColumnText.ShowTextAligned(cb, alignment, new Phrase(this.sParagraph), xCoordinate, yCoordinate, rotationDegree);

            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sParagraph"></param>
        /// <param name="sText"></param>
        /// <param name="sreference"></param>
        public void AddHyperLinkText(ref Paragraph paragraph, string text, string reference)
        {
            #region code

            Anchor anchor = new Anchor(text, FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL, SpotColor.DARK_GRAY));
            anchor.Reference = reference;
            this.sParagraph.Add(anchor);

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lFonts"></param>
        /// <param name="sTextArea"></param>
        /// <param name="iCharsPerLine"></param>
        /// <param name="iLines"></param>
        /// <param name="fPosX"></param>
        /// <param name="fPosY"></param>
        /// <param name="fWidth"></param>
        /// <param name="fHeight"></param>
        /// <param name="iAlignment"></param>
        /// <param name="fRotation"></param>
        /// <param name="fLineSpacing"></param>
        public void AddText(List<CollateralCreator.Data.Font> fonts, string textArea, int charsPerLine, int lines, float posX, float posY, float width, float height, int alignment, float rotation, float lineSpacing, int fontSize)
        {
            #region code

            string[] fTextArea = SplitTextArea(textArea);

            float fxposition = PositionX(posX);
            float fyposition = PositionY(posY);
            
            foreach (string str in fTextArea)
            {
                if (!(fTextArea.Length > 5) && (str.ToLower().Contains("visit") || str.ToLower().Contains("http://") || str.ToLower().Contains("https://") || str.ToLower().Contains("www")
                    || str.ToLower().Contains("www.") || str.ToLower().Contains("co.") || str.ToLower().Contains(".com")))
                {
                    // Location of the letter.
                    int i = 0;
                    if (str.Contains("<web_site>"))
                        i = str.IndexOf("web");
                    else if (str.Contains("visit"))
                        i = str.IndexOf("visit");
                    else 
                        i = -5;

                    // Remainder of string starting at.
                    string strUrl = str.Substring(i + 5);

                    string strContent = str.Substring(0, i + 5);

                    Phrase phrase = new Phrase(strContent, this._fontXerox);

                    if (sUrlFont != null || sUrlFontSize != 0.0 || iUrlFontStyle != 0 || UrlBaseColor != null)
                    {
                        phrase.Add(new Anchor(strUrl, FontFactory.GetFont(sUrlFont, sUrlFontSize, iUrlFontStyle, UrlBaseColor)));
                    }
                    else
                    {
                        phrase.Add(strUrl);                        
                    }
                    
                    fyposition = fyposition - 8;

                    AddAlignedText(cb, alignment, phrase, fxposition, fyposition, rotation);
                }
                else if (str.Contains("Big savings") || str.Contains("Massive savings") || str.Contains("Best-ever savings"))
                {
                    fyposition = fyposition - 8;

                    AddAlignedText(cb, alignment, new Phrase(str, FontFactory.GetFont(sUrlFont, sUrlFontSize, iUrlFontStyle, UrlBaseColor)), fxposition, fyposition, rotation);

                }
                else {
                    //if (fRotation == 0)
                    //    fyposition = fyposition - 8;
                    //~MPE added extra dimension in spacing dependent upon font size
                    if (fontSize >= 20)
                    {
                        fyposition = fyposition - 10;
                    }
                    else if (fontSize > 12 && fontSize < 20)
                    {
                        fyposition = fyposition - 8;
                    }
                    else
                    {
                        if (rotation == 0)
                        {
                            fyposition = fyposition - 8;
                        }
                    }
                    AddAlignedText(cb, alignment, new Phrase(str, this._fontXerox), fxposition, fyposition, rotation);
                }
                if (rotation == 0)
                    fyposition = fyposition - lineSpacing;
                else
                    fxposition = fxposition + 12;
            }

            #endregion
        }

        /// <summary>
        /// Adds the text to a text area - smart centre function tries to wrap text in the limits of the box width and height
        /// </summary>
        /// <param name="action">which route is the text function being called from</param>
        /// <param name="lFonts">List of fonts</param>
        /// <param name="sTextArea">TextArea text</param>
        /// <param name="iCharsPerLine">Number of characters per line</param>
        /// <param name="iLines">Line</param>
        /// <param name="fPosX">X Position in px</param>
        /// <param name="fPosY">Y Position in px</param>
        /// <param name="fWidth">Width in px</param>
        /// <param name="fHeight">Height in px</param>
        /// <param name="iAlignment">Alignment</param>
        /// <param name="fRotation">Float int</param>
        /// <param name="fLineSpacing">Line spacing</param>
        public void AddText(string textArea, int charsPerLine, int lines, float posX, float posY, float width, float height, int alignment, float rotation, float lineSpacing, int fontSize)
        {
            #region code            

            string[] fTextArea = SplitTextArea(textArea);                     

            float fxposition = PositionX(posX);
            float fyposition = PositionY(posY);
            float llx = PositionX(posX + width);
            float lly = PositionY(posY + height);
            float urx = fxposition;
            float ury = fyposition;

            int counter = 8;

            foreach (string str in fTextArea)
            {
                GetStartingXYPosition(rotation, fontSize, ref fxposition, ref fyposition);

                //add superscript for trademark 
                string[] splitScript;

                if (str.Contains("®"))
                {
                    splitScript = Regex.Split(str, "®");

                    Phrase phrase = new Phrase();

                    for (int i = 0; i < splitScript.Length; i++)
                    {
                        Chunk superScript = new Chunk("®", FontFactory.GetFont(sUrlFont, sUrlFontSize, iUrlFontStyle, UrlBaseColor));

                        superScript.SetTextRise(GetSuperScriptRise(sUrlFontSize));

                        phrase.Add(new Chunk(splitScript[i]));//, FontFactory.GetFont(sUrlFont, sUrlFontSize, iUrlFontStyle, UrlBaseColor)
                        
                        //add superscript only where available
                        if (i < splitScript.Length - 1)
                        {
                            phrase.Add(superScript);                            
                        }
                    }

                    AddAlignedText(cb, alignment, new Paragraph(0, phrase.Content, this._fontXerox), fxposition, fyposition, rotation);
                }
                else
                {
                    AddAlignedText(cb, alignment, new Phrase(str, this._fontXerox), fxposition, fyposition, rotation);
                }
                
                GetNextXYPosition(rotation, fontSize, ref fxposition, ref fyposition, ref counter, ref lineSpacing);                     
            }

            #endregion
        }

      
        /// <summary>
        /// New column text, solves word wrapping issue ~MPE 19/08/2013
        /// </summary>
        /// <param name="fonts">Font objct</param>
        /// <param name="textArea">Text area name</param>
        /// <param name="charsPerLine">Characters per line</param>
        /// <param name="lines">Number of lines</param>
        /// <param name="posX">X position in px (top left corner)</param>
        /// <param name="posY">Y position in px (top left corner)</param>
        /// <param name="width">Width of box in px</param>
        /// <param name="height">Height of box in px</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="rotation">Rotation from 0 degrees rotation clockwise</param>
        /// <param name="lineSpacing">Line spacing</param>
        /// <param name="fontSize">Font size in px</param>
        public void AddSimpleColumnText(string textArea, int charsPerLine, int lines, float posX, float posY, float width, float height, int alignment, float rotation, float lineSpacing, int fontSize)
        {
            #region Code

            ColumnText ct = new ColumnText(cb);

            float llx = PositionX(posX);
            float lly = PositionY(posY + height);            
            float urx = PositionX(posX + width);
            float ury = PositionY(posY);

            //Phrase phraseText = new Phrase(textArea, this._fontXerox);

            //use paragraph component to assign leading space value
            Paragraph paraText = new Paragraph(textArea, this._fontXerox);

            if (rotation == 0)
            {
                //ct.AddText(phraseText);
                ct.SetSimpleColumn(llx, lly, urx, ury);

                //make default leading space to 0
                paraText.Leading = 0;

                //assign leading space in floating precision number
                paraText.MultipliedLeading = 1.3F;

                ct.AddElement(paraText);
                ct.Go();
            }
            else
            {
                //use pdf component for rotation of text
                PdfPTable table = new PdfPTable(1);
                table.TotalWidth = width;

                PdfPCell cell = new PdfPCell(paraText);
                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell.Rotation = Convert.ToInt32(rotation);
                table.AddCell(cell);

                ct.SetSimpleColumn(llx, lly, urx, ury);

                //make default leading space to 0
                paraText.Leading = 0;

                //assign leading space in floating precision number
                paraText.MultipliedLeading = 1.3F;
                
                ct.AddElement(table);
                ct.Go();
            }

            #endregion
        }

        /// <summary>
        /// Add aligned text to the pdf 
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="alignment"></param>
        /// <param name="phrase"></param>
        /// <param name="fxposition"></param>
        /// <param name="fyposition"></param>
        /// <param name="rotation"></param>
        private void AddAlignedText(PdfContentByte cb, int alignment, Phrase phrase, float fxposition, float fyposition, float rotation)
        {
            #region code

            ColumnText.ShowTextAligned(cb, alignment, phrase, fxposition, fyposition, rotation);

            #endregion
        }

        /// <summary>
        /// returns X & Y coordinate for the corresponding rotation degree after appending first line
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="fontSize"></param>
        /// <param name="fxposition"></param>
        /// <param name="fyposition"></param>
        /// <param name="counter"></param>
        /// <param name="lineSpacing"></param>
        private void GetNextXYPosition(float rotation, int fontSize, ref float fxposition, ref float fyposition, ref int counter, ref float lineSpacing)
        {
            #region code 

            //logic which starts after appending the first line of text in the rotation degree
            if (rotation == 0)
            {
                fyposition = fyposition - lineSpacing;
            }
            else if (rotation == 180)
            {
                if (fontSize >= 20)
                {
                    fyposition = fyposition + 30;

                    //to align the text from the same position 
                    fxposition = fxposition + counter + 3;                    
                }
                else if (fontSize > 12 && fontSize < 20)
                {
                    fyposition = fyposition + 15;

                    //to align the text from the same position 
                    if (counter < 6)
                        fxposition = fxposition + counter + 4;
                    else
                        fxposition = fxposition + counter + 1;
                }
                else
                {
                    fyposition = fyposition + 12;

                    //to align the text from the same position 
                    if (counter < 5)
                        fxposition = fxposition + counter + 5;
                    else
                        fxposition = fxposition + counter;
                }
            }
            else
            {
                fxposition = fxposition - 12;
            }
            counter--;

            #endregion
        }

        /// <summary>
        /// returns X & Y coordinate for the corresponding rotation degree for first line
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="fontSize"></param>
        /// <param name="fxposition"></param>
        /// <param name="fyposition"></param>
        private void GetStartingXYPosition(float rotation, int fontSize, ref float fxposition, ref float fyposition)
        {
            #region code

            if (rotation == 0)
            {
                if (fontSize >= 20)
                {
                    fyposition = fyposition - 10;
                }
                else if (fontSize > 12 && fontSize < 20)
                {
                    fyposition = fyposition - 8;
                }
                else
                {
                    fyposition = fyposition - 7;
                }
            }
            else if (rotation == 180)
            {
                if (fontSize >= 20)
                {
                    fxposition = fxposition - 10;
                }
                else if (fontSize > 12 && fontSize < 20)
                {
                    fxposition = fxposition - 8;
                }
                else
                {
                    fxposition = fxposition - 7;
                }
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textArea"></param>
        /// <returns></returns>
        private string[] SplitTextArea(string textArea)
        {
            #region code

            string[] arrTextArea;
            
            if (textArea.Contains("\r\n"))
            {
                arrTextArea = Regex.Split(textArea, "\r\n");
            }
            else
            {
                arrTextArea = Regex.Split(textArea, "\n");
            }

            return arrTextArea;

            #endregion
        }

        /// <summary>
        /// sets the value for superscript based on fontsize 
        /// </summary>
        /// <param name="sUrlFontSize"></param>
        /// <returns></returns>
        private float GetSuperScriptRise(float sUrlFontSize)
        {
            #region code

            if (sUrlFontSize >= 25)
            {
                return(7.0f);
            }
            else if (sUrlFontSize >= 15 & sUrlFontSize <= 24)
            {
                return (6.0f);
            }
            else if (sUrlFontSize >= 10 & sUrlFontSize <= 14)
            {
                return (5.0f);
            }
            else if (sUrlFontSize >= 1 & sUrlFontSize <= 9)
            {
                return (3.0f);
            }
            else
            {
                return (0.0f);
            }

            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBlockWidth"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        private bool CharacterLengthCounter(int textBlockWidth, string text, int fontSize)
        {
            #region Code
            try
            {
                int fieldSize = text.Length;
                //double pixelWidth = (fieldSize * fontSize * 1.15755) + (fontSize * 0.44445);
                double pixelWidth = ((fieldSize * fontSize *  1.15755)/2) + (fontSize * 0.44445);
                int finalWidth = Convert.ToInt32(pixelWidth);
                if (finalWidth > textBlockWidth)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
            #endregion
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sTextArea"></param>
        /// <param name="sFontfile"></param>
        /// <param name="iFontsize"></param>
        /// <param name="Fontfamily"></param>
        /// <param name="lFontcolor"></param>
        /// <param name="iFontcount"></param>
        public void CreateFont(string textArea, string fontFile, int fontSize, int fontFamily, long fontColor, int fontCount)
        {
            #region code

            long cvalue = fontColor;
            int blue = Convert.ToInt16(cvalue % 256);
            cvalue = (cvalue/256);
            int green = Convert.ToInt16(cvalue%256);
            int red = Convert.ToInt16(cvalue/256);

            this._bfXerox = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/images/fonts/" + fontFile), BaseFont.CP1252, BaseFont.EMBEDDED);

            if (fontFile.Contains("-Bold")) 
            {
                sUrlFont = this._bfXerox.ToString();
                sUrlFontSize = fontSize;
                iUrlFontStyle = fontFamily;
                UrlBaseColor = new BaseColor(red, green, blue);
            }
            else
            {
                this._fontXerox = new iTextSharp.text.Font(this._bfXerox, fontSize, fontFamily, new BaseColor(red, green, blue));
            }

            #endregion
        }

        /// <summary>
        ///  method to create font encoded for greek language
        /// </summary>
        /// <param name="sTextArea"></param>
        /// <param name="sFontfile"></param>
        /// <param name="iFontsize"></param>
        /// <param name="Fontfamily"></param>
        /// <param name="lFontcolor"></param>
        /// <param name="iFontcount"></param>
        public void CreateFont(ChannelPartnerLogin channelPartner, string textArea, string fontFile, int fontSize, int fontFamily, long fontColor, int fontCount)
        {
            #region code

            long cvalue = fontColor;
            int blue = Convert.ToInt16(cvalue % 256);
            cvalue = (cvalue / 256);
            int green = Convert.ToInt16(cvalue % 256);
            int red = Convert.ToInt16(cvalue / 256);

            this._bfXerox = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/images/fonts/" + fontFile), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            if (fontFile.Contains("-Bold")) 
            {
                sUrlFont = this._bfXerox.ToString();
                sUrlFontSize = fontSize;
                iUrlFontStyle = fontFamily;
                UrlBaseColor = new BaseColor(red, green, blue);
            }
            else
            {
                sUrlFont = this._bfXerox.ToString();
                sUrlFontSize = fontSize;
                iUrlFontStyle = fontFamily;
                UrlBaseColor = new BaseColor(red, green, blue);

                this._fontXerox = new iTextSharp.text.Font(this._bfXerox, fontSize, fontFamily, new BaseColor(red, green, blue));
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPath"></param>
        public void AddTemplateRest(string path)
        {
            #region code

            if (this._Doc == null) return;
            PdfReader reader = new PdfReader(path);
            if (reader.NumberOfPages < 2) return;
            for (int i = 2; i <= reader.NumberOfPages; i++)
            {
                this._Doc.NewPage();
                PdfImportedPage page = pw.GetImportedPage(reader, i);
                cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
            }
            reader.Close();

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="iPageNum"></param>
        public void AddTemplateNextPage(string path, int pageNum)
        {
            #region code

            if (this._Doc == null) return;
            PdfReader reader = new PdfReader(path);
            if (reader.NumberOfPages == 0) return;
            this._Doc.NewPage();
            PdfImportedPage page = pw.GetImportedPage(reader, pageNum);
            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
            reader.Close();

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPath"></param>
        public void AddTemplateFirstPage(string path)
        {
            #region code

            if (this._Doc == null) return;
            PdfReader reader = new PdfReader(path);
            if (reader.NumberOfPages == 0) return;
            PdfImportedPage page = pw.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 1, 0, 0, 1, 0, 0);
            reader.Close();

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void SetCompression(string absolutepath, string virtualPath)
        {
            #region

            PdfReader reader = new PdfReader(absolutepath);

            if (File.Exists(absolutepath))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/temp/" + virtualPath));

                PdfStamper stamper = new PdfStamper(reader, new FileStream(absolutepath, FileMode.Create), PdfWriter.VERSION_1_5);
                stamper.FormFlattening = true;
                stamper.SetFullCompression();
                stamper.Close();
            }

            reader.Close();

            #endregion
        }

        #endregion

        #region helper functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fX"></param>
        /// <returns></returns>
        private float PositionX(float fX)
        {
            float fML = __MarginLeft, fPW = __PageWidth, fMR = __MarginRight;
            if (this.rot)
            {
                fPW = __PageHeight;
                fML = __MarginBottom;
                fMR = __MarginTop;
            }
            float f = fML + fX;
            if (fX < 0) f = fPW - fMR + fX;
            if (f > fPW - fMR) f = fPW;
            if (f < fML) f = fML;
            return f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fY"></param>
        /// <returns></returns>
        private float PositionY(float fY)
        {
            float fPH = __PageHeight, fMT = __MarginTop, fMB = __MarginBottom;
            if (this.rot)
            {
                fPH = __PageWidth;
                fMT = __MarginLeft;
                fMB = __MarginRight;
            }
            float f = fPH - fMT - fY;
            if (f < fMB) f = fPH;
            return f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private float PageWidth()
        {
            float fWitdh = __PageWidth - __MarginLeft - __MarginRight;
            if (this.rot) fWitdh = __PageHeight - __MarginTop - __MarginBottom;
            return fWitdh;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private float PageHeight()
        {
            float fHeight = __PageHeight - __MarginTop - __MarginBottom;
            if (this.rot) fHeight = __PageWidth - __MarginLeft - __MarginRight;
            return fHeight;
        }

        #endregion
       
    }
}
