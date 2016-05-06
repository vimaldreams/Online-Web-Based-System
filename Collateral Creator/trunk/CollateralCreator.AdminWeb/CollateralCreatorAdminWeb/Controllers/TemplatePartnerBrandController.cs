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
using CollateralCreatorAdminWeb.DAL;

namespace CollateralCreatorAdminWeb.Controllers
{
    public class TemplatePartnerBrandController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /TemplatePartnerBrand/

        public ActionResult Index(string currentFilter, string searchString, int? page, int? templateID)
        {
            #region code
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
                var templatepartnerbrandeds = unitOfWork.TemplateBrandRepository
                                              .Get(orderBy: q => q.OrderByDescending(t => t.TemplateID), 
                                                   includeProperties: "Template");

                if (!String.IsNullOrEmpty(searchString))
                {
                    templatepartnerbrandeds = unitOfWork.TemplateBrandRepository
                                              .Get(filter: t => (t.Template.Name.ToLower().Contains(searchString.ToLower())), 
                                                   orderBy: q => q.OrderByDescending(t => t.TemplateID),
                                                   includeProperties: "Template");

                    pageSize = templatepartnerbrandeds.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(templatepartnerbrandeds.ToPagedList(pageNumber, pageSize));
                }
            }
            else 
            {
                var templatepartnerbrandeds = unitOfWork.TemplateBrandRepository
                                              .Get(filter: t => (t.TemplateID == templateID),
                                                   orderBy: q => q.OrderByDescending(t => t.TemplateID),
                                                   includeProperties: "Template");

                if (!String.IsNullOrEmpty(searchString))
                {
                    templatepartnerbrandeds = unitOfWork.TemplateBrandRepository
                                              .Get(filter: t => (t.Template.Name.ToLower().Contains(searchString.ToLower()) && t.TemplateID == templateID),
                                                   orderBy: q => q.OrderByDescending(t => t.TemplateID),
                                                   includeProperties: "Template");
                    pageSize = templatepartnerbrandeds.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(templatepartnerbrandeds.ToPagedList(pageNumber, pageSize));
                }
            }                       
            
            #endregion
        }

        //
        // GET: /TemplatePartnerBrand/Details/5

        public ActionResult Details(int? id)
        {
            #region code
            TemplatePartnerBranded templatepartnerbranded = unitOfWork.TemplateBrandRepository.GetByID(id);
            if (templatepartnerbranded == null)
            {
                return HttpNotFound();
            }
            return View(templatepartnerbranded);
            #endregion
        }

        //
        // GET: /TemplatePartnerBrand/Create

        public ActionResult Create(string searchString)
        {
            #region code
            ViewBag.currentFilter = searchString;
            ViewBag.Templates = new SelectList(unitOfWork.TemplateBrandRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name"); //get drop down list for templates
            return View();
            #endregion
        }

        //
        // POST: /TemplatePartnerBrand/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TemplatePartnerBranded templatepartnerbranded)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.TemplateBrandRepository.Insert(templatepartnerbranded);
                    unitOfWork.Save();
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Template-PartnerBrand created successfully");
                }                
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating template partnerbrand. Please try again");
            }

            ViewBag.Templates = new SelectList(unitOfWork.TemplateBrandRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name"); //get drop down list for templates
            return View(templatepartnerbranded);

            #endregion
        }

        //
        // GET: /TemplatePartnerBrand/Edit/5

        public ActionResult Edit(int? id, bool brand, string searchString)
        {
            #region code
            TemplatePartnerBranded templatepartnerbranded = unitOfWork.TemplateBrandRepository.GetByID(id, brand);
            ViewBag.currentFilter = searchString;
            if (templatepartnerbranded == null)
            {
                return HttpNotFound();
            }
            ViewBag.TemplateID = new SelectList(unitOfWork.TemplateBrandRepository.context.Templates, "TemplateID", "Name", templatepartnerbranded.TemplateID);
            return View(templatepartnerbranded);
            #endregion
        }

        //
        // POST: /TemplatePartnerBrand/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TemplatePartnerBranded templatepartnerbranded, string searchString)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.TemplateBrandRepository.Update(templatepartnerbranded);
                    unitOfWork.Save();
                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Template-PartnerBrand updated successfully"); 
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating template partner brand. Please try again");
            }
            ViewBag.TemplateID = new SelectList(unitOfWork.TemplateBrandRepository.context.Templates, "TemplateID", "Name", templatepartnerbranded.TemplateID);
            return View(templatepartnerbranded);
            #endregion
        }

        //
        // GET: /TemplatePartnerBrand/Delete/5

        public ActionResult Delete(int? id, bool brand, string searchString)
        {
            #region code

            TemplatePartnerBranded templatepartnerbranded = unitOfWork.TemplateBrandRepository.GetByID(id, brand);
            ViewBag.currentFilter = searchString;
            if (templatepartnerbranded == null)
            {
                return HttpNotFound();
            }
            return View(templatepartnerbranded);                
          
            #endregion
        }

        //
        // POST: /TemplatePartnerBrand/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, bool brand, string searchString)
        {
            #region code
            try
            {
                TemplatePartnerBranded templatepartnerbranded = unitOfWork.TemplateBrandRepository.GetByID(id, brand);
                unitOfWork.TemplateBrandRepository.Delete(templatepartnerbranded);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Template-PartnerBrand deleted successfully");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting template partner brand. Please try again");
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