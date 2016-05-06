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
    public class TextAreaController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private Dictionary<string, int> colorCodes = new Dictionary<string, int>();

        //
        // GET: /TextArea/

        public ActionResult Index(string currentFilter, string searchString, int? page, int? areaID, int? pageID, int? templateID)
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

            if (areaID == null && pageID == null && templateID == null)
            {

                var textareas = unitOfWork.TextAreaRepository
                                            .Get(filter: c => c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null,
                                                orderBy: q => q.OrderByDescending(c => c.TextAreaID),
                                                includeProperties: "CustomizableArea");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    textareas = unitOfWork.TextAreaRepository
                                            .Get(filter: c => c.CustomizableArea.Page.Template.Name.ToLower().Contains(searchString.ToLower()) && c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null,
                                            orderBy: q => q.OrderByDescending(c => c.TextAreaID),        
                                            includeProperties: "CustomizableArea");
                    pageSize = textareas.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(textareas.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var textareas = unitOfWork.TextAreaRepository
                                               .Get(filter: c => c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null && c.CustomizableArea.AreaID == areaID && c.CustomizableArea.PageID == pageID && c.CustomizableArea.Page.TemplateID == templateID,
                                                orderBy: q => q.OrderByDescending(c => c.TextAreaID),
                                                includeProperties: "CustomizableArea");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    textareas = unitOfWork.TextAreaRepository
                                            .Get(filter: c => c.CustomizableArea.Page.Template.Name.ToLower().Contains(searchString.ToLower()) && c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null,
                                            orderBy: q => q.OrderByDescending(c => c.TextAreaID),
                                            includeProperties: "CustomizableArea");
                    pageSize = textareas.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(textareas.ToPagedList(pageNumber, pageSize));
                }
            }
            #endregion
        }

        //
        // GET: /TextArea/Details/5

        public ActionResult Details(int id = 0)
        {
            #region code
            TextArea textarea = unitOfWork.TextAreaRepository.GetByID(id);
            if (textarea == null)
            {
                return HttpNotFound();
            }
            return View(textarea);
            #endregion
        }

        //
        // GET: /TextArea/Create

        public ActionResult Create(string searchString)
        {
            #region code
            //get unique values
            var CustomizableAreas = unitOfWork.TextAreaRepository.context.CustomizableAreas.GroupBy(c => c.Name).Select(c => c.FirstOrDefault());

            ViewBag.currentFilter = searchString;

            ViewBag.AreaID = new SelectList(unitOfWork.TextAreaRepository.context.CustomizableAreas.OrderByDescending(c => c.AreaID), "AreaID", "AreaID");

            ViewBag.Align = new SelectList(new List<string> { "LEFT", "RIGHT", "TOP", "BOTTOM" }, string.Empty, string.Empty);

            return View();
            #endregion
        }
        
        //
        // POST: /TextArea/Create

        [HttpPost]
        public ActionResult Create(TextArea textarea)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.TextAreaRepository.Insert(textarea);
                    unitOfWork.Save();
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Text Area created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating text area. Please try again");
            }

            ViewBag.TextAreaID = new SelectList(unitOfWork.TextAreaRepository.context.CustomizableAreas, "AreaID", "Name", textarea.AreaID);
            return View(textarea);
            #endregion
        }

        //
        // GET: /Font/Create

        public ActionResult CreateFont(string searchString)
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
        public ActionResult CreateFont(Font font, FormCollection form)
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
        // GET: /FontUsage/Create

        public ActionResult CreateFontUsage(int? textAreaID, string searchString)
        {
            #region code            
            ViewBag.currentFilter = searchString;

            var templates = unitOfWork.FontUsageRepository.context.Templates.ToList().OrderByDescending(t => t.TemplateID);

            ViewBag.Templates = new SelectList(templates, "TemplateID", "Name");

            var fonts = unitOfWork.FontUsageRepository.context.TextAreas
                        .Include(c => c.CustomizableArea)
                        .Include(c => c.CustomizableArea.Page).Include(c => c.CustomizableArea.Page.Template)
                        .Where(c => (c.CustomizableArea.Page.DocumentID == null) && (c.CustomizableArea.Page.TemplateID != null))
                        .OrderByDescending(t => t.TextAreaID).ToList();

            ViewBag.FontID = new SelectList(unitOfWork.FontUsageRepository.context.Fonts.OrderByDescending(f => f.FontID).Take(2), "FontID", "FontID");
            ViewBag.TextAreaID = new SelectList(fonts, "TextAreaID", "TextAreaID");

            return View();
            #endregion
        }

        //
        // POST: /FontUsage/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFontUsage(FontUsage fontusage)
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
        // GET: /TextArea/Edit/5

        public ActionResult Edit(int? id, string searchString)
        {
            #region code
            TextArea textarea = unitOfWork.TextAreaRepository.GetByID(id);
            ViewBag.currentFilter = searchString;

            if (textarea == null)
            {
                return HttpNotFound();
            }
            //ViewBag.TextAreaID = new SelectList(unitOfWork.TextAreaRepository.context.CustomizableAreas, "AreaID", "Name", textarea.AreaID);

            ViewBag.CustomAreaName = unitOfWork.CustomAreaRepository.GetByID(textarea.AreaID).Name;

            return View(textarea);
            #endregion
        }

        //
        // POST: /TextArea/Edit/5

        [HttpPost]
        public ActionResult Edit(TextArea textarea, string searchString)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.TextAreaRepository.Update(textarea);
                    unitOfWork.Save();
                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Text Area updated successfully"); 
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating text area. Please try again");
            }
            ViewBag.TextAreaID = new SelectList(unitOfWork.TextAreaRepository.context.CustomizableAreas, "AreaID", "Name", textarea.AreaID);
            return View(textarea);
            #endregion
        }

        //
        // GET: /TextArea/Delete/5

        public ActionResult Delete(int? id, string searchString)
        {
            #region code
            TextArea textarea = unitOfWork.TextAreaRepository.GetByID(id);
            ViewBag.currentFilter = searchString;

            if (textarea == null)
            {
                return HttpNotFound();
            }
            return View(textarea);
            #endregion
        }

        //
        // POST: /TextArea/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region code
            try
            {
                TextArea textarea = unitOfWork.TextAreaRepository.GetByID(id);
                unitOfWork.TextAreaRepository.Delete(textarea);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Text Area deleted successfully"); ;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting text area. Please try again");
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