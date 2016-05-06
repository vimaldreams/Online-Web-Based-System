using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.IO;
using System.Configuration;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;
using CollateralCreatorAdminWeb.Extensions;
using CollateralCreatorAdminWeb.DAL;
using System.Data.Entity.Infrastructure;

namespace CollateralCreatorAdminWeb.Controllers
{
    public class MenuTreeTemplateBrandController : Controller
    {
        #region Member Variables

        private MenuTreeTemplate menutemplate = null;
        private TemplateButton templatebutton = null;

        #endregion
        
        #region Entities
         private UnitOfWork unitOfWork = new UnitOfWork();
        #endregion
        
        public class MenuTreeTemplate 
        {
            public int NodeID { get; set; }
            public int TemplateID { get; set; }
        }

        //
        // GET: /MenuTreeTemplateBrand/

        public ActionResult Index()
        {
            #region code
            return View();
            #endregion
        }

        //
        // GET: /MenuTreeTemplateBrand/MenuTreeTemplateAdmin
       
        public ActionResult MenuTreeTemplateAdmin()
        {
            #region code
            ViewBag.Nodes = new MultiSelectList(unitOfWork.MenuTreeRepository.context.MenuTrees.OrderByDescending(m => m.NodeID), "NodeID", "TranslationKey");

            ViewBag.Templates = new MultiSelectList(unitOfWork.MenuTreeRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name");

            ViewBag.TemplateNames = new MultiSelectList(unitOfWork.MenuTreeRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name");

            return View(new RadioViewModel{ Brand = false });
            #endregion
        }

        //
        // POST: /MenuTreeTemplateBrand/

        [HttpPost]
        public ActionResult MenuTreeTemplateAdmin(string menuTreeNodes, string templates)
        {
            #region code
            List<MenuTreeTemplate> menutemplates = new List<MenuTreeTemplate>();
            
            List<int> NodeIds = new List<int>(menuTreeNodes.Split(',').Select(int.Parse));
                        var parentNode = NodeIds[NodeIds.Count - 1];
            NodeIds.Remove(parentNode);

            //split the Json menuTreeNodes into entities           
            foreach (var template in templates.Split(','))
            {
                menutemplate = new MenuTreeTemplate();
                menutemplate.NodeID = parentNode;
                menutemplate.TemplateID = Convert.ToInt16(template);
                menutemplates.Add(menutemplate);

                foreach(int node in NodeIds)
                {
                    menutemplate = new MenuTreeTemplate();
                    menutemplate.NodeID = node;
                    menutemplate.TemplateID = Convert.ToInt16(template);
                    menutemplates.Add(menutemplate);
                    NodeIds.Remove(node);
                    break;
                }    
            }

            try
            {
                //Inserting using ExecuteStoreCommand
                foreach (MenuTreeTemplate mtt in menutemplates)
                {
                    unitOfWork.MenuTreeRepository.context.Database.ExecuteSqlCommand("Insert into MenuTreeTemplate(NodeID,TemplateID) values(" + mtt.NodeID + ", " + mtt.TemplateID + ")");
                }

                //insert data into templatebutton table - partner brand
                foreach (var template in templates.Split(','))
                {
                    templatebutton = new TemplateButton();

                    templatebutton.TemplateID = Convert.ToInt16(template);
                    templatebutton.ChannelPartnerLoginID = "";
                    templatebutton.DateCustomized = DateTime.Now;
                    templatebutton.IsCustomized = false;
                    templatebutton.IsPartnerBrand = true;
                    templatebutton.NodeID = parentNode;

                    unitOfWork.TemplateButtonRepository.Insert(templatebutton);
                    unitOfWork.Save(); 
                }
                //insert data into templatebutton table - xerox brand
                foreach (var template in templates.Split(','))
                {
                    templatebutton = new TemplateButton();

                    templatebutton.TemplateID = Convert.ToInt16(template);
                    templatebutton.ChannelPartnerLoginID = "";
                    templatebutton.DateCustomized = DateTime.Now;
                    templatebutton.IsCustomized = false;
                    templatebutton.IsPartnerBrand = false;
                    templatebutton.NodeID = parentNode;

                    unitOfWork.TemplateButtonRepository.Insert(templatebutton);
                    unitOfWork.Save();
                }        

                return Json(new { message = "MenuTree-Template matrix saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error in creating menu tree-template matrix. Please try again later!!" });
            }
            #endregion
        }

        [HttpPost]
        public ActionResult TemplateBrandAdmin(RadioViewModel model, FormCollection collection)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    int templateID = Convert.ToInt16(collection["TemplateNames"]);

                    //insert template partnerbranded table data
                    TemplatePartnerBranded templatebrand = new TemplatePartnerBranded();
                    templatebrand.TemplateID = templateID;
                    templatebrand.PartnerBranded = model.Brand;
                    templatebrand.PartNumber = collection["partNumber"];
                    
                    //save template to the directory
                    SaveUploadedFile(Request.Files["templateFile"], "pdf", templateID, model.Brand, "PDFAppRootPath");

                    //save thumbnail to the directory
                    if (model.Brand)
                    {
                        SaveUploadedFile(Request.Files["thumbnailFile"], "thumbnail", templateID, model.Brand, "PartnerAppRootPath");
                    }
                    else
                    {
                        SaveUploadedFile(Request.Files["thumbnailFile"], "thumbnail", templateID, model.Brand, "XeroxAppRootPath");
                    }

                    unitOfWork.TemplateBrandRepository.Insert(templatebrand);
                    unitOfWork.Save();
                }
                return RedirectToAction("MenuTreeTemplateAdmin").WithNotification(NotificationStatus.Success, "Template brand created successfully");
                //return Json(new { message = "Template brand saved successfully" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MenuTreeTemplateAdmin").WithNotification(NotificationStatus.Error, "Error in creating template brand. Please try again later!!");
                //return Json(new { message = "Error in creating template brand. Please try again later!!" });
            }

