using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CollateralCreator.Business;
using CollateralCreator.Data;
using CollateralCreator.SQLProvider;
using Signals.Translations;
using System.Configuration;


/// <summary>
/// Class to represent admin page within a Xerox application.
/// </summary>
namespace CollateralCreator.Web
{
    public partial class AdminOrderScreen : System.Web.UI.Page
    {
        #region Member Variables

        private string sDocumentID = string.Empty;

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
            ((Literal)Master.FindControl("litHeaderText")).Text = "<span class=\"title\" id=\"xrx_bnr_partner_title\">" + GetGlobalResourceObject("MainText", "XeroxTitle").ToString() + "</span>";
            if (!IsPostBack)
            {
                GetTranslations();
                BuildGrid();
            }

            #endregion
        }

        /// <summary>
        /// Fires on page index event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_AdminArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            #region code

            GridView_AdminArea.PageIndex = e.NewPageIndex;
            BuildGrid();

            #endregion
        }

        /// <summary>
        /// Fires on databound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_AdminArea_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region code

            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblselect = (Label)e.Row.FindControl("lblDocumentSelect");
                if (lblselect != null)
                    lblselect.Text = "Select All ";
                Label lblstatus = (Label)e.Row.FindControl("lblDocumentStatus");
                if (lblstatus != null)
                    lblstatus.Text = "Status ";
                Label lblsubmit = (Label)e.Row.FindControl("lblBatchSubmit");
                if (lblsubmit != null)
                    lblsubmit.Text = "BatchSubmit ";
                Button lnkbatchsubmit = (Button)e.Row.FindControl("lnkbatchSubmitStatus");
                if (lnkbatchsubmit != null)
                    lnkbatchsubmit.Text = "Submit ";

                DropDownList ddheaderstatus = (DropDownList)e.Row.FindControl("ddHeaderDocumentStatus");
                if (ddheaderstatus != null)
                {
                    //ddheaderstatus.Items.Add("Change To");
                    //ddheaderstatus.Items.Add("Received");
                    ddheaderstatus.Items.Add("Work-In-Progress");
                    ddheaderstatus.Items.Add("Shipped");
                }
            }

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int objDocumentID = Convert.ToInt32(GridView_AdminArea.DataKeys[e.Row.RowIndex].Value);

                if (objDocumentID != 0)
                {
                    CheckBox chkdocument = (CheckBox)e.Row.FindControl("chkDocument");
                    if (chkdocument != null)
                        chkdocument.Attributes.Add("documentid", objDocumentID.ToString());

                    int bDocumentStatus = 0;
                    string sDocumentStatus = string.Empty;
                    if (drv["DocumentStateID"] != DBNull.Value)
                        bDocumentStatus = Convert.ToInt16(drv["DocumentStateID"]);

                    switch (bDocumentStatus)
                    {
                        case 3:
                            sDocumentStatus = "Received";
                            break;
                        case 4:
                            sDocumentStatus = "Work-In-Progress";
                            break;
                        case 5:
                            sDocumentStatus = "Shipped";
                            break;
                    }

                    DropDownList ddstatus = (DropDownList)e.Row.FindControl("ddDocumentStatus");
                    if (ddstatus != null)
                    {
                        if (sDocumentStatus != "Received")
                        {
                            ddstatus.Items.Add(new ListItem("Work-In-Progress", "4"));
                            ddstatus.Items.Add(new ListItem("Shipped", "5"));
                        }
                        else
                        {
                            ddstatus.Items.Add(new ListItem("Received", "3"));
                            ddstatus.Items.Add(new ListItem("Work-In-Progress", "4"));
                            ddstatus.Items.Add(new ListItem("Shipped", "5"));
                        }

                        ddstatus.Items.FindByText(sDocumentStatus).Selected = true;
                        ddstatus.Attributes.Add("documentid", objDocumentID.ToString());
                    }

                    string sDocumentCarrier = string.Empty;
                    if (drv["Carrier"] != DBNull.Value)
                        sDocumentCarrier = drv["Carrier"].ToString();

                    DropDownList ddcarrier = (DropDownList)e.Row.FindControl("ddDocumentCarrier");
                    if (ddcarrier != null)
                    {
                        ddcarrier.Items.Add(new ListItem("None", "0"));
                        ddcarrier.Items.Add(new ListItem("UPS", "1"));
                        ddcarrier.Items.Add(new ListItem("FedEx", "2"));

                        ddcarrier.Items.FindByText(sDocumentCarrier).Selected = true;
                    }


                    //set the background color based on document status
                    if (sDocumentStatus == "Received")
                        e.Row.Cells[6].BackColor = System.Drawing.Color.LightCoral;
                    else if (sDocumentStatus == "Work-In-Progress")
                        e.Row.Cells[6].BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    else if (sDocumentStatus == "Shipped")
                        e.Row.Cells[6].BackColor = System.Drawing.Color.LightGreen;

                    TextBox txttrackingnumber = (TextBox)e.Row.FindControl("txttrackingnumber");
                    if (txttrackingnumber != null)
                        txttrackingnumber.Attributes.Add("documentid", objDocumentID.ToString());

                    if (txttrackingnumber.Text != null && txttrackingnumber.Text != string.Empty)
                        txttrackingnumber.ReadOnly = true;

                    Button lnkdownload = (Button)e.Row.FindControl("lnkDownloadDocument");
                    if (lnkdownload != null)
                        lnkdownload.Text = "Download";

                    lnkdownload.Attributes.Add("onclick", "javascript:return DownloadDocument(" + objDocumentID + ");");

                    Button lnksubmit = (Button)e.Row.FindControl("lnkSubmitStatus");
                    if (lnksubmit != null)
                    {
                        if (sDocumentStatus == "Shipped")
                            lnksubmit.Visible = false;

                        lnksubmit.Text = "Submit";
                    }
                }
            }


            #endregion
        }

        /// <summary>
        ///  Fires on rowcommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_AdminArea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region code
            
            if (e.CommandName.Equals("BatchUpdate"))
            {
                string sflag = string.Empty;
                string sTrackingNumber = string.Empty;
                string sDocumentCarrier = string.Empty;
                for (int i = 0; i < GridView_AdminArea.Rows.Count; i++)
                {
                    CheckBox chkrow = (CheckBox)GridView_AdminArea.Rows[i].FindControl("chkDocument");
                    if (chkrow.Checked)
                    {
                        TextBox TrackingNumber = (TextBox)GridView_AdminArea.Rows[i].Cells[7].FindControl("txttrackingnumber");
                        Label DocumentId = (Label)GridView_AdminArea.Rows[i].Cells[2].FindControl("lbldocumentid");
                        DropDownList DocumentCarrier = (DropDownList)GridView_AdminArea.Rows[i].Cells[8].FindControl("ddDocumentCarrier");

                        if (TrackingNumber.Text != null && TrackingNumber.Text != string.Empty)
                        {
                            sflag = "success";
                            sDocumentID += DocumentId.Text + ",";
                            sTrackingNumber += TrackingNumber.Text + ",";
                            sDocumentCarrier += DocumentCarrier.SelectedItem.Text + ",";
                        }
                        else
                        {
                            sflag = "failure";
                            sDocumentID = string.Empty;
                            sTrackingNumber = string.Empty;
                            break;
                        }
                    }
                }

                sDocumentID = sDocumentID.TrimEnd(',');
                sTrackingNumber = sTrackingNumber.TrimEnd(',');
                sDocumentCarrier = sDocumentCarrier.TrimEnd(',');
                string sDocumentStatus = ((DropDownList)GridView_AdminArea.HeaderRow.FindControl("ddHeaderDocumentStatus")).SelectedItem.Text;

                //update the status for all the selected jobs
                if (sflag == "success")
                {
                    try
                    {
                        if (sDocumentStatus == "Shipped")
                        {
                            CollateralCreatorRepository.AdminDocument_UpdateBatchStatus(sDocumentID, sDocumentStatus, sTrackingNumber, sDocumentCarrier);
                            ShowAlertMessage("You have successfully shipped the selected jobs.");
                            BuildGrid();
                        }
                        else
                            ShowAlertMessage("Please change the header status to submit the selected jobs.");

                        //Send Shipment Email for all the selected users
                        Task thread = null;
                        if (sDocumentStatus == "Shipped")
                            thread = Task.Factory.StartNew(SendBatchShipmentEmail);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Document Admin Update Error :" + ex.Message);
                    }
                }
                else if (sflag == string.Empty)
                    ShowAlertMessage("Please select atleast one job before submitting the document.");
                else
                    ShowAlertMessage("Please enter Tracking Number for all the selected jobs before submitting the document.");
            }

            if (e.CommandName.Equals("Update"))
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                int iDocumentID = int.Parse(((Label)row.Cells[2].FindControl("lbldocumentid")).Text);
                string sDocumentStatus = ((DropDownList)row.Cells[6].FindControl("ddDocumentStatus")).SelectedItem.Text;
                string sTrackingNumber = ((TextBox)row.Cells[7].FindControl("txttrackingnumber")).Text;
                string sDocumentCarrier = ((DropDownList)row.Cells[8].FindControl("ddDocumentCarrier")).SelectedItem.Text;

                if (sDocumentStatus == "Shipped")
                {
                    CheckBox chkrow = (CheckBox)row.Cells[5].FindControl("chkDocument");
                    if (chkrow.Checked)
                    {
                        if (sTrackingNumber != null && sTrackingNumber != string.Empty)
                        {
                            try
                            {
                                //Send Shipment Email to the user
                                if (sDocumentStatus == "Shipped")
                                {
                                    ViewState["DocumentID"] = iDocumentID;
                                    CollateralCreatorRepository.AdminDocument_UpdateStatus(iDocumentID, sDocumentStatus,
                                                                                      sTrackingNumber, sDocumentCarrier);
                                    ShowAlertMessage("You have successfully shipped the selected job.");
                                    BuildGrid();
                                }

                                //Send Shipment Email for all the selected users
                                Task thread = null;
                                if (sDocumentStatus == "Shipped")
                                    thread = Task.Factory.StartNew(SendShipmentEmail);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("Document Admin Update Error :" + ex.Message);
                            }
                        }
                        else
                            ShowAlertMessage("Please enter Tracking Number before submitting the document.");
                    }
                    else
                        ShowAlertMessage("Please select the job before updating the status.");
                }
                else
                    ShowAlertMessage("Please Change the Status before submitting the document.");
            }

            #endregion
        }

        /// <summary>
        /// Fires on batch select checkbox event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_AdminArea_BatchSelect(object sender, EventArgs e)
        {
            #region code

            CheckBox chk = (CheckBox)GridView_AdminArea.HeaderRow.FindControl("chkHeaderDocument");
            if (chk.Checked)
            {
                for (int i = 0; i < GridView_AdminArea.Rows.Count; i++)
                {
                    CheckBox chkrow = (CheckBox)GridView_AdminArea.Rows[i].FindControl("chkDocument");
                    chkrow.Checked = true;

                    Button btn = (Button)GridView_AdminArea.Rows[i].FindControl("lnkSubmitStatus");
                    btn.Visible = false;
                }
            }
            else
            {
                for (int i = 0; i < GridView_AdminArea.Rows.Count; i++)
                {
                    CheckBox chkrow = (CheckBox)GridView_AdminArea.Rows[i].FindControl("chkDocument");
                    chkrow.Checked = false;

                    Button btn = (Button)GridView_AdminArea.Rows[i].FindControl("lnkSubmitStatus");
                    btn.Visible = true;
                }
            }

            #endregion
        }
        
        /// <summary>
        /// Fires on download usage button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btndownload_UsageReport(object sender, EventArgs e)
        {
            #region code

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CollateralCreatorUsageReport.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            StringBuilder sb = new StringBuilder();

            DataTable datatable = new DataTable();
            if (ViewState["AdminData"] != null)
                datatable = (DataTable)ViewState["AdminData"];

            foreach (DataColumn dc in datatable.Columns)
            {
                if (dc.ColumnName != "DocumentStateID")
                    sb.Append(dc.ColumnName + ',');
            }
            sb.Append("\r\n");
            int i;
            foreach (DataRow dr in datatable.Rows)
            {
                for (i = 0; i < datatable.Columns.Count; i++)
                {
                    if (dr[i].ToString().Contains(','))
                        dr[i] = dr[i].ToString().Replace(",", "");

                    //don't include the Document status ID
                    if (i >= 0 && i != 21)
                    {
                        //format the postcode as a string
                        if (i == 17)
                            sb.Append("=\"" + dr[i].ToString() + "\"" + ',');
                        else
                            sb.Append(dr[i].ToString() + ',');
                    }
                }
                sb.Append("\r\n");
            }


            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();

            #endregion
        }

        /// <summary>
        /// Fires on download archive button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btndownload_ArchiveReport(object sender, EventArgs e)
        {
            #region code

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CollateralCreatorUsageReport.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            StringBuilder sb = new StringBuilder();

            List<Document> objDocuments = CollateralCreatorManager.GetDocumentArchiveHistory();

            DataTable datatable = LoadDocumentQueue(objDocuments);

            foreach (DataColumn dc in datatable.Columns)
            {
                if (dc.ColumnName != "DocumentStateID")
                    sb.Append(dc.ColumnName + ',');
            }
            sb.Append("\r\n");
            int i;
            foreach (DataRow dr in datatable.Rows)
            {
                for (i = 0; i < datatable.Columns.Count; i++)
                {
                    if (dr[i].ToString().Contains(','))
                        dr[i] = dr[i].ToString().Replace(",", "");

                    //don't include the Document status ID
                    if (i >= 0 && i != 21) 
                    {
                        //format the postcode as a string
                        if (i == 17)
                            sb.Append("=\"" + dr[i].ToString() + "\"" + ',');
                        else
                            sb.Append(dr[i].ToString() + ',');
                    }
                }
                sb.Append("\r\n");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();

            #endregion
        }
        
        /// <summary>
        /// Fires on row update event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_AdminArea_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            #region code

            //GridViewRow row = GridView_AdminArea.Rows[e.RowIndex];
            //Int32 iDocumentID = Int32.Parse(GridView_AdminArea.DataKeys[e.RowIndex].Value.ToString());

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the datasource into gridview
        /// </summary>
        private void BuildGrid()
        {
            #region code

            List<Document> objDocuments = CollateralCreatorManager.GetDocumentPrintQueue();

            DataTable dt = LoadDocumentQueue(objDocuments);
            if(dt.Rows.Count == 0)
            {
                btndownlaod.Visible = false;
                pnlAdmin.Visible = true;
                pnlTable.Visible = false;
            }
            else
            {
                ViewState["AdminData"] = dt;
                GridView_AdminArea.DataSource = dt;
                GridView_AdminArea.DataBind();
            }

            #endregion
        }

        /// <summary>
        /// Loads data into table
        /// </summary>
        /// <param name="objDocuments"></param>
        /// <returns></returns>
        private DataTable LoadDocumentQueue(List<Document> objDocuments)
        {
            #region code

            DataTable table = new DataTable();
            
            table.Columns.Add("DocumentID", typeof(Int32));
            table.Columns.Add("Name", typeof(String));
            table.Columns.Add("CompanyName", typeof(String));
            table.Columns.Add("Reseller ID", typeof(String));
            table.Columns.Add("Company ID", typeof(String));
            table.Columns.Add("FirstName", typeof(String));
            table.Columns.Add("LastName", typeof(String));
            table.Columns.Add("Phone", typeof(String));
            table.Columns.Add("Email", typeof(String));
            table.Columns.Add("CreatedDate", typeof(String));
            table.Columns.Add("ModifiedDate", typeof(String));
            table.Columns.Add("Quantity", typeof(String));
            table.Columns.Add("Delivery To/CompanyName", typeof(String));
            table.Columns.Add("Delivery Address1", typeof(String));
            table.Columns.Add("Delivery Address2", typeof(String));
            table.Columns.Add("City", typeof(String));
            table.Columns.Add("State", typeof(String));
            table.Columns.Add("PostCode", typeof(String));
            table.Columns.Add("Country", typeof(String));
            table.Columns.Add("Delivery/AttentionTo", typeof(String));
            table.Columns.Add("Delivery Phone", typeof(String));
            table.Columns.Add("DocumentStateID", typeof(Int16));
            table.Columns.Add("DocumentStatus", typeof(String));
            table.Columns.Add("TrackingNumber", typeof(String));
            table.Columns.Add("Simplex/Duplex", typeof(String));
            table.Columns.Add("Paper Size", typeof(String));
            table.Columns.Add("Paper Option", typeof(String));
            table.Columns.Add("Print Mode", typeof(String));
            table.Columns.Add("Color Correction", typeof(String));
            table.Columns.Add("PartNumber", typeof(String));
            table.Columns.Add("Carrier", typeof(String));            

            foreach(Document d in objDocuments)
            {
                string sGeometry = string.Empty;
                if (d.Duplex)
                    sGeometry = "Duplex";
                else
                    sGeometry = "Simplex";

                string sDocumentstatus = string.Empty;
                switch (d.DocumentStateID)
                {
                    case 3:
                        sDocumentstatus = "Received";
                        break;
                    case 4:
                        sDocumentstatus = "Work-In-Progress";
                        break;
                    case 5:
                        sDocumentstatus = "Shipped";
                        break;
                }
                
                string OriginDateFormat = d.CreatedDate.ToString("MM/dd/yy");
                string ModifiedDateFormat = d.ModifiedDate.ToString("MM/dd/yy");

                table.Rows.Add(d.DocumentID, d.Name, d.CompanyName, d.ChannelPartnerLoginID, d.CompanyID, d.FirstName, d.LastName, d.Phone, d.Email, OriginDateFormat, ModifiedDateFormat,
                            d.Quantity, d.CompanyName, d.AddressLine1, d.AddressLine2, d.City, d.State, d.PostCode, d.Country, d.Attention, d.Phone,
                            d.DocumentStateID, sDocumentstatus, d.TrackingNumber, sGeometry, d.PageSize, d.PaperOption, d.PrintMode, d.ColorCorrection, d.PartNumber.Substring(0,9), d.Carrier);
                
            }
            
            return table;

            #endregion
        }
                
        /// <summary>
        /// Sends bacth email when multiple order status is changed
        /// </summary>
        private void SendBatchShipmentEmail()
        {
            #region code

            List<CollateralCreator.Data.Document> shipdocuments = CollateralCreatorRepository.GetDocumentByIDs(sDocumentID);

            //send batch email to all the users
            var emailSubject = " Xerox Collateral Creator Order - Shipment Details";
            foreach (Document shipdocument in shipdocuments)
            {
                StringBuilder sUserEmailBuilder = new StringBuilder(1000);
                sUserEmailBuilder.Append("Thank You for your Order");
                sUserEmailBuilder.AppendFormat("<br/><br/>");
                sUserEmailBuilder.AppendFormat("Dear <strong>{0} {1}</strong> - your order has been shipped.", shipdocument.FirstName, shipdocument.LastName);
                sUserEmailBuilder.AppendFormat("<br/>");
                sUserEmailBuilder.Append("==================================================");
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.Append("<bold>SHIPPING DETAILS:</bold>");
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.Append("==================================================");
                sUserEmailBuilder.Append("<br/><br/>");
                sUserEmailBuilder.AppendFormat("Shipped Date: {0}", DateTime.Now.ToString("D"));
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.AppendFormat("Shipped Order: {0}", shipdocument.Name);
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.AppendFormat("Shipped Copies: {0}", shipdocument.Quantity);
                sUserEmailBuilder.Append("<br/><br/>");

                sUserEmailBuilder.AppendFormat("Your shipping address:");
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.Append("      c/o  " + shipdocument.Attention);
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.Append("      " + shipdocument.AddressLine1);
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.Append("      " + shipdocument.AddressLine2);
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.Append("      " + shipdocument.City + "," + shipdocument.State);
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.Append("      " + shipdocument.PostCode);
                sUserEmailBuilder.Append("<br/><br/>");
                
                string carrierUrl = string.Empty;
                if (shipdocument.Carrier == "UPS")
                    carrierUrl = "http://www.ups.com/tracking/tracking.html";
                else
                    carrierUrl = "http://www.fedex.com/Tracking?cntry_code=ca_english";

                sUserEmailBuilder.AppendFormat("Your Tracking Number is <strong>{0}</strong> with <strong><a href=\"{2}\" style=\"color:blue;\">{1}</a></strong>", shipdocument.TrackingNumber, shipdocument.Carrier, carrierUrl);
                sUserEmailBuilder.Append("<br/><br/>");
                sUserEmailBuilder.AppendFormat("<strong>Contact your Xerox Inside Partner Manager</strong> if you have any questions.");
                sUserEmailBuilder.AppendFormat("<br/><br/><br/><br/>");
                sUserEmailBuilder.Append("© 1999-2012 XEROX CORPORATION. All rights reserved.");

                var em = new EmailManager();
               
                //To the user
                em.Post(shipdocument.Email, emailSubject, sUserEmailBuilder.ToString(), true);
            }

            #endregion
        }

        /// <summary>
        /// Sends email when the order status is changed
        /// </summary>
        private void SendShipmentEmail()
        {
            #region code

            int iDocumentID = (int) ViewState["DocumentID"];
            CollateralCreator.Data.Document shipdocument = CollateralCreatorRepository.GetDocumentByID(iDocumentID);

            var emailSubject = " Xerox Collateral Creator Order - Shipment Details";

            StringBuilder sUserEmailBuilder = new StringBuilder(1000);
            sUserEmailBuilder.Append("Thank You for your Order");
            sUserEmailBuilder.AppendFormat("<br/><br/>");
            sUserEmailBuilder.AppendFormat("Dear <strong>{0} {1}</strong> - your order has been shipped.",
                                           shipdocument.FirstName, shipdocument.LastName);
            sUserEmailBuilder.AppendFormat("<br/>");
            sUserEmailBuilder.Append("==================================================");
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("<bold>SHIPPING DETAILS</bold>");
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("==================================================");
            sUserEmailBuilder.Append("<br/><br/>");
            sUserEmailBuilder.AppendFormat("Shipped Date: {0}",
                                           DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")));
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.AppendFormat("Shipped Order: {0}", shipdocument.Name);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.AppendFormat("Shipped Copies: {0}", shipdocument.Quantity);
            sUserEmailBuilder.Append("<br/><br/>");

            sUserEmailBuilder.AppendFormat("Your shipping address:");
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      c/o  " + shipdocument.Attention);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + shipdocument.AddressLine1);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + shipdocument.AddressLine2);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + shipdocument.City + "," + shipdocument.State);
            sUserEmailBuilder.Append("<br/>");
            sUserEmailBuilder.Append("      " + shipdocument.PostCode);
            sUserEmailBuilder.Append("<br/><br/>");

            string carrierUrl = string.Empty;

            if (shipdocument.Carrier == "UPS")
            {
                carrierUrl = "http://www.ups.com/tracking/tracking.html";
                sUserEmailBuilder.AppendFormat(
                   "Your Tracking Number is: <strong>{0}</strong>", shipdocument.TrackingNumber);
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.AppendFormat("Carrier: <strong><a href=\"{1}\" style=\"color:blue;\">{0}</a></strong>", shipdocument.Carrier, carrierUrl);
            }
            else if (shipdocument.Carrier == "FedEx")
            {
                carrierUrl = "http://www.fedex.com/Tracking?cntry_code=ca_english";
                sUserEmailBuilder.AppendFormat(
                   "Your Tracking Number is: <strong>{0}</strong>", shipdocument.TrackingNumber);
                sUserEmailBuilder.Append("<br/>");
                sUserEmailBuilder.AppendFormat("Carrier: <strong><a href=\"{1}\" style=\"color:blue;\">{0}</a></strong>", shipdocument.Carrier, carrierUrl);
            }
            else 
            {
                sUserEmailBuilder.AppendFormat(
                       "Your Tracking Number is: <strong>{0}</strong>", shipdocument.TrackingNumber);            
            }

            sUserEmailBuilder.Append("<br/><br/>");
            sUserEmailBuilder.AppendFormat(
                "<strong>Contact your Xerox Inside Partner Manager</strong> if you have any questions.");
            sUserEmailBuilder.AppendFormat("<br/><br/><br/><br/>");
            sUserEmailBuilder.Append("© 1999-" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture) + " XEROX CORPORATION. All rights reserved.");

            var em = new EmailManager();

            //To the user
            em.Post(shipdocument.Email, emailSubject, sUserEmailBuilder.ToString(), true);

            #endregion
        }

        /// <summary>
        /// Custom Javascript popup
        /// </summary>
        /// <param name="error"></param>
        private void ShowAlertMessage(string error)
        {
            #region code

            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
            if (page != null)
            {
                error = error.Replace("'", "\'");
                ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');", true);
            }

            #endregion
        }
        
        /// <summary>
        /// Loads page translations for the corresponding language
        /// </summary>
        private void GetTranslations()
        {
            #region code

            lblAdminEmptyMessage.Text = "No Documents are in the Print Queue !!!";
            lblAdminHeader.Text = "Collateral Creator - Admin Area";
            lblAdminQueue.Text = "Print Queue";
            btndownlaod.Text = "Download Usage Report";
            GridView_AdminArea.Columns[0].HeaderText = "Name of Project";
            GridView_AdminArea.Columns[1].HeaderText = "PartNumber";
            GridView_AdminArea.Columns[2].HeaderText = "Country";
            GridView_AdminArea.Columns[3].HeaderText = "Job #";
            GridView_AdminArea.Columns[4].HeaderText = "Contact";
            GridView_AdminArea.Columns[5].HeaderText = "Submitted Date";
            GridView_AdminArea.Columns[7].HeaderText = "Status";
            GridView_AdminArea.Columns[8].HeaderText = "Tracking #";
            GridView_AdminArea.Columns[9].HeaderText = "Modified Date";
            GridView_AdminArea.Columns[10].HeaderText = "Carrier";
            GridView_AdminArea.Columns[11].HeaderText = "";

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        #endregion
    }
}