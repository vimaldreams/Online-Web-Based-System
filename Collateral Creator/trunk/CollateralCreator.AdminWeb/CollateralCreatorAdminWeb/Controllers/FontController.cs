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
    public class FontController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private Dictionary<string, int> colorCodes = new Dictionary<string, int>();

        //
        // GET: /Font/

        public ActionResult Index(string currentFilter, string searchString, int? page, int? fontID, int? templateID)
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

            if (fontID == null && templateID == null)
            {
                var fonts = unitOfWork.FontRepository
                                    .Get(filter: f => f.FontUsage.DocumentID == null,
                                        orderBy: q => q.OrderByDescending(f => f.FontID),
                                        includeProperties: "FontType,FontUsage");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    int templateId = unitOfWork.FontRepository.context.Templates.Where(t => t.Name.ToLower().Contains(searchString.ToLower())).Select(t => t.TemplateID).FirstOrDefault();

                    fonts = unitOfWork.FontRepository
                                        .Get(filter: f => f.FontUsage.DocumentID == null && f.FontUsage.TemplateID == templateId,
                                            orderBy: q => q.OrderByDescending(f => f.FontID),
                                            includeProperties: "FontType,FontUsage");
                    pageSize = fonts.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(fonts.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var fonts = unitOfWork.FontRepository
                                  .Get(filter: f => f.FontUsage.DocumentID == null && f.FontID == fontID,
                                      orderBy: q => q.OrderByDescending(f => f.FontID),
                                      includeProperties: "FontType,FontUsage");
                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    int templateId = unitOfWork.FontRepository.context.Templates.Where(t => t.Name.ToLower().Contains(searchString.ToLower())).Select(t => t.TemplateID).FirstOrDefault();

                    fonts = unitOfWork.FontRepository
                                       .Get(filter: f => f.FontUsage.DocumentID == null && f.FontID == fontID  && f.FontUsage.TemplateID == templateId,
                                           orderBy: q => q.OrderByDescending(f => f.FontID),
                                           includeProperties: "FontType,FontUsage");
                    pageSize = fonts.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(fonts.ToPagedList(pageNumber, pageSize));
                }
            }

            #endregion
        }

        //
        // GET: /Font/Details/5

        public ActionResult Details(int id = 0)
        {
            #region code
            Font font = unitOfWork.FontRepository.GetByID(id);
            if (font == null)
            {
                return HttpNotFound();
            }
            return View(font);
            #endregion
        }

        //
        // GET: /Font/Create

        public ActionResult Create(string searchString)
        {
            #region code
            ViewBag.currentFilter = searchString;
                        
            colorCodes.Add("Black", 0);
            colorCodes.Add("Gray", 8224125);
            colorCodes.Add("Dark Gray", 5855577);
            colorCodes.Add("Red", 14230065);
            colorCodes.Add("Green", 7188285);
            colorCodes.Add("Blue", 2659797);
            colorCodes.Add("Orange", 15103488);
            colorCodes.Add("Turquosie", 3456186);
            colorCodes.Add("Violet", 10167683);
            colorCodes.Add("White", 16777215);
            ViewBag.ColourCodes = new SelectList(colorCodes, "Value", "Key");

            ViewBag.FontTypeID = new SelectList(unitOfWork.FontRepository.context.FontTypes, "FontTypeID", "Name");
            ViewBag.FontID = new SelectList(unitOfWork.FontRepository.context.FontUsages, "FontID", "FontID");

            return View();
            #endregion
        }

        //
        // POST: /Font/Create

        [HttpPost]
        public ActionResult Create(Font font, FormCollection form)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    if (font.Name == null)
                    {
                        font.Name = form["Name"];
                    }
                    if (font.Color == 0)
                    {
                        font.Color = Convert.ToInt32(form["ColourCodes"]);
                    }

                    unitOfWork.FontRepository.Insert(font);
                    unitOfWork.Save();
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Font created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating font area. Please try again");
            }
            ViewBag.ColourCodes = new SelectList(colorCodes, "Value", "Key");
            ViewBag.FontTypeID = new SelectList(unitOfWork.FontRepository.context.FontTypes, "FontTypeID", "Name", font.FontTypeID);
            ViewBag.FontID = new SelectList(unitOfWork.FontRepository.context.FontUsages, "FontID", "FontID", font.FontID);
            return View(font);
            #endregion
        }

        //
        // GET: /Font/Edit/5

        public ActionResult Edit(int? id, string searchString)
        {
            #region code
            Font font = unitOfWork.FontRepository.GetByID(id);
            ViewBag.currentFilter = searchString;

            if (font == null)
            {
                return HttpNotFound();
            }
            ViewBag.FontTypeID = new SelectList(unitOfWork.FontRepository.context.FontTypes, "FontTypeID", "Name", font.FontTypeID);
            ViewBag.FontID = new SelectList(unitOfWork.FontRepository.context.FontUsages, "FontID", "FontID", font.FontID);

            ViewBag.FontName = new SelectList(unitOfWork.FontRepository.context.Fonts, "FontID", "Name");

            return View(font);
            #endregion
        }

        //
        // POST: /Font/Edit/5

        [HttpPost]
        public ActionResult Edit(Font font, string searchString, FormCollection form)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    if (font.Name == null)
                    {
                        font.Name = form["FontName"];
                    }

                    unitOfWork.FontRepository.Update(font);
                    unitOfWork.Save();
                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Font updated successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating font area. Please try again");
            }
            ViewBag.FontTypeID = new SelectList(unitOfWork.FontRepository.context.FontTypes, "FontTypeID", "Name", font.FontTypeID);
            ViewBag.FontID = new SelectList(unitOfWork.FontRepository.context.FontUsages, "FontID", "FontID", font.FontID);
            return View(font);
            #endregion
        }

        //
        // GET: /Font/Delete/5

        public ActionResult Delete(int? id, string searchString)
        {
            #region code
            Font font = unitOfWork.FontRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (font == null)
            {
                return HttpNotFound();
            }
            return View(font);
            #endregion
        }

        //
        // POST: /Font/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region code
            try
            {
                Font font = unitOfWork.FontRepository.GetByID(id);
                unitOfWork.FontRepository.Delete(font);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Font deleted successfully"); ;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting font area. Please try again");
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