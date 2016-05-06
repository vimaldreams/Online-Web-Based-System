using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Transactions;

using System.Configuration;
using System.Web.Configuration;
using System.Web.Security;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;
using CollateralCreatorAdminWeb.Extensions;
using CollateralCreatorAdminWeb.DAL;

namespace CollateralCreatorAdminWeb.Controllers
{
    public class HomeController : Controller
    {
        #region Member Variables

        private List<Product_MenuTree> productMenuTreeList = new List<Product_MenuTree>();
        private Product_MenuTree productMenuTree = null;

        #endregion

        #region Entities
        private UnitOfWork unitOfWork = new UnitOfWork();
        #endregion

        /// <summary>
        ///  Index page GET action
        /// </summary>
        /// <returns>MVC view</returns>
        public ActionResult Index()
        {
            #region code

            //EncryptConnectionString();

            return View();

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void EncryptConnectionString()
        {
            var config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            var section = config.GetSection("secureAppSettings");
            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");                
            }
            else 
            {
                section.SectionInformation.UnprotectSection();
            }
            config.Save();
        }

        /// <summary>
        /// Product Admin GET action
        /// </summary>
        /// <returns>MVC view</returns>
        [HttpGet]
        public ActionResult ProductAdmin()
        {
            #region code

            var selectList = from m in unitOfWork.MenuTreeRepository.context.MenuTrees
                                .Where(m => m.ParentNodeID == 1 && m.LanguageID == 23)
                                .OrderByDescending(m => m.NodeID)                              
                                 select m;

            List<MenuTree> queryList = new List<MenuTree>();

            foreach (MenuTree list in selectList)
            {
                list.TranslationKey = Resource(list.TranslationKey);
                queryList.Add(list);
            }

            ViewBag.MenuTreeList = new SelectList(queryList, "NodeID", "TranslationKey");

            ViewBag.FamilyCodes = new MultiSelectList(unitOfWork.MenuTreeRepository.context.Product_Family, "ProductCode", "FamilyCode");

            ViewBag.SelectedFamilyCodes = new MultiSelectList(new[] { "" }, "", "");

            return View();

            #endregion
        }

        /// <summary>
        /// Product Admin POST action
        /// </summary>
        /// <param name="nodeID">Node ID</param>
        /// <param name="familyCodes">Family code</param>
        /// <returns>MVC action result</returns>
        [HttpPost]
        public ActionResult ProductAdmin(int nodeID, string familyCodes)
        {
            #region code

            //split the Json family codes into entities           
            foreach (string familyCode in familyCodes.Split(','))
            {
                productMenuTree = new Product_MenuTree();               
                productMenuTree.NodeID = nodeID;
                productMenuTree.FamilyCode = familyCode;
             
                productMenuTreeList.Add(productMenuTree);                
            }            
          
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (Product_MenuTree productMenuTree in productMenuTreeList)
                    {
                        unitOfWork.ProductMenuTreeRepository.Insert(productMenuTree);
                        unitOfWork.Save();
                    }                    
                }

                return Json(new { message = "Familycodes saved successfully for the selected product." });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
                
            #endregion
        }             

        /// <summary>
        /// Method to get the translation text from resx files
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string Resource(string key)
        {
            #region code
            switch (key)
            {
                case "MenuTreeSuppliesOffer":
                    return Resources.Menu.MenuTreeSuppliesOffer.ToString();
                case "MenuTreeConnectKeyWC5800":
                    return Resources.Menu.MenuTreeConnectKeyWC5800.ToString();
                case "MenuTreeCleanUpCashIn":
                    return Resources.Menu.MenuTreeCleanUpCashIn.ToString();
                case "MenuTreeColorQube8700":
                    return Resources.Menu.MenuTreeColorQube8700.ToString();
                case "MenuTreeColorQube8900":
                    return Resources.Menu.MenuTreeColorQube8900.ToString();
                case "MenuTreeColorQube9301-03":
                    return Resources.Menu.MenuTreeColorQube9301_03.ToString();
                case "MenuTreeConnectKey":
                    return Resources.Menu.MenuTreeConnectKey.ToString();
                case "MenuTreeeConcierge":
                    return Resources.Menu.MenuTreeeConcierge.ToString();
                case "MenuTreeManagedPrintServices":
                    return Resources.Menu.MenuTreeManagedPrintServices.ToString();
                case "MenuTreeMultifunctionPrinter":
                    return Resources.Menu.MenuTreeMultifunctionPrinter.ToString();
                case "MenuTreePhaser4600-20":
                    return Resources.Menu.MenuTreePhaser4600_20.ToString();
                case "MenuTreePhaser6010WC6015":
                    return Resources.Menu.MenuTreePhaser6010WC6015.ToString();
                case "MenuTreePhaser6500WC6505":
                    return Resources.Menu.MenuTreePhaser6500WC6505.ToString();
                case "MenuTreePhaser6600WC6605":
                    return Resources.Menu.MenuTreePhaser6600WC6605.ToString();
                case "MenuTreePhaser6700":
                    return Resources.Menu.MenuTreePhaser6700.ToString();
                case "MenuTreePhaser7100":
                    return Resources.Menu.MenuTreePhaser7100.ToString();
                case "MenuTreePhaser7800":
                    return Resources.Menu.MenuTreePhaser7800.ToString();
                case "MenuTreeSolidInk":
                    return Resources.Menu.MenuTreeSolidInk.ToString();
                default:
                    return string.Empty;
            }
            #endregion
        }

        /// <summary>
        /// Edit admin page GET action
        /// </summary>
        /// <returns></returns>
        public ActionResult EditAdmin()
        {
            #region Code
            return View();
            #endregion
        }
    }
}
