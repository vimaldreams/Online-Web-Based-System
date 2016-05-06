using CollateralCreator.SQLProvider;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.Repositories;
using Xerox.SSOComponents.Data.SqlServer;

/// <summary>
/// Class to represent a login page within a Xerox application.
/// </summary>

namespace CollateralCreator.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Globalization;
    using System.Security.Permissions;
    using System.Collections.Specialized;
    using System.Configuration;

    public partial class LoginForm : XeroxWebPage
    {
        #region Member Variables

        private NameValueCollection collection = new NameValueCollection();
        private string queryStringParams { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Fires on page load
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">Any arguments that were passed</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region code
            if (Page.IsPostBack) return;
           
            ChannelPartnerService service = new ChannelPartnerService(new ChannelPartnerRepository(ConfigurationManager.ConnectionStrings["XeroxPortal"].ToString()));

            // add event handlers
            service.SignInSuccess += ServiceSignInSuccess;
            service.SignInFailed += ServiceSignInFailed;
                      
            //check collateral ID if collateral ID goto method a, else drop through
            if (Request["CollateralID"] != null)
            {
                //check query string
                if (CheckQueryString())
                {
                    if (service.SignIn(collection))
                    {
                        //log the query string 
                        CollateralCreatorRepository.Log_SmartCentreQueryString(Request.QueryString.ToString());

                        Response.Redirect("~/Edit.aspx?Type=SmartCentre&" + queryStringParams, true); 
                        //Response.Redirect("~/SCEdit.aspx?Type=SmartCentre&" + queryStringParams, true); 
                    }
                    else
                    {
                        // redirect to not authorised page
                        Response.Redirect(VirtualPathUtility.ToAbsolute("~/shared/NotAuthenticated.aspx"), true);
                    }
                }
            }          
            else //original process
            {

                if (service.SignIn(Request.Form))
                {
                    //get the language/culture code from the incoming language form param
                    if (Request.Form["language"] != null)
                    {
                        //set the correct culture on a session to the correct Xerox language ~MPE
                        Session["CustomCultureCode"] = Request.Form["language"].ToString();                        

                    }

                    Response.Redirect("~/Home.aspx", true);
                                     
                }
                else
                {

                    // redirect to not authorised page
                    Response.Redirect(VirtualPathUtility.ToAbsolute("~/shared/NotAuthenticated.aspx"), true);
                }
            }
            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true after parsing the query string successfully
        /// </summary>
        /// <returns></returns>
        private bool CheckQueryString()
        {
            #region code
            if (!String.IsNullOrEmpty(Request["CollateralID"]))
            {
                queryStringParams += "CollateralID=" + Request["CollateralID"];

                //test for lang
                if (!String.IsNullOrEmpty(Request["LanguageID"]))
                {
                    queryStringParams += "&LanguageID=" + Request["LanguageID"];

                    //test for brand
                    if (!String.IsNullOrEmpty(Request["BrandFlag"]))
                    {
                        queryStringParams += "&BrandFlag=" + Request["BrandFlag"];

                        //test for company ID, but if null ignore for now
                        if (!String.IsNullOrEmpty(Request["CompanyID"]))
                        {
                            queryStringParams += "&CompanyID=" + Request["CompanyID"];
                        }
                        if (!CreateCollection())
                        {
                            //create script block redirect
                            CreateScriptBlockMessage("DataIssue");
                            return false;
                        }
                    }
                    else
                    {
                        //create script block redirect
                        CreateScriptBlockMessage("BrandFlag");
                        return false;
                    }
                }
                else
                {
                    //create script block redirect
                    CreateScriptBlockMessage("LanguageID");
                    return false;
                }
            }
            else
            {
                //create script block redirect
                CreateScriptBlockMessage("CollateralID");
                return false;
            }

            return true;
            #endregion
        }

        /// <summary>
        /// Returns true if the collection is successful
        /// </summary>
        /// <returns></returns>
        private bool CreateCollection()
        {
            #region code
            if (!String.IsNullOrEmpty(Request["ChannelPartnerID"]))
            {
                collection.Add("login_id", "SIGNALS"); //Request["ChannelPartnerID"]

                queryStringParams += "&ChannelPartnerID=" + Request["ChannelPartnerID"];

               
                //collect optional parameters
                if (Request["MenuHierarchy"] != null)
                {
                    queryStringParams += "&MenuHierarchy=" + Request["MenuHierarchy"];
                }
                if (Request["CompanyName"] != null)
                {
                    queryStringParams += "&CompanyName=" + Request["CompanyName"];
                }
                if (Request["CompanyWebsiteURL"] != null)
                {
                    queryStringParams += "&CompanyWebsiteURL=" + Request["CompanyWebsiteURL"];
                }
                if (Request["AddressL1"] != null)
                {
                    queryStringParams += "&AddressL1=" + Request["AddressL1"];
                }
                if (Request["AddressL2"] != null)
                {
                    queryStringParams += "&AddressL2=" + Request["AddressL2"];
                }
                if (Request["AddressL3"] != null)
                {
                    queryStringParams += "&AddressL3=" + Request["AddressL3"];
                }
                if (Request["AddressL4"] != null)
                {
                    queryStringParams += "&AddressL4=" + Request["AddressL4"];
                }
                if (Request["State"] != null)
                {
                    queryStringParams += "&State=" + Request["State"];
                }
                if (Request["ZipCode"] != null)
                {
                    queryStringParams += "&ZipCode=" + Request["ZipCode"];
                }
                if (Request["SalesContactPhoneNumber"] != null)
                {
                    queryStringParams += "&SalesContactPhoneNumber=" + Request["SalesContactPhoneNumber"];
                }
                if (Request["SalesContactEmailAddress"] != null)
                {
                    queryStringParams += "&SalesContactEmailAddress=" + Request["SalesContactEmailAddress"];
                }

                return true;
            }
            else
            {
                //create script block redirect
                CreateScriptBlockMessage("ChannelPartnerID");
                return false;
            }
            #endregion
        }
        /// <summary>
        /// Add a javascript message box to alert users that there are missing parameters
        /// </summary>
        /// <param name="missingQueryValue"></param>
        private void CreateScriptBlockMessage(string missingQueryValue)
        {
            #region code
            //get default message to show from the global resx file
            string message = GetGlobalResourceObject("MainText", "MissingQueryStringParamter").ToString().Replace("{0}", missingQueryValue);

            //inject a javascript popup if mandatory fields are missing
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ThrowAlertError", "alert('" + message + "'); window.location.href = 'http://www.ektron.co.uk/';", true);
            #endregion
        }

        /// <summary>
        /// Event handler for successful sign on of a user
        /// </summary>
        /// <param name="loginId">The login id of a user</param>
        private static void ServiceSignInSuccess(string loginId)
        {
            #region code
            //if (log.IsErrorEnabled)
            //{
            //    log.Info(string.Format("Login success: {0}", loginId));
            //}
            #endregion
        }

        /// <summary>
        /// Event handler for failed sign on of a user
        /// </summary>
        /// <param name="backpack">The backpack that was passed to the page (if any)</param>
        private static void ServiceSignInFailed(NameValueCollection backpack)
        {
            #region code
            // Only kick off logging if there is info to log!
            if (backpack.Count == 0) return;

            System.Text.StringBuilder b = new System.Text.StringBuilder();


            // Write for any trace handlers
            foreach (string myKey in backpack.AllKeys)
            {
                if (myKey == null) continue;

                System.Diagnostics.Trace.WriteLine(myKey + ":" + backpack[myKey]);
                System.Diagnostics.Debug.WriteLine(myKey + ":" + backpack[myKey]);

                b.AppendFormat("Key:{0}:{1}", myKey, backpack[myKey]).AppendLine();

                //if (log.IsErrorEnabled)
                //{
                //    log.InfoFormat("Key:{0}:{1}", myKey, backpack[myKey]);
                //}
            }

            // Try and get the login id to log a failed error against user
            if (!string.IsNullOrEmpty(backpack["login_id"]))
            {
                string loginId = backpack["login_id"];
              //  EventLogManager.Instance().AddEvent(loginId, EventLogType.FailedSignIn, 0, "", DateTime.Now);
            }

            // MailHelper.SendSignInFailMail(b.ToString());
            #endregion
        }

        #endregion 
    }
}
