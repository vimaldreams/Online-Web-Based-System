using CollateralCreator.Business;
using CollateralCreator.Data;
using CollateralCreator.SQLProvider;
using Signals.Translations;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Models;

/// <summary>
/// Class to represent a home page within a Xerox application.
/// </summary>
/// 
namespace CollateralCreator.Web
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Home : XeroxWebPage
    {
        #region Member variables

        public string Message { get; set; }
        private string productCodes { get; set; }
        private string languageCode { get; set; }

        private List<MenuTree> menuItems = null;     

        #endregion

        #region Events

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Code
            try
            {

                string[] array = ConfigurationManager.AppSettings.Get("adminUsers").Split(',');

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].ToLower().Trim().Equals(ChannelPartner.Email.Trim().ToLower()))
                    {                             
                        Response.Redirect("~/Portal/AdminOrderScreen.aspx", true);
                    }
                }

                if (!IsPostBack)
                {
                    LoadPageTranslations(); //MPE 4/32013
                    BreadCrumb();
                    BuildJavascript();
                    //LoadPageTranslations(); MPE 4/3/2013
                    CreateButtons();
                    BuildProductTreeView();
                }
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.Message); }
            #endregion
        }             

        #endregion

        #region Methods

        /// <summary>
        /// Create breadcrumb trail
        /// </summary>
        private void BreadCrumb()
        {
            #region code

            this.lblbreadcrumbdesc.Text = GetLocalResourceObject("BreadCrumbTxt").ToString(); //tg.GetTranslation("BreadCrumbTxt").ForeignText; 
            SiteMapNode node = SiteMap.CurrentNode;
            do
            {
                Literal link = new Literal();
                link.Text = "<span class=\"currentbreadcrumlink\">" + node.Title + "</span>";
                SiteHierarchy.Controls.AddAt(0, link);

                Label label = new Label();
                label.Text = " > ";
                SiteHierarchy.Controls.AddAt(0, label);

                node = node.ParentNode;
            }
            while (node != null);

            #endregion
        }
                
        /// <summary>
        /// Method to create custom javascript block to create button text on jquery function in menu
        /// </summary>
        private void BuildJavascript()
        {
            #region Code
            litJavaScript.Text = "<script type=\"text/javascript\" language=\"javascript\">\n";
            litJavaScript.Text += "var sNoDetailsFlag =\"\";\n";
        
            //litJavaScript.Text += "var sDefaultProduct =\"" + GetLocalResourceObject("DefaultProduct").ToString() + "\";\n";
            litJavaScript.Text += "var sBrandHeader =\"" + GetLocalResourceObject("BrandHeaderText").ToString() + "\";\n";
            litJavaScript.Text += "var sXeroxBrand =\"" + GetLocalResourceObject("XeroxBrandText").ToString() + "\";\n";
            litJavaScript.Text += "var sPartnerBrand =\"" + GetLocalResourceObject("PartnerBrandText").ToString() + "\";\n";
            if (GetLocalResourceObject("ResellerBrandText") != null)
                litJavaScript.Text += "var sResellerBrand =\"" + GetLocalResourceObject("ResellerBrandText").ToString() + "\";\n";            
            litJavaScript.Text += "var sBtnPreview =\"" + GetLocalResourceObject("PreviewbtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sBtnEdit =\"" + GetLocalResourceObject("EditbtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sBtnDownload =\"" + GetLocalResourceObject("DownloadbtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sBtnOrderPrint =\"" + GetLocalResourceObject("OrderbtnText").ToString() + "\";\n";
            litJavaScript.Text += "var sCPartnerId =\"" + this.ChannelPartner.ChannelPartnerId + "\";\n";
            litJavaScript.Text += "var sCLoginId =\"" + this.ChannelPartner.LoginId + "\";\n";
            litJavaScript.Text += "var sCEmail =\"" + this.ChannelPartner.Email + "\";\n";
            litJavaScript.Text += "var sNoDetailMessage =\"" + GetLocalResourceObject("NoDetailsLabel").ToString() + "\";\n";

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

                litJavaScript.Text += "var sLanguage =\"" + languageCode + "\";\n";
            }           

            Address a = null;
            if (this.ChannelPartner.Addresses.Count > 0) a = this.ChannelPartner.Addresses[0];

            litJavaScript.Text += "var sjAttention =\"\";\n";
            litJavaScript.Text += "var sjCompanyName =\"" + this.ChannelPartner.LocationName + "\";\n";
            if (a != null)
            {
                if (a.AddressLine1 != string.Empty && a.AddressLine1 != null)
                    litJavaScript.Text += "var sjAddressLine1 =\"" + a.AddressLine1 + "\";\n";
                else
                    litJavaScript.Text += "var sjAddressLine1 =\"\";\n";
                if (a.AddressLine2 != string.Empty && a.AddressLine2 != null)
                    litJavaScript.Text += "var sjAddressLine2 =\"" + a.AddressLine2 + "\";\n";
                else
                    litJavaScript.Text += "var sjAddressLine2 =\"\";\n";
                if (a.Town != string.Empty && a.Town != null)
                    litJavaScript.Text += "var sjCity =\"" + a.Town + "\";\n";
                else
                    litJavaScript.Text += "var sjCity =\"\";\n";
                if (a.State != string.Empty && a.State != null)
                    litJavaScript.Text += "var sjState =\"" + a.State + "\";\n";
                else
                    litJavaScript.Text += "var sjState =\"\";\n";
                if (a.PostCode != string.Empty && a.PostCode != null)
                    litJavaScript.Text += "var sjPostCode =\"" + a.PostCode + "\";\n";
                else
                    litJavaScript.Text += "var sjPostCode =\"\";\n";
            }
            else{
                litJavaScript.Text += "var sjAddressLine1 =\"\";\n";
                litJavaScript.Text += "var sjAddressLine2 =\"\";\n";
                litJavaScript.Text += "var sjCity =\"\";\n";
                litJavaScript.Text += "var sjState =\"\";\n";
                litJavaScript.Text += "var sjPostCode =\"\";\n";
               
            }
            if (this.ChannelPartner.FirstName != string.Empty && this.ChannelPartner.FirstName != null)
                litJavaScript.Text += "var sjFirstName =\"" + this.ChannelPartner.FirstName + "\";\n";
            else
                litJavaScript.Text += "var sjFirstName =\"\";\n";
            if (this.ChannelPartner.LastName != string.Empty && this.ChannelPartner.LastName != null)
                litJavaScript.Text += "var sjLastName =\"" + this.ChannelPartner.LastName + "\";\n";
            else
                litJavaScript.Text += "var sjFirstName =\"\";\n";
            if (this.ChannelPartner.Email != string.Empty && this.ChannelPartner.Email != null)
                litJavaScript.Text += "var sjEmail =\"" + this.ChannelPartner.Email + "\";\n";
            else
                litJavaScript.Text += "var sjFirstName =\"\";\n";
            litJavaScript.Text += "var sjPhone =\"\";\n";

            //check whether the address details are empty
            if (a == null)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";

            //check whether the custom details are empty
            if (ChannelPartner.LocationName == null && ChannelPartner.LocationName == string.Empty)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";
            if (ChannelPartner.Info.Count == 0)
                litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";

            //check whether the logo is default
            Xerox.SSOComponents.Models.Image img = null;
            if (ChannelPartner.Images.Count > 0) img = this.ChannelPartner.Images[0];
            if (img == null) litJavaScript.Text += "var sNoDetailsFlag =\"Error\";\n";
           
            litJavaScript.Text += "</script>\n";
            #endregion
        }
        
        /// <summary>
        /// Create UI Buttons - Javascript
        /// </summary>
        private void CreateButtons()
        {
            #region Code
            this.litDialogBoxCancelButton.Text = "<a id=\"DialogBoxCancelButton\" class=\"dialogclosebutton\" href=\"javascript:CancelPreviewDialog();\">" + GetLocalResourceObject("ClosebtnText")  + "</a>";
            #endregion
        }
        
        /// <summary>
        /// This method is now redundant -due to translations before called in resx files. MPE 4/3/2013
        /// </summary>
        private void LoadPageTranslations()
        {
            #region Code
            //tg = GetTranslationGroup("UI", DisplayCountryId, DisplayLanguageId);

            //this.lblWelcomeMsg.Text = tg.GetTranslation("WelcomeToCollateralCreator").ForeignText;
            //this.lblWelcomeDesc.Text = tg.GetTranslation("Page1Info").ForeignText;
            //this.lblRecentActivity.Text = tg.GetTranslation("RecentActivityLabel").ForeignText;
            //this.lblSelectProduct.Text = tg.GetTranslation("SelectProductLabel").ForeignText;
            //this.lblrightpanelHeader.Text = tg.GetTranslation("CustomizeText").ForeignText;
            //this.lblselectedText.Text = tg.GetTranslation("SelectedText").ForeignText;
            #endregion
        }

        /// <summary>
        /// Build UI Product Menu tree list
        /// </summary>
        private void BuildProductTreeView()
        {
            #region Code
            
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

                menuItems = CollateralCreatorManager.GetMenuTreeData(languageCode);
            }
            
            try
            {
                if (this.ChannelPartner.Backpack["productsabletosell"] != null && this.ChannelPartner.Backpack["productsabletosell"] != string.Empty)
                {
                    string[] productsToSell = this.ChannelPartner.Backpack["productsabletosell"].Split(',');

                    //get all the product codes for the selected menutree items
                    List<MenuTree> productNodeItems = CollateralCreatorManager.GetMenuTreeProductCodes();

                    if (productNodeItems.Count > 0)
                    {
                        List<MenuTree> filteredProductNodeItems = new List<MenuTree>();

                        //loop through all the product codes from channelpartner
                        for (int i = 0; i < productsToSell.Length; i++)
                        {
                            foreach (MenuTree mt in productNodeItems)
                            {
                                if (productsToSell[i] == mt.FamilyCode)
                                {
                                    filteredProductNodeItems.Add(mt);
                                }
                            }
                        }

                        if (filteredProductNodeItems.Count > 0)
                        {
                           List<MenuTree> filteredProductMenuItems = GetTranslatedMenuItems(filteredProductNodeItems, menuItems);

                           BuildMenuTree(filteredProductMenuItems);
                        }
                        else
                        {
                            BuildMenuTree(menuItems);
                        }
                    }
                    else
                    {
                        BuildMenuTree(menuItems);
                    }
                }
                else
                {
                    BuildMenuTree(menuItems);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }

            #endregion
        }

        /// <summary>
        /// Method to build menu tree view
        /// </summary>
        /// <param name="menuItems"></param>
        private void BuildMenuTree(List<MenuTree> menuItems)
        {
            //logic for sorting alphabetical order
            //var sortedList = menuItems.OrderBy(x => x.MenuText).ToList();

            StringBuilder output = new StringBuilder(1000);

            if (menuItems.Count > 0)
            {
                output.Append("<div id=\"productoptions\">");
              
                //foreach (var product in sortedList)
                foreach (MenuTree menu in menuItems)
                {
                    //if (product.ParentNodeID == 1)
                    if(menu.IsRoot)
                    {
                        output.Append("<div class=\"product-category\">");
                        output.Append("<img class=\"image\" src=\"images/portal/icons/plus.gif\" alt=\"expand or collapse\" />");
                        output.Append("<span class=\"producttext\" parentnodeid=\"" + menu.ParentNodeID + "\" nodeid=\"" + menu.NodeID + "\">" + menu.MenuText + "</span>");

                        //to display class categorybox category div only once for all the category items
                        int nodecount = 0;
                        //foreach (var category in sortedList)
                        foreach (MenuTree submenu in menuItems)
                        {
                            //if (product.NodeID == category.ParentNodeID)
                            if (submenu.ParentNodeID == menu.NodeID)
                            {
                                //to display class categorybox category div only once for all the category items
                                if (nodecount == 0)
                                    output.Append("<div class=\"categorybox category\">");

                                output.Append("<div class=\"item\">");

                                if (!submenu.IsRoot) 
                                {
                                    output.Append("<img class=\"linkpointer\" src=\"images/navigation/link_pointer_10pt.gif\" alt=\"link_pointer\" />");
                                }

                                output.Append("<span class=\"itemtext\" parentnodeid=\"" + submenu.ParentNodeID + "\" nodeid=\"" + submenu.NodeID + "\">" + submenu.MenuText + "</span>");

                                int subnodecount = 0;
                                //foreach (var subcategory in sortedList)
                                foreach (MenuTree subcategory in menuItems)
                                {
                                    if (subcategory.ParentNodeID == submenu.NodeID)
                                    {
                                        if (subnodecount == 0)
                                            output.Append("<div class=\"subcategorybox subcategory\">");

                                        output.Append("<div class=\"subitem\">");
                                        output.Append("<img class=\"linkpointer\" src=\"images/navigation/link_pointer_10pt.gif\" alt=\"link_pointer\" />");
                                        output.Append("<span class=\"subitemtext\" parentnodeid=\"" + subcategory.ParentNodeID + "\" nodeid=\"" + subcategory.NodeID + "\">" + subcategory.MenuText + "</span>");
                                        output.Append("</div>");

                                        subnodecount++;
                                    }
                                }
                                if (subnodecount != 0)
                                    output.Append("</div>"); //end div for subcategory box
                                
                                output.Append("</div>");

                                nodecount++;
                            }
                        }
                        //to display class categorybox category div only once for all the category items
                        if (nodecount != 0)
                            output.Append("</div>"); //end div for category box

                        output.Append("</div>"); //end div for product-category
                    }
                }
                output.Append("</div>"); //end div for productoptions
            }
            else
            {
                output.Append(GetLocalResourceObject("NoDataText").ToString());
            }

            litSelectProducts.Text = output.ToString();
            
        }

        /// <summary>
        /// Method returns the filtered menu items based on product codes //~VP 09/10/2013
        /// </summary>
        /// <param name="TranslatedMenuItems"></param>
        /// <param name="ProductNodeItems"></param>
        /// <param name="MenuItems"></param>
        private List<MenuTree> GetTranslatedMenuItems(List<MenuTree> productNodeItems, List<MenuTree> menuItems)
        {
            List<MenuTree> filteredProductItems = new List<MenuTree>();
     
            //filter the menu tree items for the product codes
            if (productNodeItems != null && productNodeItems.Count > 0)
            {
                //filter the reduntant nodeID's for the selected productnode items
                var distinctList = productNodeItems.Select(f => f.NodeID).Distinct().ToList();

                //remove all the node items if it is not present in the products able to sell items //~VP 09/10/2013
                foreach(MenuTree mt in menuItems)
                {
                    foreach (var nodeID in distinctList)
                    {
                        if (nodeID == mt.NodeID)
                        {
                            filteredProductItems.Add(mt);                            
                        }
                        else if (nodeID == mt.ParentNodeID)
                        {
                            filteredProductItems.Add(mt);
                            break;
                        }
                    }
                }                
            }

            return filteredProductItems;
        }

        /// <summary>
        /// Custom Javascript popup
        /// </summary>
        /// <param name="message"></param>
        public void ShowAlertMessage(string message)
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
