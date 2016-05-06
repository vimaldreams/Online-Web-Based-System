using CollateralCreator.Data;
using CollateralCreator.Business;
using CollateralCreator.SQLProvider;

/// <summary>
/// Class to represent built document page within a Xerox application.
/// </summary>
namespace CollateralCreator.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Threading;

    public partial class BuiltDocument : XeroxWebPage
    {
        #region Member variables

        private int templateID = 0;
        private Int16 documentId = 0;
        private bool partnerBranded = false;

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
                RunEmailThread();
            }

            #endregion
        }

        /// <summary>
        /// Fires on click of back to collateral button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnBackToCollateralCreator_Click(object sender, EventArgs e)
        //{
        //    #region Code

        //    Response.Redirect("Home.aspx", true);  //Does this have to be a postback click event? Counldn't it be an asp.net link button or just an link with a button css style?
            
        //    #endregion
        //}

        #endregion

        #region Methods

        /// <summary>
        /// sends email on separate thread
        /// </summary>
        private void RunEmailThread()
        {
            #region Code

            Task thread = null;
            if (documentId != 0) 
            {             
                thread = Task.Factory.StartNew(SendOrderConfirmationEmail);
            }

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

            #endregion
        }

        /// <summary>
        /// Builds and send email to admin and user
        /// </summary>
        private void SendOrderConfirmationEmail()
        {
            #region Code

            //set the culture code for new email thread
            if (Session["CustomCultureCode"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CustomCultureCode"].ToString(), true);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CustomCultureCode"].ToString(), true);
            }

            CollateralCreator.Data.Document Document = CollateralCreatorRepository.GetDocumentByID(documentId);

            string sAdminUrl = string.Empty;
            if (ConfigurationManager.AppSettings["AdminUrl"] != null &&
                ConfigurationManager.AppSettings["AdminUrl"].Length > 0)
            {
                sAdminUrl = ConfigurationManager.AppSettings["AdminUrl"];
            }

            var adminEmailSubject = "Xerox Collateral Creator Job Detail - Part # " + Document.PartNumber;
            StringBuilder sAdminEmailBuilder = new StringBuilder(1000);
            sAdminEmailBuilder.Append("Dear <strong>Admin</strong>, ");
            sAdminEmailBuilder.AppendFormat("<br/><br/>");
            sAdminEmailBuilder.Append("Below are the details to process the Job from Xerox Collateral Creator Tool:");
            sAdminEmailBuilder.AppendFormat("<br/><br/>");
            sAdminEmailBuilder.Append("     <strong>Name of Project - </strong>" + Document.Name);
            sAdminEmailBuilder.AppendFormat("<br/>");
            sAdminEmailBuilder.Append("     <strong>Your Job Number - </strong>" + Document.DocumentID);
            sAdminEmailBuilder.AppendFormat("<br/>");
            sAdminEmailBuilder.Append("     <strong>Date Submitted - </strong>" + DateTime.Now.ToString("G"));
            sAdminEmailBuilder.AppendFormat("<br/><br/>");
            sAdminEmailBuilder.Append("Please click the link provided below to process the job: ");
            sAdminEmailBuilder.AppendFormat("<span style=\"color:blue;\"><a>{0}</a></span>", sAdminUrl);
            sAdminEmailBuilder.AppendFormat("<br/><br/><br/><br/>");
            sAdminEmailBuilder.AppendFormat("Thanks,");
            sAdminEmailBuilder.AppendFormat("<br/>");
            sAdminEmailBuilder.AppendFormat("Xerox Response");

            var emailSubject = GetLocalResourceObject("EmailConfirmHeader").ToString();  //"Xerox Collateral Creator Order Confirmation";
            StringBuilder sUserEmailBuilder = new StringBuilder(1000);
            sUserEmailBuilder.Append("<strong>" + GetLocalResourceObject("EmailThankYou").ToString() + "</strong>"); //Thank You for Your Order
            sUserEmailBuilder.AppendFormat("<br/><br/>");
            sUserEmailBuilder.AppendFormat(GetLocalResourceObject("EmailSalutation").ToString() + " <strong>{0} {1}</strong> - " + GetLocalResourceObject("EmailOrderStatment").ToString(), 
                                            Document.FirstName, Document.LastName);
            sUserEmailBuilder.AppendFormat("<br/>");
            sUserEmailBuilder.Append("==================================================");
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("<bold>" + GetLocalResourceObject("EmailOrderSummary").ToString()  + "</bold>");
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("==================================================");
            sUserEmailBuilder.Append("<br/><br/>");
            sUserEmailBuilder.AppendFormat(GetLocalResourceObject("EmailDate").ToString() + ": {0}",
                                            DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")));
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.AppendFormat(GetLocalResourceObject("EmailOrder").ToString() + ": {0}", Document.Name);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.AppendFormat(GetLocalResourceObject("EmailCopies").ToString() + ": {0}", Document.Quantity);
            sUserEmailBuilder.Append("<br/><br/>");

            sUserEmailBuilder.AppendFormat(GetLocalResourceObject("EmailDelivered").ToString() + ":");
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + GetLocalResourceObject("EmailCareOf").ToString() + "  " + Document.Attention);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + Document.AddressLine1);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + Document.AddressLine2);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + Document.City + "," + Document.State);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + Document.PostCode);
            sUserEmailBuilder.Append("<br/><br/>");

            sUserEmailBuilder.AppendFormat(GetLocalResourceObject("EmailFooterMain").ToString());
            sUserEmailBuilder.Append("<br/><br/>");
            sUserEmailBuilder.AppendFormat(GetLocalResourceObject("EmailFooterSub").ToString());
            sUserEmailBuilder.AppendFormat("<br/><br/><br/><br/>");
            sUserEmailBuilder.Append("© 1999-" + DateTime.Now.Year.ToString() + " " + GetLocalResourceObject("EmailFooterAllrights").ToString());

            var em = new EmailManager();
            //To Admin User
            em.Post(adminEmailSubject, sAdminEmailBuilder.ToString(), true);
            //To the user
            em.Post(Document.Email, emailSubject, sUserEmailBuilder.ToString(), true);

            #endregion
        }

        /// <summary>
        /// Create bread crumb trail
        /// </summary>
        private void BreadCrumb()
        {
            #region code

            this.lblbreadcrumbdesc.Text = GetLocalResourceObject("BreadCrumbText").ToString(); 
           
            SiteMapNode node = SiteMap.CurrentNode;
            do
            {
                Literal link = new Literal();
                link.Text = "<span class=\"currentbreadcrumlink\">" + node.Title + "</span>";
                SiteHierarchy.Controls.AddAt(0, link);

                Label label = new Label();
                label.Text = " > ";
                SiteHierarchy.Controls.AddAt(0, label);

                if (node.PreviousSibling != null && GlobalVariable == string.Empty)
                {
                    Literal plink = new Literal();
                    plink.Text = "<span class=\"breadcrumlink\">" + node.PreviousSibling.Title + "</span>";
                    SiteHierarchy.Controls.AddAt(0, plink);

                    Label nlabel = new Label();
                    nlabel.Text = " > ";
                    SiteHierarchy.Controls.AddAt(0, nlabel);
                }
                
                node = node.ParentNode;
            }
            while (node != null);

            #endregion
        }

        /// <summary>
        /// Loads UI elements
        /// </summary>
        private void LoadDataElements()
        {
            #region Code

            CollateralCreatorRepository.GetDocumentTemplateInfo(PreviewDocument.DocumentId, ref templateID, ref partnerBranded);

            if (partnerBranded)
            {
                ImgDocument.Src = VirtualPathUtility.ToAbsolute("~/images/templates/partnerbrand/thumbnail_" + templateID + ".jpg?v=" + DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                ImgDocument.Src = VirtualPathUtility.ToAbsolute("~/images/templates/xeroxbrand/thumbnail_" + templateID + ".jpg?v=" + DateTime.Now.ToString("yyyyMMdd"));
            }

            #endregion
        }

        /// <summary>
        /// Loads page translations for the corresponding language from UI Culture
        /// </summary>
        private void LoadPageTranslations()
        {
            #region Code

            this.lblHeaderText.Text = GetLocalResourceObject("OrderHeaderText").ToString() ;
            this.lblDeliveryText.Text = GetLocalResourceObject("OrderDeliveryText").ToString() ;
            this.litThanksText.Text = GetLocalResourceObject("OrderThanksText").ToString()  ;
            this.btnBackToCollateralCreator.Text = GetLocalResourceObject("BackHomebtnText").ToString() ;
            
            #endregion
        }

        #endregion
    }
}