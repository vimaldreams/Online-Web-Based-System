using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;
using CollateralCreatorAdminWeb.Extensions;
using CollateralCreatorAdminWeb.ViewModels;
using CollateralCreatorAdminWeb.ViewModelBuilder;

namespace CollateralCreatorAdminWeb.Controllers
{
    /// <summary>
    /// Template controller
    /// </summary>
    public class TemplateController : Controller
    {
        #region Member variables

        private XeroxCCToolEntities db = new XeroxCCToolEntities();
        private CreateTemplateVieweModelBuilder createModel = new CreateTemplateVieweModelBuilder();

        #endregion

        /// <summary>
        /// Get list view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            #region code
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.TempIDSortParm = String.IsNullOrEmpty(sortOrder) ? "TemplateID" : ""; 

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                pageNumber = 1;
            }
            ViewBag.currentFilter = searchString;
          
            //find if a search string has been inputted - this will be the template ID
            if (!String.IsNullOrEmpty(searchString))
            {
                var templates = db.Templates.Where(t => t.Name.ToLower().Contains(searchString.ToLower().Trim())).OrderByDescending(t => t.TemplateID);
                pageSize = templates.Count();
                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                return View(templates.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var templates = db.Templates.ToList().OrderByDescending(t => t.TemplateID);
                return View(templates.ToPagedList(pageNumber, pageSize));
            }

            #endregion
        }
        
        /// <summary>
        /// Get details view - not in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            #region Code
            Template template = db.Templates.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
            #endregion
        }

       /// <summary>
       /// Create template - view
       /// </summary>
       /// <returns></returns>
        public ActionResult Create()
        {
            #region Code
            //create a base template creation view model
            CreateTemplateViewModel templateView = createModel.Build();
            return View(templateView);
            #endregion
        }

        public ActionResult CustomArea()
        {
            return View();
        }

        /// <summary>
        /// Create template - save
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Template template)
        {
            #region Code
            try
            {
                if (ModelState.IsValid)
                {                   
                    template.DateCreated = DateTime.Now;
                    template.DateModified = DateTime.Now;
                    //unitOfWork.TemplateRepository.Insert(template);
                    //unitOfWork.Save();

                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Template created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating template. Please try again");
            }
            return View(template);
            #endregion
        }
       
       /// <summary>
       /// Edit template - get
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult Edit(int? id, string searchString)
        {
            #region Code
          
           
            return View();
            #endregion
        }

        /// <summary>
        /// Edit template - save
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Template template, string searchString)
        {
            #region Code
            try
            {
                if (ModelState.IsValid)
                {
                    template.DateModified = DateTime.Now;
                    //unitOfWork.TemplateRepository.Update(template);
                    //unitOfWork.Save();
                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Template updated successfully"); 
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating template. Please try again");
            }
            return View(template);
            #endregion
        }

        /// <summary>
        /// Delete template - view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id, string searchString)
        {
            #region code
            Template template = db.Templates.Find(id);
            ViewBag.currentFilter = searchString;

            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
            #endregion
        }

       
        /// <summary>
        /// Delete template - save
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region Code
            try
            {
              
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Template deleted successfully");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting template. Please try again");
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }


    }
}