using CollateralCreator.Data;
using CollateralCreator.Business;
using CollateralCreator.SQLProvider;

/// <summary>
/// Class to represent a order summary page within a Xerox application.
/// </summary>
namespace CollateralCreator.Web
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;    

    public partial class OrderSummary : XeroxWebPage
    {
        #region Member Variables

        private Int16 documentId = 0;
        private Int16 globalLogo = -1;

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
        /// Fires on click of submit order button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            #region Code

            if (ViewState["document"] != null)
            {
                int docId = Convert.ToInt16(ViewState["document"]);

                //Update the Document Contact Details on successfull order confirmation
                if (Session["OrderInformation"] != null)
                {
                    CollateralCreator.Data.Document orderDocument = (Document)Session["OrderInformation"];

                    CollateralCreatorRepository.DocumentUpdateAddressDetails(orderDocument.DocumentID, 0, false,
                                                                             orderDocument.Attention,
                                                                             orderDocument.CompanyName,
                                                                             orderDocument.CompanyID,
                                                                             orderDocument.AddressLine1,
                                                                             orderDocument.AddressLine2,
                                                                             orderDocument.City,
                                                                             orderDocument.State,
                                                                             orderDocument.PostCode,
                                                                             orderDocument.FirstName,
                                                                             orderDocument.LastName,
                                                                             orderDocument.Phone,
                                                                             orderDocument.Email,
                                                                             orderDocument.ChannelPartnerPhone,
                                                                             orderDocument.Country);

                    //send the document status correspondingly 1-New 2-Submitted 3-Received 4-Dispatched
                    //Change the document status to Submitted 
                    CollateralCreatorRepository.ChangeDocumentStatusByID(docId, "Work-In-Progress", orderDocument.Quantity, "UPS");

                    Response.Redirect("BuiltDocument.aspx?documentid=" + ViewState["document"], true);
                }
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
            #region code

            if (ViewState["document"] != null)
                Response.Redirect("OrderDocument.aspx?documentid=" + ViewState["document"], true);

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
            int oscount = 1;
            SiteMapNode node = SiteMap.CurrentNode;
            do
            {
                Literal link = new Literal();
                if (oscount <= 3 && Request.QueryString["documentid"] != null)
                {
                    if (node.Url.Contains("OrderSummary.aspx"))
                        link.Text = "<span class=\"currentbreadcrumlink\">" + node.Title + "</span>";
                    else
                        link.Text = "<a href=\"" + node.Url + "?documentid=" + documentId + "\" class=\"breadcrumlink\">" + node.Title + "</a>";
                        //link.Text = "<span class=\"breadcrumlink\">" + node.Title + "</span>";
                }

                else if (oscount == 4)
                    link.Text = "<a href=\"" + node.Url + "\" class=\"breadcrumlink\">" + node.Title + "</a>";
                    //link.Text = "<span class=\"breadcrumlink\">" + node.Title + "</span>";

                SiteHierarchy.Controls.AddAt(0, link);

                Label label = new Label();
                label.Text = " > ";
                SiteHierarchy.Controls.AddAt(0, label);

                if (node.PreviousSibling != null && GlobalVariable == string.Empty)
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
                oscount++;
            } while (node != null);
            #endregion
        }

        /// <summary>
        /// Loads UI elements
        /// </summary>
        private void LoadDataElements()
        {
            #region Code

            //get the Document object for the template and brand provided
            CollateralCreator.Data.Document objdocument = DocumentManager.GetDocument(documentId);
            lblSelectedProductText.Text = objdocument.Name;

            StringBuilder sbsummary = new StringBuilder(1000);

            if (objdocument != null && objdocument.Pages.Count != 0)
            {
                int PageNum = 1;
                int imagecount = 1;
                int areacount = 1;
                //Page docpage in objdocument.Pages
                for (int p = 0; p < objdocument.Pages.Count; p++)
                {
                    if (objdocument.Pages[p].CustomizableAreas.Count > 0)
                    {
                        //Customizable area doc in objdocument.Pages.CustomizableArea
                        foreach (CustomizableArea ca in objdocument.Pages[p].CustomizableAreas)
                        {
                            if (ca.TextArea != null)
                            {
                                sbsummary.Append(
                                    "<tr><td><strong><label id=\"lblCustomizeArea1\" class=\"headerText\">" +
                                    GetLocalResourceObject("CustomizeAreaTxt").ToString() + " - " + areacount +
                                    "</label></strong></td></tr>");
                                sbsummary.Append(
                                    "<tr><td class=\"tdsummarytext\"><label id=\"lblCustomizeAreaText1\">" +
                                    ca.TextArea.Text +
                                    "</label></td></tr> ");
                                areacount++;
                            }
                            if (ca.ImageArea != null)
                            {
                                if (imagecount == 1)
                                {
                                    sbsummary.Append(
                                        "<tr><td><strong><label id=\"lblResellerLogo\" class=\"headerText\">" +
                                        GetLocalResourceObject("PartnerLogoTxt").ToString() + "</label></strong></td></tr>");

                                    if (globalLogo == 1)
                                        sbsummary.Append(
                                            "<tr><td class=\"tdsummarytext\"><img alt=\"Partnerlogo\" src=\"Handlers/ImageHandler.ashx?mode=documentimage&imageid0=" +
                                            ca.ImageArea.ImageAreaID +
                                            "\" class=\"Logosummaryimage\"/></td></tr>");
                                    else
                                    {
                                        Xerox.SSOComponents.Models.Image img = null;
                                        if (ChannelPartner.Images.Count > 0) img = this.ChannelPartner.Images[0];
                                        string sLoginId = this.ChannelPartner.LoginId;
                                        sbsummary.Append(
                                            "<tr><td class=\"tdsummarytext\"><img alt=\"Partnerlogo\" src=\"Handlers/ImageHandler.ashx?mode=defaultimage&loginid=" + sLoginId + "v=" + DateTime.Now.ToString("yyyyMMdd") + "\" class=\"Logosummaryimage\" /></td></tr>");
                                    }
                                }
                                imagecount++;
                            }
                        }
                    }
                    PageNum++;
                }
            }
            litCustomizeDocumentSection.Text += sbsummary.ToString();

            if (Session["OrderInformation"] != null)
            {

                CollateralCreator.Data.Document orderDocument = (Document) Session["OrderInformation"];

                StringBuilder sbprintoptions = new StringBuilder(1000);
                sbprintoptions.Append(
                    "<tr class=\"tdprinttext\"><td class=\"tdprintoption\"><label id=\"lblquantity\" class=\"headerText\">" +
                    GetLocalResourceObject("QuantityLabel").ToString() + "</label></td>");
                sbprintoptions.AppendFormat(
                    "<td><label id=\"lblquantitytext\" class=\"headerText\">{0}</label></td></tr>",
                    orderDocument.Quantity);

                sbprintoptions.Append(
                    "<tr class=\"tdprinttext\"><td class=\"tdprintoption\"><label id=\"lblprintjob\" class=\"headerText\">" +
                    GetLocalResourceObject("PrintJobLabel").ToString() + "</label></td>");
                sbprintoptions.AppendFormat(
                    "<td><label id=\"lblprintjobtext\" class=\"headerText\">{0}</label></td></tr>", objdocument.Name);

                sbprintoptions.Append(
                    "<tr class=\"tdprinttext\"><td class=\"tdprintoption\"><label id=\"lblgeometry\" class=\"headerText\">" +
                    GetLocalResourceObject("GeometryLabel").ToString() + "</label></td>");
                if (objdocument.Duplex)
                    sbprintoptions.AppendFormat(
                        "<td><label id=\"lblgeometrytext\" class=\"headerText\">{0}</label></td></tr>", "Duplex");
                else
                    sbprintoptions.AppendFormat(
                        "<td><label id=\"lblgeometrytext\" class=\"headerText\">{0}</label></td></tr>", "Simplex");

                sbprintoptions.Append(
                    "<tr class=\"tdprinttext\"><td class=\"tdprintoption\"><label id=\"lblpaperoptions\" class=\"headerText\">" +
                    GetLocalResourceObject("PaperOptionsLabel").ToString() + "</label></td>");
                sbprintoptions.AppendFormat(
                    "<td><label id=\"lblpaperoptionstext\" class=\"headerText\">{0}</label></td></tr>",
                    objdocument.PaperOption);

                sbprintoptions.Append(
                    "<tr class=\"tdprinttext\"><td class=\"tdprintoption\"><label id=\"lblpapersize\" class=\"headerText\">" +
                    GetLocalResourceObject("PaperSizeLabel").ToString() + "</label></td>");
                sbprintoptions.AppendFormat(
                    "<td><label id=\"lblpapersizetext\" class=\"headerText\">{0}</label></td></tr>",
                    objdocument.PaperSize);

                sbprintoptions.Append(
                    "<tr class=\"tdprinttext\"><td class=\"tdprintoption\"><label id=\"lblcolorcorrection\" class=\"headerText\">" +
                    GetLocalResourceObject("ColorCorrectionLabel").ToString() + "</label></td>");
                sbprintoptions.AppendFormat(
                    "<td><label id=\"lblcolorcorrectiontext\" class=\"headerText\">{0}</label></td></tr>",
                    objdocument.ColorCorrection);

                sbprintoptions.Append(
                    "<tr class=\"tdprinttext\"><td class=\"tdprintoption\"><label id=\"lblprintmode\" class=\"headerText\">" +
                    GetLocalResourceObject("PrintModeText").ToString() + "</label></td>");
                sbprintoptions.AppendFormat(
                    "<td><label id=\"lblprintmodetext\" class=\"headerText\">{0}</label></td></tr>",
                    objdocument.PrintMode);
                litPrintOptionsText.Text = sbprintoptions.ToString();

                StringBuilder sbdeliveryaddress = new StringBuilder(1000);
                
                if(orderDocument.Attention != string.Empty)
                    sbdeliveryaddress.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lblattention\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.Attention);
                if (orderDocument.CompanyName != string.Empty)
                    sbdeliveryaddress.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lblcompanyname\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.CompanyName);
                if (orderDocument.AddressLine1 != string.Empty)
                    sbdeliveryaddress.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lbladdressline1\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.AddressLine1);
                if (orderDocument.AddressLine2 != string.Empty)
                    sbdeliveryaddress.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lbladdressline2\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.AddressLine2);
                if (orderDocument.City != string.Empty)
                    sbdeliveryaddress.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lblcity\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.City);
                if (orderDocument.State != string.Empty)
                    sbdeliveryaddress.AppendFormat(
                     "<tr><td class=\"tdsummarytext\"><label id=\"lblstate\" class=\"headerText\">{0}</label></td></tr>",
                     orderDocument.State);
                if (orderDocument.PostCode != string.Empty)
                    sbdeliveryaddress.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lblpostcode\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.PostCode);
                litDeliveryAddressText.Text = sbdeliveryaddress.ToString();

                StringBuilder sbordercontact = new StringBuilder(1000);
                if (orderDocument.FirstName != string.Empty)
                    sbordercontact.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lblfirstname\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.FirstName);
                if (orderDocument.LastName != string.Empty)
                    sbordercontact.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lbllastname\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.LastName);
                if (orderDocument.Phone != string.Empty)
                    sbordercontact.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lblphone\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.Phone);
                if (orderDocument.Email != string.Empty)
                    sbordercontact.AppendFormat(
                        "<tr><td class=\"tdsummarytext\"><label id=\"lblemail\" class=\"headerText\">{0}</label></td></tr>",
                        orderDocument.Email);
                litOrderContactText.Text = sbordercontact.ToString();
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
        /// Parse query string and builds logic to create the document
        /// </summary>
        private void ProcessVars()
        {
            #region Code

            if (Request.QueryString["documentid"] != null)
                Int16.TryParse(Request.QueryString["documentid"], out documentId);
            if (Request.QueryString["documentid"] != null)
                Int16.TryParse(Request.QueryString["logo"], out globalLogo);

            ViewState["document"] = documentId;

            #endregion
        }

        /// <summary>
        /// Loads page translations for the corresponding language from UI Culture
        /// </summary>
        private void LoadPageTranslations()
        {
            #region Code
           
            this.lblHeaderText.Text = GetLocalResourceObject("SummaryHeaderText").ToString();
            this.lblSubHeaderText.Text = GetLocalResourceObject("SummarySubheaderText").ToString();
            this.lblSelectProduct.Text = GetLocalResourceObject("SummarySelectedProductText").ToString();
            this.lblPrintOptions.Text = GetLocalResourceObject("SummaryPrintOptionsText").ToString();
            this.lblDeliveryAddress.Text = GetLocalResourceObject("SummaryDeliveryText").ToString();
            this.lblOrderContactInfo.Text = GetLocalResourceObject("SummaryOrderContactText").ToString();
            this.btnSubmitOrder.Text = GetLocalResourceObject("SubmitOrderbtnText").ToString();
            this.btnCancel.Text = GetLocalResourceObject("CancelbtnText").ToString();

            #endregion
        }

        #endregion
    }
}