            ViewBag.Nodes = new MultiSelectList(unitOfWork.MenuTreeRepository.context.MenuTrees.OrderByDescending(m => m.NodeID), "NodeID", "TranslationKey");
            ViewBag.Templates = new MultiSelectList(unitOfWork.TemplateRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name");
            ViewBag.TemplateNames = new MultiSelectList(unitOfWork.TemplateRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name");
            return View(model);
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="templateID"></param>
        /// <param name="brand"></param>
        /// <param name="AppRootPath"></param>
        public void SaveUploadedFile(HttpPostedFileBase file, string type, int templateID, bool brand, string AppRootPath)
        {
            #region code
            if (file != null && file.ContentLength > 0 && !string.IsNullOrWhiteSpace(file.FileName))
            {
                // Save file
                string fileName = GetOutputPath(file.FileName, type, templateID, brand, AppRootPath);

                file.SaveAs(fileName);
                file.InputStream.Dispose();
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="templateID"></param>
        /// <param name="brand"></param>
        /// <param name="AppRootPath"></param>
        /// <returns></returns>
        public string GetOutputPath(string fileName, string type, int templateID, bool brand, string AppRootPath)
        {
            #region code
            if (fileName == null)
            {
                return null;
            }

            string Path = string.Empty;

            if (type == "pdf")
            {
                if (brand)
                {
                    Path = string.Concat(GetDataDirectory(AppRootPath), fileName.Replace(fileName, "Template_" + templateID + "_pb.pdf"));
                }
                else
                {
                    Path = string.Concat(GetDataDirectory(AppRootPath), fileName.Replace(fileName, "Template_" + templateID + ".pdf"));
                }
            }
            else 
            {
                Path = string.Concat(GetDataDirectory(AppRootPath), fileName.Replace(fileName, "thumbnail_" + templateID + ".jpg"));
            }

            try
            {
                FileInfo fileInfo = new FileInfo(Path);

                if (fileInfo.Directory == null)
                {
                    return null;
                }

                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                return Path;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return null;
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppRootPath"></param>
        /// <returns></returns>
        public string GetDataDirectory(string AppRootPath)
        {
            #region code
            string baseDirectory = ConfigurationManager.AppSettings[AppRootPath];

            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            return baseDirectory;
            #endregion
        }
      
    }
}
