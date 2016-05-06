using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using CollateralCreatorAdminWeb.Models;

namespace XCTUploadToolWeb
{
    public partial class Main : System.Web.UI.Page
    {
        private XeroxCCToolEntities xctEntities = new XeroxCCToolEntities();



        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void CmdConvert_Click(object sender, EventArgs e)
        {
            #region Code
            string whereDidIget = "0";
            try
            {
                int pageId = 0;
                bool imagePartnerBrand = true;
                List<int> pageIds = new List<int>();

                string documentId = TxtDocID.Text;


                string partNumber = TxtPartNo.Text;


                int numberOfPages = Convert.ToInt32(TxtNoPages.Text);


                string xmlDocPath = UploadXml.PostedFile.FileName;


                string pdfPath = UploadPdf.PostedFile.FileName;



                //load xml data
                //load enquiry xml
                //FileStream xfs = new FileStream(xmlDocPath, FileMode.Open, FileAccess.Read);
                //XmlTextReader xtr = new XmlTextReader(xmlDocPath);
                XmlTextReader xtr = new XmlTextReader(UploadXml.PostedFile.InputStream);
                xtr.WhitespaceHandling = WhitespaceHandling.None;
                XmlDocument xeroxXml = new XmlDocument();
                xeroxXml.Load(xtr);
                //get language
                //find layout node
                XmlNode xNode = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout");
                //get attribute
                XmlNode langNode = xNode.Attributes["wave2_name"];
                //find lang ID from languages model
                Language lang = xctEntities.Languages.Single(l => l.LanguageName == langNode.InnerText);
                //find display name
                XmlNode xNodedDisplay = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/display_name");
                string fullDisplayName = xNodedDisplay.InnerText;


                //create new template 
                Template localTemplate = new Template(); //indentity on
                localTemplate.CollorCorrection = "Commercial";
                localTemplate.DateCreated = DateTime.Now;
                localTemplate.DateModified = DateTime.Now;
                localTemplate.Description = fullDisplayName;
                localTemplate.Detail = fullDisplayName;
                localTemplate.Duplex = true;
                localTemplate.IsDeleted = false;
                localTemplate.LanguageID = lang.LanguageID;
                localTemplate.Name = fullDisplayName + " - " + lang.LanguageName;
                if (ChkLandscape.Checked)
                {
                    localTemplate.PaperSize = "A-size (11*8.5)";
                    localTemplate.PageSize = "a4landscape";
                }
                else
                {
                    localTemplate.PaperSize = "A-size (8.5*11)";
                    localTemplate.PageSize = "a4";
                }
                localTemplate.PaperOption = "100# Elite Gloss Text";
               
                localTemplate.PrintMode = "High-Quality";
                localTemplate.Quantity = 0;
                localTemplate.RecycledPaper = false;

                xctEntities.Entry(localTemplate).State = System.Data.EntityState.Added;
                xctEntities.SaveChanges();
                //fetch last template id
                var templateId = localTemplate.TemplateID;

                //create partner brand template
                TemplatePartnerBranded localTpb = new TemplatePartnerBranded(); //indentity on
                localTpb.TemplateID = templateId;
                localTpb.PartnerBranded = false;
                localTpb.PartNumber = partNumber;
                xctEntities.Entry(localTpb).State = System.Data.EntityState.Added;
                xctEntities.SaveChanges();

                whereDidIget = "1";

                numberOfPages = numberOfPages + 1;
                for (int i = 1; i < numberOfPages; i++)
                {
                    CollateralCreatorAdminWeb.Models.Page localPage = new CollateralCreatorAdminWeb.Models.Page();
                    localPage.TemplateID = templateId;
                    if (ChkLandscape.Checked)
                    {
                        localPage.Height = 595;
                        localPage.Width = 842;
                    }
                    else
                    {
                        localPage.Height = 842;
                        localPage.Width = 595;
                    }
                    localPage.Rotation = 0;
                    localPage.PartnerBranded = false;
                    localPage.PageNumber = i;

                    xctEntities.Entry(localPage).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();

                    pageId = localPage.PageID;
                    pageIds.Add(pageId);
                }

                whereDidIget = "2";

                if (BlockControl1.Visible)
                {
                    //find correct nodes
                    XmlNode xNode1 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='1']"); //main text
                    CreateTextArea(pageIds[BlockControl1.PageNo - 1], BlockControl1.Height, BlockControl1.Width, BlockControl1.XCoord,
                        BlockControl1.YCoord, BlockControl1.FontName,
                        BlockControl1.FontSize, BlockControl1.FontColour, BlockControl1.Rotation, templateId, xNode1);
                }

                if (BlockControl2.Visible)
                {
                    //find correct nodes
                    XmlNode xNode2 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='2']"); //main text
                    CreateTextArea(pageIds[BlockControl2.PageNo - 1], BlockControl2.Height, BlockControl2.Width, BlockControl2.XCoord,
                        BlockControl2.YCoord, BlockControl2.FontName, BlockControl2.FontSize, BlockControl2.FontColour, BlockControl2.Rotation, templateId, xNode2);
                }

                if (BlockControl3.Visible)
                {
                    //find correct nodes
                    XmlNode xNode3 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='3']"); //main text
                    CreateTextArea(pageIds[BlockControl3.PageNo - 1], BlockControl3.Height, BlockControl3.Width, BlockControl3.XCoord,
                        BlockControl3.YCoord, BlockControl3.FontName, BlockControl3.FontSize, BlockControl3.FontColour, BlockControl3.Rotation, templateId, xNode3);
                }

