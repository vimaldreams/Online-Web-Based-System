using System;
using System.Collections;
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
using CollateralCreatorAdminWeb.DAL;

namespace CollateralCreatorAdminWeb.Controllers
{
    public class PageController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        /// <summary>
        /// Get List of pages
        /// </summary>
        /// <returns></returns>
        /// <param name="currentFilter">Current search filter</param>
        /// <param name="page">Page ID</param>
        /// <param name="searchString">Any search string from search text box</param>
        public ActionResult Index(string currentFilter, string searchString, int? page, int? templateID)
        {
            #region Code
            int pageSize = 20;
            int pageNumber = (page ?? 1);
           
            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                pageNumber = 1;
            }
            ViewBag.currentFilter = searchString;



            if (templateID == null)
            {
                var pages = unitOfWork.PageRepository
                            .Get(filter: p => (p.DocumentID == null) && (p.TemplateID != null), 
                                 orderBy: q => q.OrderByDescending(p => p.PageID), 
                                 includeProperties: "Document, Template");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    pages = unitOfWork.PageRepository
                            .Get(filter: p => (p.DocumentID == null) && (p.TemplateID != null) && p.Template.Name.ToLower().Contains(searchString.ToLower()), 
                                orderBy: q => q.OrderByDescending(p => p.PageID), 
                                includeProperties: "Document, Template");
                    pageSize = pages.Count();
                }
                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(pages.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var pages = unitOfWork.PageRepository.Get(filter: p => (p.DocumentID == null) && (p.TemplateID != null) && (p.TemplateID == templateID), 
                                                          orderBy: q => q.OrderByDescending(p => p.PageID), 
                                                          includeProperties: "Document, Template");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    pages = unitOfWork.PageRepository.Get(filter: p => p.Template.Name.ToLower().Contains(searchString.ToLower()), 
                                                          orderBy: q => q.OrderByDescending(p => p.PageID), 
                                                          includeProperties: "Document, Template");
                    pageSize = pages.Count();
                }
                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(pages.ToPagedList(pageNumber, pageSize));
                }
            }           
           
           
            #endregion
        }

        //
        // GET: /Page/Details/5
        public ActionResult Details(int id = 0)
        {
            #region code
            Page page = unitOfWork.PageRepository.GetByID(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
            #endregion
        }

       /// <summary>
       /// Create - get
       /// </summary>
       /// <returns></returns>
        public ActionResult Create(string searchString)
        {
            #region Code

            ViewBag.currentFilter = searchString;
            ViewBag.Templates = new SelectList(unitOfWork.PageRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name"); //get drop down list for templates
   
            return View();
            #endregion
        }

       /// <summary>
       /// Create -save
       /// </summary>
       /// <param name="page"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Page page, int NoOfPages)
        {
            #region Code
            try
            {
                if (ModelState.IsValid)
                {
                    for (int i = 1; i <= NoOfPages; i++)
                    {
                        page.PageNumber = i;
                        unitOfWork.PageRepository.Insert(page);
                        unitOfWork.Save();
                    }                   

                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Page created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating page. Please try again");
            }

            ViewBag.Templates = new SelectList(unitOfWork.PageRepository.context.Languages, "TemplateID", "Name"); //get drop down list for templates
            return View(page);
            #endregion
        }

        public ActionResult CreateCustomArea(int? pageID, string searchString)
        {
            #region code
            ViewBag.currentFilter = searchString;

            ViewBag.PageID = new SelectList(unitOfWork.CustomAreaRepository.context.Pages.Include(p => p.Template).OrderByDescending(p => p.PageID), "PageID", "PageID");
            return View();
            #endregion
        }

        //
        // POST: /CustomArea/Create

        [HttpPost]
        public ActionResult CreateCustomArea(CustomizableArea customizablearea)
        {
            #region
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.CustomAreaRepository.Insert(customizablearea);
                    unitOfWork.Save();
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Custom Area created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating cusotm area. Please try again");
            }

            ViewBag.PageID = new SelectList(unitOfWork.CustomAreaRepository.context.Pages, "PageID", "PageID", customizablearea.PageID);
            return View(customizablearea);
            #endregion
        }

       /// <summary>
       /// Edit - get
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult Edit(int? id, string searchString)
        {
            #region code
            Page page = unitOfWork.PageRepository.Get(includeProperties: "Template").First(p => p.PageID == id);
            ViewBag.currentFilter = searchString;
            if (page == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentID = new SelectList(unitOfWork.PageRepository.context.Documents, "DocumentID", "ChannelPartnerLoginID", page.DocumentID);
            ViewBag.TemplateID = new SelectList(unitOfWork.PageRepository.context.Templates, "TemplateID", "Name", page.Template.Name);
            return View(page);
            #endregion
        }

       /// <summary>
       /// Edit -save
       /// </summary>
       /// <param name="page"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Page page, string searchString)
        {
            #region Code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.PageRepository.Update(page);
                    unitOfWork.Save();
                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Page updated successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating page. Please try again");
            }
            ViewBag.Templates = new SelectList(unitOfWork.PageRepository.context.Languages, "TemplateID", "Name", page.TemplateID); //get drop down list for templates
        
            return View(page);
            #endregion
        }

       /// <summary>
       /// Delete - get
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult Delete(int? id, string searchString)
        {
            #region Code
            Page page = unitOfWork.PageRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
            #endregion
        }

        /// <summary>
        /// Delete - save
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region Code
            try
            {
                Page page = unitOfWork.PageRepository.GetByID(id);
                unitOfWork.PageRepository.Delete(page);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Page deleted successfully"); 
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting page. Please try again");
            }
            #endregion
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}