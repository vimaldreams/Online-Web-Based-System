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
    public class ExternalCollateralController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /ExternalCollateral/

        public ActionResult Index(string currentFilter, string searchString, int? page)
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

            var externalcollaterals = unitOfWork.ExternalCollateralRepository.Get(includeProperties: "Template")
                                      .OrderByDescending(e => e.TemplateID);

            //find if a search string has been inputted - this will be the template ID
            if (!String.IsNullOrEmpty(searchString))
            {
                externalcollaterals = externalcollaterals.Where(e => e.Template.Name.ToLower().Contains(searchString.ToLower()))
                             .OrderByDescending(e => e.TemplateID);
                pageSize = externalcollaterals.Count();
            }

            if (pageSize == 0)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
            }
            else
            {
                return View(externalcollaterals.ToPagedList(pageNumber, pageSize));
            }

            #endregion
        }

        //
        // GET: /ExternalCollateral/Details/5

        public ActionResult Details(int id = 0)
        {
            #region code
            ExternalCollateral externalcollateral = unitOfWork.ExternalCollateralRepository.GetByID(id);
            if (externalcollateral == null)
            {
                return HttpNotFound();
            }
            return View(externalcollateral);
            #endregion
        }

        //
        // GET: /ExternalCollateral/Create

        public ActionResult Create(string searchString)
        {
            #region code
            ViewBag.currentFilter = searchString;
            ViewBag.Languages = new SelectList(unitOfWork.ExternalCollateralRepository.context.Languages, "LanguageCode", "LanguageName"); //get drop down list of languages
            ViewBag.Templates = new SelectList(unitOfWork.ExternalCollateralRepository.context.Templates.OrderByDescending(t => t.TemplateID), "TemplateID", "Name"); //get drop down list of templates
            return View();
            #endregion
        }

        //
        // POST: /ExternalCollateral/Create

        [HttpPost]
        public ActionResult Create(ExternalCollateral externalcollateral)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.ExternalCollateralRepository.Insert(externalcollateral);
                    unitOfWork.Save();
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Smart Centre Collateral created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating external collateral template. Please try again");
            }

            ViewBag.Languages = new SelectList(unitOfWork.ExternalCollateralRepository.context.Languages, "LanguageCode", "LanguageName");
            ViewBag.Templates = new SelectList(unitOfWork.ExternalCollateralRepository.context.Templates, "TemplateID", "Name", externalcollateral.TemplateID);
            return View(externalcollateral);
            #endregion
        }

        //
        // GET: /ExternalCollateral/Edit/5

        public ActionResult Edit(int? id, string searchString)
        {
            #region Code

            ExternalCollateral externalcollateral = unitOfWork.ExternalCollateralRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (externalcollateral == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageCode = new SelectList(unitOfWork.ExternalCollateralRepository.context.Languages, "LanguageCode", "LanguageName", externalcollateral.LanguageCode);
            ViewBag.TemplateID = new SelectList(unitOfWork.ExternalCollateralRepository.context.Templates, "TemplateID", "Name", externalcollateral.TemplateID);
            return View(externalcollateral);
            #endregion
        }

        //
        // POST: /ExternalCollateral/Edit/5

        [HttpPost]
        public ActionResult Edit(ExternalCollateral externalcollateral, string searchString)
        {
            #region Code
            try
            {
            if (ModelState.IsValid)
            {
                unitOfWork.ExternalCollateralRepository.Update(externalcollateral);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Smart Centre Collateral updated successfully");
            }
            }
             catch (Exception ex)
             {
                 return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating external collateral template. Please try again");
             }
            ViewBag.LanguageCode = new SelectList(unitOfWork.ExternalCollateralRepository.context.Languages, "LanguageCode", "LanguageName", externalcollateral.LanguageCode);
            ViewBag.TemplateID = new SelectList(unitOfWork.ExternalCollateralRepository.context.Templates, "TemplateID", "Name", externalcollateral.TemplateID);
            return View(externalcollateral);
            #endregion
        }

        //
        // GET: /ExternalCollateral/Delete/5

        public ActionResult Delete(int? id, string searchString)
        {
            #region code
            ExternalCollateral externalcollateral = unitOfWork.ExternalCollateralRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (externalcollateral == null)
            {
                return HttpNotFound();
            }
            return View(externalcollateral);
            #endregion
        }

        //
        // POST: /ExternalCollateral/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region code
            try
            {
                ExternalCollateral externalcollateral = unitOfWork.ExternalCollateralRepository.GetByID(id);
                unitOfWork.ExternalCollateralRepository.Delete(externalcollateral);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Smart Centre Collateral deleted successfully");
            }
             catch (Exception ex)
             {
                 return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting external collateral template. Please try again");
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