                if (BlockControl4.Visible)
                {
                    //find correct nodes
                    XmlNode xNode4 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='4']"); //main text
                    CreateTextArea(pageIds[BlockControl4.PageNo - 1], BlockControl4.Height, BlockControl4.Width, BlockControl4.XCoord,
                        BlockControl4.YCoord, BlockControl4.FontName, BlockControl4.FontSize, BlockControl4.FontColour, BlockControl4.Rotation, templateId, xNode4);
                }

                if (BlockControl5.Visible)
                {
                    //find correct nodes
                    XmlNode xNode5 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='5']"); //main text
                    CreateTextArea(pageIds[BlockControl5.PageNo - 1], BlockControl5.Height, BlockControl5.Width, BlockControl5.XCoord,
                        BlockControl5.YCoord, BlockControl5.FontName, BlockControl5.FontSize, BlockControl5.FontColour, BlockControl5.Rotation, templateId, xNode5);
                }

                if (BlockControl6.Visible)
                {
                    //find correct nodes
                    XmlNode xNode6 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='6']"); //main text
                    CreateTextArea(pageIds[BlockControl6.PageNo - 1], BlockControl6.Height, BlockControl6.Width, BlockControl6.XCoord,
                        BlockControl6.YCoord, BlockControl6.FontName, BlockControl6.FontSize, BlockControl6.FontColour, BlockControl6.Rotation, templateId, xNode6);
                }

                if (BlockControl7.Visible)
                {
                    //find correct nodes
                    XmlNode xNode7 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='7']"); //main text
                    CreateTextArea(pageIds[BlockControl7.PageNo - 1], BlockControl7.Height, BlockControl7.Width, BlockControl7.XCoord,
                        BlockControl7.YCoord, BlockControl7.FontName, BlockControl7.FontSize, BlockControl7.FontColour, BlockControl7.Rotation, templateId, xNode7);
                }


                if (BlockControl8.Visible)
                {
                    //find correct nodes
                    XmlNode xNode8 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='8']"); //main text
                    CreateTextArea(pageIds[BlockControl8.PageNo - 1], BlockControl8.Height, BlockControl8.Width, BlockControl8.XCoord,
                        BlockControl8.YCoord, BlockControl8.FontName, BlockControl8.FontSize, BlockControl8.FontColour, BlockControl8.Rotation, templateId, xNode8);
                }

                if (BlockControl9.Visible)
                {
                    //find correct nodes
                    XmlNode xNode9 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='9']"); //main text
                    CreateTextArea(pageIds[BlockControl9.PageNo - 1], BlockControl9.Height, BlockControl9.Width, BlockControl9.XCoord,
                        BlockControl9.YCoord, BlockControl9.FontName, BlockControl9.FontSize, BlockControl9.FontColour, BlockControl9.Rotation, templateId, xNode9);
                }

                if (BlockControl10.Visible)
                {
                    //find correct nodes
                    XmlNode xNode10 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='10']"); //main text
                    CreateTextArea(pageIds[BlockControl10.PageNo - 1], BlockControl10.Height, BlockControl10.Width, BlockControl10.XCoord,
                        BlockControl10.YCoord, BlockControl10.FontName, BlockControl10.FontSize, BlockControl10.FontColour, BlockControl10.Rotation, templateId, xNode10);
                }

                if (BlockControl11.Visible)
                {
                    //find correct nodes
                    XmlNode xNode11 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='11']"); //main text
                    CreateTextArea(pageIds[BlockControl11.PageNo - 1], BlockControl11.Height, BlockControl11.Width, BlockControl11.XCoord,
                        BlockControl11.YCoord, BlockControl11.FontName, BlockControl11.FontSize, BlockControl11.FontColour, BlockControl11.Rotation, templateId, xNode11);
                }

                if (BlockControl12.Visible)
                {
                    //find correct nodes
                    XmlNode xNode12 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='12']"); //main text
                    CreateTextArea(pageIds[BlockControl12.PageNo - 1], BlockControl12.Height, BlockControl12.Width, BlockControl12.XCoord,
                        BlockControl12.YCoord, BlockControl12.FontName, BlockControl12.FontSize, BlockControl12.FontColour, BlockControl12.Rotation, templateId, xNode12);
                }

                if (BlockControl13.Visible)
                {
                    //find correct nodes
                    XmlNode xNode13 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='13']"); //main text
                    CreateTextArea(pageIds[BlockControl13.PageNo - 1], BlockControl13.Height, BlockControl13.Width, BlockControl13.XCoord,
                        BlockControl13.YCoord, BlockControl13.FontName, BlockControl13.FontSize, BlockControl13.FontColour, BlockControl13.Rotation, templateId, xNode13);
                }

                if (BlockControl14.Visible)
                {
                    //find correct nodes
                    XmlNode xNode14 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='14']"); //main text
                    CreateTextArea(pageIds[BlockControl14.PageNo - 1], BlockControl14.Height, BlockControl14.Width, BlockControl14.XCoord,
                        BlockControl14.YCoord, BlockControl14.FontName, BlockControl14.FontSize, BlockControl14.FontColour, BlockControl14.Rotation, templateId, xNode14);
                }

                if (BlockControl15.Visible)
                {
                    //find correct nodes
                    XmlNode xNode15 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='15']"); //main text
                    CreateTextArea(pageIds[BlockControl15.PageNo - 1], BlockControl15.Height, BlockControl15.Width, BlockControl15.XCoord,
                        BlockControl15.YCoord, BlockControl15.FontName, BlockControl15.FontSize, BlockControl15.FontColour, BlockControl15.Rotation, templateId, xNode15);
                }

                if (BlockControl16.Visible)
                {
                    //find correct nodes
                    XmlNode xNode16 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='16']"); //main text
                    CreateTextArea(pageIds[BlockControl16.PageNo - 1], BlockControl16.Height, BlockControl16.Width, BlockControl16.XCoord,
                        BlockControl16.YCoord, BlockControl16.FontName, BlockControl16.FontSize, BlockControl16.FontColour, BlockControl16.Rotation, templateId, xNode16);
                }

                if (BlockControl17.Visible)
                {
                    //find correct nodes
                    XmlNode xNode17 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='17']"); //main text
                    CreateTextArea(pageIds[BlockControl17.PageNo - 1], BlockControl17.Height, BlockControl17.Width, BlockControl17.XCoord,
                        BlockControl17.YCoord, BlockControl17.FontName, BlockControl17.FontSize, BlockControl17.FontColour, BlockControl17.Rotation, templateId, xNode17);
                }

                if (BlockControl18.Visible)
                {
                    //find correct nodes
                    XmlNode xNode18 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='18']"); //main text
                    CreateTextArea(pageIds[BlockControl18.PageNo - 1], BlockControl18.Height, BlockControl18.Width, BlockControl18.XCoord,
                        BlockControl18.YCoord, BlockControl18.FontName, BlockControl18.FontSize, BlockControl18.FontColour, BlockControl18.Rotation, templateId, xNode18);
                }

                if (BlockControl19.Visible)
                {
                    //find correct nodes
                    XmlNode xNode19 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='19']"); //main text
                    CreateTextArea(pageIds[BlockControl19.PageNo - 1], BlockControl19.Height, BlockControl19.Width, BlockControl19.XCoord,
                        BlockControl19.YCoord, BlockControl19.FontName, BlockControl19.FontSize, BlockControl19.FontColour, BlockControl19.Rotation, templateId, xNode19);
                }

                if (BlockControl20.Visible)
                {
                    //find correct nodes
                    XmlNode xNode20 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='20']"); //main text
                    CreateTextArea(pageIds[BlockControl20.PageNo - 1], BlockControl20.Height, BlockControl20.Width, BlockControl20.XCoord,
                        BlockControl20.YCoord, BlockControl20.FontName, BlockControl20.FontSize, BlockControl20.FontColour, BlockControl20.Rotation, templateId, xNode20);
                }
                if (BlockControl21.Visible)
                {
                    //find correct nodes
                    XmlNode xNode21 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='21']"); //main text
                    CreateTextArea(pageIds[BlockControl21.PageNo - 1], BlockControl21.Height, BlockControl21.Width, BlockControl21.XCoord,
                        BlockControl21.YCoord, BlockControl21.FontName, BlockControl21.FontSize, BlockControl21.FontColour, BlockControl21.Rotation, templateId, xNode21);
                }
                if (BlockControl22.Visible)
                {
                    //find correct nodes
                    XmlNode xNode22 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='22']"); //main text
                    CreateTextArea(pageIds[BlockControl22.PageNo - 1], BlockControl22.Height, BlockControl22.Width, BlockControl22.XCoord,
                        BlockControl22.YCoord, BlockControl22.FontName, BlockControl22.FontSize, BlockControl22.FontColour, BlockControl22.Rotation, templateId, xNode22);
                }
                if (BlockControl23.Visible)
                {
                    //find correct nodes
                    XmlNode xNode23 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='23']"); //main text
                    CreateTextArea(pageIds[BlockControl23.PageNo - 1], BlockControl23.Height, BlockControl23.Width, BlockControl23.XCoord,
                        BlockControl23.YCoord, BlockControl23.FontName, BlockControl23.FontSize, BlockControl23.FontColour, BlockControl23.Rotation, templateId, xNode23);
                }
                if (BlockControl24.Visible)
                {
                    //find correct nodes
                    XmlNode xNode24 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='24']"); //main text
                    CreateTextArea(pageIds[BlockControl24.PageNo - 1], BlockControl24.Height, BlockControl24.Width, BlockControl24.XCoord,
                        BlockControl20.YCoord, BlockControl24.FontName, BlockControl24.FontSize, BlockControl24.FontColour, BlockControl24.Rotation, templateId, xNode24);
                }
                if (BlockControl25.Visible)
                {
                    //find correct nodes
                    XmlNode xNode25 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='25']"); //main text
                    CreateTextArea(pageIds[BlockControl25.PageNo - 1], BlockControl25.Height, BlockControl25.Width, BlockControl25.XCoord,
                        BlockControl25.YCoord, BlockControl25.FontName, BlockControl25.FontSize, BlockControl25.FontColour, BlockControl25.Rotation, templateId, xNode25);
                }
                if (BlockControl26.Visible)
                {
                    //find correct nodes
                    XmlNode xNode26 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='26']"); //main text
                    CreateTextArea(pageIds[BlockControl26.PageNo - 1], BlockControl26.Height, BlockControl26.Width, BlockControl26.XCoord,
                        BlockControl26.YCoord, BlockControl26.FontName, BlockControl26.FontSize, BlockControl26.FontColour, BlockControl26.Rotation, templateId, xNode26);
                }
                if (BlockControl27.Visible)
                {
                    //find correct nodes
                    XmlNode xNode27 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='27']"); //main text
                    CreateTextArea(pageIds[BlockControl27.PageNo - 1], BlockControl27.Height, BlockControl27.Width, BlockControl27.XCoord,
                        BlockControl27.YCoord, BlockControl27.FontName, BlockControl27.FontSize, BlockControl27.FontColour, BlockControl27.Rotation, templateId, xNode27);
                }
                if (BlockControl28.Visible)
                {
                    //find correct nodes
                    XmlNode xNode28 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='28']"); //main text
                    CreateTextArea(pageIds[BlockControl28.PageNo - 1], BlockControl28.Height, BlockControl28.Width, BlockControl28.XCoord,
                        BlockControl28.YCoord, BlockControl28.FontName, BlockControl28.FontSize, BlockControl28.FontColour, BlockControl28.Rotation, templateId, xNode28);
                }
                if (BlockControl29.Visible)
                {
                    //find correct nodes
                    XmlNode xNode29 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='29']"); //main text
                    CreateTextArea(pageIds[BlockControl29.PageNo - 1], BlockControl29.Height, BlockControl29.Width, BlockControl29.XCoord,
                        BlockControl29.YCoord, BlockControl29.FontName, BlockControl29.FontSize, BlockControl29.FontColour, BlockControl29.Rotation, templateId, xNode29);
                }
                if (BlockControl30.Visible)
                {
                    //find correct nodes
                    XmlNode xNode30 = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Text' and @item='30']"); //main text
                    CreateTextArea(pageIds[BlockControl30.PageNo - 1], BlockControl30.Height, BlockControl30.Width, BlockControl30.XCoord,
                        BlockControl30.YCoord, BlockControl30.FontName, BlockControl30.FontSize, BlockControl30.FontColour, BlockControl30.Rotation, templateId, xNode30);
                }


                whereDidIget = "3";

                if (ChkUseImage1.Checked || TxtImg1X.Text.Length > 0)
                {
                    #region Code
                    XmlNode xNodeImage = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Image' and @item='1']"); //image 1

                    //customisable areas -- image -- node 3
                    CustomizableArea customAreaLogo = new CustomizableArea();
                    customAreaLogo.Height = Convert.ToInt32(TxtImgHeight1.Text);
                    customAreaLogo.Rotation = Convert.ToInt32(TxtImageRotation1.Text);
                    customAreaLogo.Width = Convert.ToInt32(TxtImgWidth1.Text);
                    customAreaLogo.XPos = Convert.ToInt32(TxtImg1X.Text);
                    customAreaLogo.YPos = Convert.ToInt32(TxtImg1Y.Text);
                    XmlNode node3FieldName = xNodeImage.SelectSingleNode("field_name");
                    XmlNode node3HelpText = xNodeImage.SelectSingleNode("help_text");
                    customAreaLogo.Name = node3FieldName.InnerText + " - " + node3HelpText.InnerText;
                    customAreaLogo.PageID = pageIds[Convert.ToInt32(TxtImagePageNo1.Text) - 1];
                    xctEntities.Entry(customAreaLogo).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();

                    XmlNode defaultImageNode = xNodeImage.SelectSingleNode("data");
                    string canEdit = defaultImageNode.Attributes["can_edit"].InnerText;
                    //get default image
                    string fileName1 = "";
                    if (canEdit.Equals("false"))
                    {
                        //upload the file
                        string[] array = defaultImageNode.ChildNodes[0].InnerText.Split('.');
                        fileName1 = "C:\\Xerox_Collateral\\ClientResources\\" + array[0] + " .jpg";
                        imagePartnerBrand = false;
                    }
                    else
                    {
                        //upload blank
                        fileName1 = @"C:\Xerox_Collateral\ClientResources\PartnerLogo.jpg";
                        imagePartnerBrand = true;
                    }
                    FileStream fs = new FileStream(fileName1, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);

                    ImageArea imageArea = new ImageArea();
                    imageArea.AreaID = customAreaLogo.AreaID;
                    imageArea.Width = Convert.ToInt32(TxtImgWidth1.Text);
                    imageArea.Height = Convert.ToInt32(TxtImgHeight1.Text);
                    imageArea.PartnerBranded = imagePartnerBrand;
                    imageArea.Image = bytes;

                    fs.Close();

                    xctEntities.Entry(imageArea).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();
                    #endregion
                }

                if (ChkUseImage2.Checked || TxtImg2X.Text.Length > 0)
                {
                    #region Code
                    XmlNode xNodeImage = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Image' and @item='2']"); //image 1

                    //customisable areas -- image -- node 2
                    CustomizableArea customAreaLogo = new CustomizableArea();
                    customAreaLogo.Height = Convert.ToInt32(TxtImgHeight2.Text);
                    customAreaLogo.Rotation = Convert.ToInt32(TxtImageRotation2.Text);
                    customAreaLogo.Width = Convert.ToInt32(TxtImgWidth2.Text);
                    customAreaLogo.XPos = Convert.ToInt32(TxtImg2X.Text);
                    customAreaLogo.YPos = Convert.ToInt32(TxtImg2Y.Text);
                    XmlNode node2FieldName = xNodeImage.SelectSingleNode("field_name");
                    XmlNode node2HelpText = xNodeImage.SelectSingleNode("help_text");
                    customAreaLogo.Name = node2FieldName.InnerText + " - " + node2HelpText.InnerText;
                    customAreaLogo.PageID = pageIds[Convert.ToInt32(TxtImagePageNo2.Text) - 1];
                    xctEntities.Entry(customAreaLogo).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();

                    XmlNode defaultImageNode = xNodeImage.SelectSingleNode("data");
                    string canEdit = defaultImageNode.Attributes["can_edit"].InnerText;
                    //get default image
                    string fileName2 = "";
                    if (canEdit.Equals("false"))
                    {
                        //upload the file
                        string[] array = defaultImageNode.ChildNodes[0].InnerText.Split('.');
                        fileName2 = "C:\\Xerox_Collateral\\ClientResources\\" + array[0] + " .jpg";


                        FileInfo f = new FileInfo(fileName2);
                        imagePartnerBrand = false;

                    }
                    else
                    {
                        //upload blank
                        fileName2 = @"C:\Xerox_Collateral\ClientResources\PartnerLogo.jpg";
                        imagePartnerBrand = true;
                    }
                    //get default image
                    //FileStream fs = new FileStream(fileName2.Replace(" ", ""), FileMode.Open, FileAccess.Read);
                    FileStream fs = new FileStream(fileName2, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);

                    ImageArea imageArea = new ImageArea();
                    imageArea.AreaID = customAreaLogo.AreaID;
                    imageArea.Width = Convert.ToInt32(TxtImgWidth2.Text);
                    imageArea.Height = Convert.ToInt32(TxtImgHeight2.Text);
                    imageArea.PartnerBranded = imagePartnerBrand;
                    imageArea.Image = bytes;

                    
                    fs.Close();

                    xctEntities.Entry(imageArea).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();
                    #endregion
                }


                if (ChkUseImage3.Checked || TxtImg3X.Text.Length > 0)
                {
                    #region Code
                    XmlNode xNodeImage = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Image' and @item='3']"); //image 1

                    //customisable areas -- image -- node 3
                    CustomizableArea customAreaLogo = new CustomizableArea();
                    customAreaLogo.Height = Convert.ToInt32(TxtImgHeight3.Text);
                    customAreaLogo.Rotation = Convert.ToInt32(TxtImageRotation3.Text);
                    customAreaLogo.Width = Convert.ToInt32(TxtImgWidth3.Text);
                    customAreaLogo.XPos = Convert.ToInt32(TxtImg3X.Text);
                    customAreaLogo.YPos = Convert.ToInt32(TxtImg3Y.Text);
                    XmlNode node3FieldName = xNodeImage.SelectSingleNode("field_name");
                    XmlNode node3HelpText = xNodeImage.SelectSingleNode("help_text");
                    customAreaLogo.Name = node3FieldName.InnerText + " - " + node3HelpText.InnerText;
                    customAreaLogo.PageID = pageIds[Convert.ToInt32(TxtImagePageNo3.Text) - 1];
                    xctEntities.Entry(customAreaLogo).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();

                    XmlNode defaultImageNode = xNodeImage.SelectSingleNode("data");
                    string canEdit = defaultImageNode.Attributes["can_edit"].InnerText;
                    //get default image
                    string fileName3 = "";
                    if (canEdit.Equals("false"))
                    {
                        //upload the file
                        string[] array = defaultImageNode.ChildNodes[0].InnerText.Split('.');
                        fileName3 = "C:\\Xerox_Collateral\\ClientResources\\" + array[0] + " .jpg";
                        imagePartnerBrand = false;
                    }
                    else
                    {
                        //upload blank
                        fileName3 = @"C:\Xerox_Collateral\ClientResources\PartnerLogo.jpg";
                        imagePartnerBrand = true;
                    }
                    //get default image
                    FileStream fs = new FileStream(fileName3, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);

                    ImageArea imageArea = new ImageArea();
                    imageArea.AreaID = customAreaLogo.AreaID;
                    imageArea.Width = Convert.ToInt32(TxtImgWidth3.Text);
                    imageArea.Height = Convert.ToInt32(TxtImgHeight3.Text);
                    imageArea.PartnerBranded = false;
                    imageArea.Image = bytes;

                    fs.Close();

                    xctEntities.Entry(imageArea).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();
                    #endregion
                }

                if (ChkUseImage4.Checked || TxtImg4X.Text.Length > 0)
                {
                    #region Code
                    XmlNode xNodeImage = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Image' and @item='4']"); //image 1

                    //customisable areas -- image -- node 4
                    CustomizableArea customAreaLogo = new CustomizableArea();
                    customAreaLogo.Height = Convert.ToInt32(TxtImgHeight4.Text);
                    customAreaLogo.Rotation = Convert.ToInt32(TxtImageRotation4.Text);
                    customAreaLogo.Width = Convert.ToInt32(TxtImgWidth4.Text);
                    customAreaLogo.XPos = Convert.ToInt32(TxtImg4X.Text);
                    customAreaLogo.YPos = Convert.ToInt32(TxtImg4Y.Text);
                    XmlNode node4FieldName = xNodeImage.SelectSingleNode("field_name");
                    XmlNode node4HelpText = xNodeImage.SelectSingleNode("help_text");
                    customAreaLogo.Name = node4FieldName.InnerText + " - " + node4HelpText.InnerText;
                    customAreaLogo.PageID = pageIds[Convert.ToInt32(TxtImagePageNo4.Text) - 1];
                    xctEntities.Entry(customAreaLogo).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();

                    XmlNode defaultImageNode = xNodeImage.SelectSingleNode("data");
                    string canEdit = defaultImageNode.Attributes["can_edit"].InnerText;
                    //get default image
                    string fileName4 = "";
                    if (canEdit.Equals("false"))
                    {
                        //upload the file
                        string[] array = defaultImageNode.ChildNodes[0].InnerText.Split('.');
                        fileName4 = "C:\\Xerox_Collateral\\ClientResources\\" + array[0] + " .jpg";
                        imagePartnerBrand = false;
                    }
                    else
                    {
                        //upload blank
                        fileName4 = @"C:\Xerox_Collateral\ClientResources\PartnerLogo.jpg";
                        imagePartnerBrand = true;
                    }
                    //get default image
                    FileStream fs = new FileStream(fileName4, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);

                    ImageArea imageArea = new ImageArea();
                    imageArea.AreaID = customAreaLogo.AreaID;
                    imageArea.Width = Convert.ToInt32(TxtImgWidth4.Text);
                    imageArea.Height = Convert.ToInt32(TxtImgHeight4.Text);
                    imageArea.PartnerBranded = false;
                    imageArea.Image = bytes;

                    fs.Close();

                    xctEntities.Entry(imageArea).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();
                    #endregion
                }


                if (ChkUseImage5.Checked || TxtImg5X.Text.Length > 0)
                {
                    #region Code
                    XmlNode xNodeImage = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Image' and @item='5']"); //image 1

                    //customisable areas -- image -- node 4
                    CustomizableArea customAreaLogo = new CustomizableArea();
                    customAreaLogo.Height = Convert.ToInt32(TxtImgHeight5.Text);
                    customAreaLogo.Rotation = Convert.ToInt32(TxtImageRotation5.Text);
                    customAreaLogo.Width = Convert.ToInt32(TxtImgWidth5.Text);
                    customAreaLogo.XPos = Convert.ToInt32(TxtImg5X.Text);
                    customAreaLogo.YPos = Convert.ToInt32(TxtImg5Y.Text);
                    XmlNode node5FieldName = xNodeImage.SelectSingleNode("field_name");
                    XmlNode node5HelpText = xNodeImage.SelectSingleNode("help_text");
                    customAreaLogo.Name = node5FieldName.InnerText + " - " + node5HelpText.InnerText;
                    customAreaLogo.PageID = pageIds[Convert.ToInt32(TxtImagePageNo5.Text) - 1];
                    xctEntities.Entry(customAreaLogo).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();

                    XmlNode defaultImageNode = xNodeImage.SelectSingleNode("data");
                    string canEdit = defaultImageNode.Attributes["can_edit"].InnerText;
                    //get default image
                    string fileName5 = "";
                    if (canEdit.Equals("false"))
                    {
                        //upload the file
                        string[] array = defaultImageNode.ChildNodes[0].InnerText.Split('.');
                        fileName5 = "C:\\Xerox_Collateral\\ClientResources\\" + array[0] + " .jpg";
                        imagePartnerBrand = false;
                    }
                    else
                    {
                        //upload blank
                        fileName5 = @"C:\Xerox_Collateral\ClientResources\PartnerLogo.jpg";
                        imagePartnerBrand = true;
                    }
                    //get default image
                    FileStream fs = new FileStream(fileName5, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);

                    ImageArea imageArea = new ImageArea();
                    imageArea.AreaID = customAreaLogo.AreaID;
                    imageArea.Width = Convert.ToInt32(TxtImgWidth5.Text);
                    imageArea.Height = Convert.ToInt32(TxtImgHeight5.Text);
                    imageArea.PartnerBranded = false;
                    imageArea.Image = bytes;

                    fs.Close();

                    xctEntities.Entry(imageArea).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();
                    #endregion
                }


                if (ChkUseImage6.Checked || TxtImg6X.Text.Length > 0)
                {
                    #region Code
                    XmlNode xNodeImage = xeroxXml.SelectSingleNode("/CustomisedDocument/Layout/Group/Article/Field[@type='Image' and @item='6']"); //image 1

                    //customisable areas -- image -- node 6
                    CustomizableArea customAreaLogo = new CustomizableArea();
                    customAreaLogo.Height = Convert.ToInt32(TxtImgHeight6.Text);
                    customAreaLogo.Rotation = Convert.ToInt32(TxtImageRotation6.Text);
                    customAreaLogo.Width = Convert.ToInt32(TxtImgWidth6.Text);
                    customAreaLogo.XPos = Convert.ToInt32(TxtImg6X.Text);
                    customAreaLogo.YPos = Convert.ToInt32(TxtImg6Y.Text);
                    XmlNode node6FieldName = xNodeImage.SelectSingleNode("field_name");
                    XmlNode node6HelpText = xNodeImage.SelectSingleNode("help_text");
                    customAreaLogo.Name = node6FieldName.InnerText + " - " + node6HelpText.InnerText;
                    customAreaLogo.PageID = pageIds[Convert.ToInt32(TxtImagePageNo6.Text) - 1];
                    xctEntities.Entry(customAreaLogo).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();

                    XmlNode defaultImageNode = xNodeImage.SelectSingleNode("data");
                    string canEdit = defaultImageNode.Attributes["can_edit"].InnerText;
                    //get default image
                    string fileName6 = "";
                    if (canEdit.Equals("false"))
                    {
                        //upload the file
                        string[] array = defaultImageNode.ChildNodes[0].InnerText.Split('.');
                        fileName6 = "C:\\Xerox_Collateral\\ClientResources\\" + array[0] + " .jpg";
                        imagePartnerBrand = false;
                    }
                    else
                    {
                        //upload blank
                        fileName6 = @"C:\Xerox_Collateral\ClientResources\PartnerLogo.jpg";
                        imagePartnerBrand = true;
                    }
                    //get default image
                    FileStream fs = new FileStream(fileName6, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);

                    ImageArea imageArea = new ImageArea();
                    imageArea.AreaID = customAreaLogo.AreaID;
                    imageArea.Width = Convert.ToInt32(TxtImgWidth6.Text);
                    imageArea.Height = Convert.ToInt32(TxtImgHeight6.Text);
                    imageArea.PartnerBranded = false;
                    imageArea.Image = bytes;

                    fs.Close();

                    xctEntities.Entry(imageArea).State = System.Data.EntityState.Added;
                    xctEntities.SaveChanges();
                    #endregion
                }

                whereDidIget = "4";
                //finally add external collateral ID
                ExternalCollateral extCol = new ExternalCollateral();
                extCol.ExCollateralID = Convert.ToInt32(documentId);
                extCol.LanguageCode = lang.LanguageCode;
                extCol.TemplateID = templateId;
                xctEntities.Entry(extCol).State = System.Data.EntityState.Added;
                xctEntities.SaveChanges();

                //rename the pdf file with the template ID
                //template_xx
                //System.IO.File.Copy(pdfPath, @"c:\Xerox_Collateral\FinalPdfTemplates\template_" + templateId.ToString() + ".pdf");
                UploadPdf.PostedFile.SaveAs(@"c:\Xerox_Collateral\FinalPdfTemplates\template_" + templateId.ToString() + ".pdf");

                whereDidIget = "5";
                LblMessage.Text = "Upload complete";
                LblError.Text = "";
            }
            catch(Exception error)
            {
                LblMessage.Text = "";
                LblError.Text = "Error in upload: " + whereDidIget + "<br/>" + error.Message + "<br/>" + error.StackTrace.ToString();
            }
            #endregion
        }

        private void CreateTextArea(int pageId, int height, int width, int x, int y, string fontName, int fontSize, int colour, int rotation, int templateId, XmlNode xeroxNode)
        {
            #region Code
            try
            {
                int charsPerLine = 0;
                int numLines = 0;
                XmlNode node1HelpText = xeroxNode.SelectSingleNode("help_text");
                XmlNode node1Data = xeroxNode.SelectSingleNode("data");

                //customisable areas -- main header text
                CustomizableArea customHeaderText = new CustomizableArea();
                customHeaderText.Height = height;
                customHeaderText.Rotation = rotation;
                customHeaderText.Width = width;
                customHeaderText.XPos = x;
                customHeaderText.YPos = y;
                XmlNode node1FieldName = xeroxNode.SelectSingleNode("field_name");
                if (node1HelpText.InnerText.Length > 0)
                {
                    customHeaderText.Name = node1FieldName.InnerText + " - " + node1HelpText.InnerText;
                }
                else
                {
                    customHeaderText.Name = node1FieldName.InnerText;
                }
                customHeaderText.PageID = pageId;
                xctEntities.Entry(customHeaderText).State = System.Data.EntityState.Added;
                xctEntities.SaveChanges();

                int result = 0;
                int iResultRound = 0;
                foreach (char c in node1Data.InnerText)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        result++;
                    }
                }
                if (result == 0)
                {
                    iResultRound = 50;
                }
                else
                {
                    iResultRound = ((int)Math.Round((result + 50) / 10.0)) * 20;
                }

                numLines = node1Data.InnerText.Split('\n').Length;
                if (node1Data.InnerText.Length > 150)
                {
                    numLines = 2;
                }
                charsPerLine = iResultRound / numLines;

                TextArea textAreaHeader = new TextArea();
                textAreaHeader.AreaID = customHeaderText.AreaID;
                textAreaHeader.FullyEditable = true;
                textAreaHeader.CharsPerLine = charsPerLine;
                textAreaHeader.Lines = numLines;
                textAreaHeader.Align = "LEFT";
                if (fontSize > 18)
                {
                    textAreaHeader.LineSpacing = 12;
                }
                else
                {
                    textAreaHeader.LineSpacing = 7;
                }
                textAreaHeader.Text = node1Data.InnerText;

                //if (node1Data.InnerText.Length > 80)
                //{
                //    const Int32 MAX_WIDTH = 80;

                //    int offset = 0;
                //    string text = Regex.Replace(node1Data.InnerText, @"\s{2,}", " ");
                //    List<string> lines = new List<string>();
                //    string myText = "";
                //    while (offset < text.Length)
                //    {
                //        int index = text.LastIndexOf(" ",
                //                         Math.Min(text.Length, offset + MAX_WIDTH));
                //        string line = text.Substring(offset,
                //            (index - offset <= 0 ? text.Length : index) - offset);
                //        offset += line.Length + 1;
                //        lines.Add(line);
                //        myText += line;
                //    }
                //    //textAreaHeader.Text = Regex.Replace(node1Data.InnerText, "(.{" + 150 + "})", "$1" + Environment.NewLine);

                //    textAreaHeader.Text = myText;
                //}
                //else
                //{
                //    textAreaHeader.Text = node1Data.InnerText;
                //}
                xctEntities.Entry(textAreaHeader).State = System.Data.EntityState.Added;
                xctEntities.SaveChanges();

                Font fontHeader = new Font();
                fontHeader.FontTypeID = 4;
                fontHeader.Size = fontSize;
                fontHeader.Bold = true;
                fontHeader.Italic = false;
                fontHeader.Underline = false;
                fontHeader.Strikethrough = false;
                fontHeader.Color = colour;
                fontHeader.Name = fontName;
                xctEntities.Entry(fontHeader).State = System.Data.EntityState.Added;
                xctEntities.SaveChanges();

                FontUsage fontUsageHeader = new FontUsage();
                fontUsageHeader.FontID = fontHeader.FontID;
                fontUsageHeader.TextAreaID = textAreaHeader.TextAreaID;
                fontUsageHeader.TemplateID = templateId;
                fontUsageHeader.PartnerBranded = false;
                xctEntities.Entry(fontUsageHeader).State = System.Data.EntityState.Added;
                xctEntities.SaveChanges();
            }
            catch { }

            #endregion
        }

        protected void DrpTextBlocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Code
            //build text block user control list
            int noOfBlocks = Convert.ToInt32(DrpTextBlocks.SelectedValue);

            BlockControl1.Visible = false;
            BlockControl2.Visible = false;
            BlockControl3.Visible = false;
            BlockControl4.Visible = false;
            BlockControl5.Visible = false;
            BlockControl6.Visible = false;
            BlockControl7.Visible = false;
            BlockControl8.Visible = false;
            BlockControl9.Visible = false;
            BlockControl10.Visible = false;
            BlockControl11.Visible = false;
            BlockControl12.Visible = false;
            BlockControl13.Visible = false;
            BlockControl14.Visible = false;

            BlockControl15.Visible = false;
            BlockControl16.Visible = false;
            BlockControl17.Visible = false;
            BlockControl18.Visible = false;

            BlockControl19.Visible = false;
            BlockControl20.Visible = false;
            BlockControl21.Visible = false;
            BlockControl22.Visible = false;

            switch (noOfBlocks)
            {
                case 1:
                    BlockControl1.Visible = true;
                    break;
                case 2:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    break;
                case 3:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    break;
                case 4:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    break;
                case 5:
                     BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    break;
                case 6:
                       BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    break;
                case 7:
                        BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    break;
                case 8:
                      BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    break;
                case 9:
                     BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    break;
                case 10:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    break;
                case 11:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    break;
                case 12:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    break;
                case 13:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    break;
                case 14:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    break;
                case 15:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    break;
                case 16:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    break;
                case 17:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    break;
                case 18:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    break;
                case 19:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    break;
                case 20:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    break;
                case 21:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    break;
                case 22:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    break;
                case 23:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    break;
                case 24:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    BlockControl24.Visible = true;
                    break;
                case 25:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    BlockControl24.Visible = true;
                    BlockControl25.Visible = true;
                    break;
                case 26:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    BlockControl24.Visible = true;
                    BlockControl25.Visible = true;
                    BlockControl26.Visible = true;
                    break;
                case 27:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    BlockControl24.Visible = true;
                    BlockControl25.Visible = true;
                    BlockControl26.Visible = true;
                    BlockControl27.Visible = true;
                    break;
                case 28:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    BlockControl24.Visible = true;
                    BlockControl25.Visible = true;
                    BlockControl26.Visible = true;
                    BlockControl27.Visible = true;
                    BlockControl28.Visible = true;
                    break;
                case 29:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    BlockControl24.Visible = true;
                    BlockControl25.Visible = true;
                    BlockControl26.Visible = true;
                    BlockControl27.Visible = true;
                    BlockControl28.Visible = true;
                    BlockControl29.Visible = true;
                    break;
                case 30:
                    BlockControl1.Visible = true;
                    BlockControl2.Visible = true;
                    BlockControl3.Visible = true;
                    BlockControl4.Visible = true;
                    BlockControl5.Visible = true;
                    BlockControl6.Visible = true;
                    BlockControl7.Visible = true;
                    BlockControl8.Visible = true;
                    BlockControl9.Visible = true;
                    BlockControl10.Visible = true;
                    BlockControl11.Visible = true;
                    BlockControl12.Visible = true;
                    BlockControl13.Visible = true;
                    BlockControl14.Visible = true;
                    BlockControl15.Visible = true;
                    BlockControl16.Visible = true;
                    BlockControl17.Visible = true;
                    BlockControl18.Visible = true;
                    BlockControl19.Visible = true;
                    BlockControl20.Visible = true;
                    BlockControl21.Visible = true;
                    BlockControl22.Visible = true;
                    BlockControl23.Visible = true;
                    BlockControl24.Visible = true;
                    BlockControl25.Visible = true;
                    BlockControl26.Visible = true;
                    BlockControl27.Visible = true;
                    BlockControl28.Visible = true;
                    BlockControl29.Visible = true;
                    BlockControl30.Visible = true;
                    break;           
           
            }

            #endregion

        }
    }
}