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
    public class FontUsageController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /FontUsage/

        public ActionResult Index(string currentFilter, string searchString, int? page, int? textareaID, int? areaID, int? pageID, int? templateID)
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

            if (textareaID == null && areaID == null && pageID == null && templateID == null)
            {
                var fontusages = unitOfWork.FontUsageRepository
                                            .Get(filter: f => f.DocumentID == null && f.PageID == null,
                                                orderBy: q => q.OrderByDescending(f => f.FontID),
                                                includeProperties: "Font");


                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    int templateId = unitOfWork.FontUsageRepository.context.Templates.Where(t => t.Name.ToLower().Contains(searchString.ToLower())).Select(t => t.TemplateID).FirstOrDefault();

                    fontusages = unitOfWork.FontUsageRepository
                                            .Get(filter: f => f.DocumentID == null && f.PageID == null && f.TemplateID == templateId,
                                                orderBy: q => q.OrderByDescending(f => f.FontID),
                                                includeProperties: "Font");
                    pageSize = fontusages.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(fontusages.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var fontusages = unitOfWork.FontUsageRepository
                                           .Get(filter: f => f.DocumentID == null && f.PageID == null && f.TemplateID == templateID && f.TextAreaID == textareaID,
                                               orderBy: q => q.OrderByDescending(f => f.FontID),
                                               includeProperties: "Font");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    int templateId = unitOfWork.FontUsageRepository.context.Templates.Where(t => t.Name.ToLower().Contains(searchString.ToLower())).Select(t => t.TemplateID).FirstOrDefault();

                    fontusages = unitOfWork.FontUsageRepository
                                           .Get(filter: f => f.DocumentID == null && f.PageID == null && f.TemplateID == templateId,
                                               orderBy: q => q.OrderByDescending(f => f.FontID),
                                               includeProperties: "Font");
                    pageSize = fontusages.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(fontusages.ToPagedList(pageNumber, pageSize));
                }
            }
            #endregion
        }

        //
        // GET: /FontUsage/Details/5

        public ActionResult Details(int id = 0)
        {
            #region code
            FontUsage fontusage = unitOfWork.FontUsageRepository.GetByID(id);
            if (fontusage == null)
            {
                return HttpNotFound();
            }
            return View(fontusage);
            #endregion
        }

        //
        // GET: /FontUsage/Create

        public ActionResult Create(string searchString)
        {
            #region code

            ViewBag.FontID = new SelectList(unitOfWork.FontUsageRepository.context.Fonts.OrderByDescending(f => f.FontID), "FontID", "FontID").ToList();
            ViewBag.currentFilter = searchString;

            var templates = unitOfWork.FontUsageRepository.context.Templates.ToList().OrderByDescending(t => t.TemplateID);

            ViewBag.Templates = new SelectList(templates, "TemplateID", "Name");

            var fonts = unitOfWork.FontUsageRepository.context.TextAreas
                        .Include(c => c.CustomizableArea)
                        .Include(c => c.CustomizableArea.Page).Include(c => c.CustomizableArea.Page.Template)
                        .Where(c => (c.CustomizableArea.Page.DocumentID == null) && (c.CustomizableArea.Page.TemplateID != null))
                        .OrderByDescending(t => t.TextAreaID).ToList();

            ViewBag.TextAreaID = new SelectList(fonts, "TextAreaID", "TextAreaID");

            return View();
            #endregion
        }

        //
        // POST: /FontUsage/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FontUsage fontusage)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.FontUsageRepository.Insert(fontusage);
                    unitOfWork.Save();
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "FontUsage created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating fontusage area. Please try again");
            }
            ViewBag.FontID = new SelectList(unitOfWork.FontUsageRepository.context.Fonts, "FontID", "Name", fontusage.FontID);
            return View(fontusage);
            #endregion
        }

        //
        // GET: /FontUsage/Edit/5

        public ActionResult Edit(int? id, string searchString)
        {
            #region code
            FontUsage fontusage = unitOfWork.FontUsageRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (fontusage == null)
            {
                return HttpNotFound();
            }
            
            //ViewBag.FontID = new SelectList(unitOfWork.FontUsageRepository.context.Fonts, "FontID", "Name", fontusage.FontID);

            var fonts = unitOfWork.FontUsageRepository.context.Fonts.ToList();

            ViewBag.FontName = (from f in fonts 
                                where f.FontID == fontusage.FontID
                                select f.Name).FirstOrDefault();


            return View(fontusage);
            #endregion
        }

        //
        // POST: /FontUsage/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FontUsage fontusage, string searchString)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.FontUsageRepository.Update(fontusage);
                    unitOfWork.Save();

                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "FontUsage updated successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating fontusage area. Please try again");
            }
            ViewBag.FontID = new SelectList(unitOfWork.FontUsageRepository.context.Fonts, "FontID", "Name", fontusage.FontID);
            return View(fontusage);
            #endregion
        }

        //
        // GET: /FontUsage/Delete/5

        public ActionResult Delete(int? id, string searchString)
        {
            #region code
            FontUsage fontusage = unitOfWork.FontUsageRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (fontusage == null)
            {
                return HttpNotFound();
            }
            return View(fontusage);
            #endregion
        }

        //
        // POST: /FontUsage/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region code
            try
            {
                FontUsage fontusage = unitOfWork.FontUsageRepository.context.FontUsages.Find(id);
                unitOfWork.FontUsageRepository.Delete(fontusage);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "FontUsage deleted successfully");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting fontusage area. Please try again");
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