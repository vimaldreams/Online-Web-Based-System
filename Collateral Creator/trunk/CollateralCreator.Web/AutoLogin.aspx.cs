using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CollateralCreator.Business;
using CollateralCreator.Web;
using Xerox.SSOComponents;
using Xerox.SSOComponents.Data.SqlServer;
using Xerox.SSOComponents.Models;
using System.Configuration;

namespace CollateralCreator.Web
{
    public partial class AutoLogin  : XeroxWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Autologin(ChannelPartner.LoginId);
        }

        private static void Autologin(string loginId)
        {

            ChannelPartnerService service = new ChannelPartnerService(new ChannelPartnerRepository(ConfigurationManager.ConnectionStrings["XeroxPortal"].ToString()));
            ChannelPartner channelPartner = service.Retrieve(loginId);

            var p = new RemotePost(AppSettings.PortalAutoSignInUrl);
            
            //only post if allowed to login into portal
            if (channelPartner == null || channelPartner.Backpack == null ||
               !channelPartner.Backpack.ContainsKey("featuresetregion") ||
               channelPartner.Backpack["featuresetregion"] != "NARS") return;

            if (channelPartner.Backpack.ContainsKey("catalog_group") &&
                       channelPartner.Backpack["catalog_group"].ToLower() == "us_res_dirresp")
            {
                //normal and don't post
            }   
            else
            {
                if (channelPartner.Backpack.Count > 0)
                {
                    foreach (KeyValuePair<string, string> x in
                        channelPartner.Backpack.Where(x => x.Key.ToLower() != "umbrellaapp"))
                    {
                        p.Add(x.Key, x.Value);
                    }
                }       
       
                p.Post();
            }
        }
    }

}