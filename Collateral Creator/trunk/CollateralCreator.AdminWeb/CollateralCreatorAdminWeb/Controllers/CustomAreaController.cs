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
using System.IO;

namespace CollateralCreatorAdminWeb.Controllers
{
    public class CustomAreaController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /CustomArea/

        public ActionResult Index(string currentFilter, string searchString, int? page, int? pageID, int? templateID)
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



            if (pageID == null && templateID == null)
            {
                var customizableareas = unitOfWork.CustomAreaRepository
                                        .Get(filter: c => (c.Page.DocumentID == null) && (c.Page.TemplateID != null), 
                                             orderBy: q => q.OrderByDescending(c => c.AreaID), 
                                             includeProperties: "Page");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    customizableareas = unitOfWork.CustomAreaRepository
                                        .Get(filter: c => c.PageID == c.Page.PageID && c.Page.DocumentID == null && c.Page.TemplateID != null && c.Page.Template.Name.ToLower().Contains(searchString.ToLower()),
                                             orderBy: q => q.OrderByDescending(c => c.AreaID),
                                             includeProperties: "Page");
                    pageSize = customizableareas.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(customizableareas.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var customizableareas = unitOfWork.CustomAreaRepository
                                        .Get(filter: c => c.Page.DocumentID == null && c.Page.TemplateID != null && c.PageID == pageID && c.Page.TemplateID == templateID, 
                                             orderBy: q => q.OrderByDescending(c => c.AreaID),
                                             includeProperties: "Page");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    customizableareas = unitOfWork.CustomAreaRepository
                                        .Get(filter: c => c.PageID == pageID && c.Page.DocumentID == null && c.Page.TemplateID != null &&  c.Page.TemplateID == templateID && c.Page.Template.Name.ToLower().Contains(searchString.ToLower()),
                                             orderBy: q => q.OrderByDescending(c => c.AreaID),
                                             includeProperties: "Page");

                    pageSize = customizableareas.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(customizableareas.ToPagedList(pageNumber, pageSize));
                }
            }

            #endregion
        }

        //
        // GET: /CustomArea/Details/5

        public ActionResult Details(int id = 0)
        {
            #region code
            CustomizableArea customizablearea = unitOfWork.CustomAreaRepository.GetByID(id);
            if (customizablearea == null)
            {
                return HttpNotFound();
            }
            return View(customizablearea);
            #endregion
        }

        //
        // GET: /CustomArea/Create
       
        public ActionResult Create(string searchString)
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
        public ActionResult Create(CustomizableArea customizablearea)
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

        //
        // GET: /TextArea/Create

        public ActionResult CreateTextArea(int? areaID, string searchString)
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

        [HttpPost]
        public ActionResult CreateTextArea(TextArea textarea)
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
        // GET: /ImageArea/Create

        public ActionResult CreateImageArea(int? areaID, string searchString)
        {
            #region code
            ViewBag.currentFilter = searchString;
            ViewBag.AreaID = new SelectList(unitOfWork.ImageAreaRepository.context.CustomizableAreas.OrderByDescending(c => c.AreaID), "AreaID", "AreaID");

            return View();
            #endregion
        }

        //
        // POST: /ImageArea/Create

        [HttpPost]
        public ActionResult CreateImageArea(ImageArea imagearea)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    byte[] fileData = null;
                    HttpPostedFileBase image = Request.Files["partnerLogo"];
                    using (var binaryReader = new BinaryReader(image.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        imagearea.Image = fileData;
                    }

                    unitOfWork.ImageAreaRepository.Insert(imagearea);
                    unitOfWork.Save();
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Success, "Image Area created successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "Error in creating image area. Please try again");
            }

            ViewBag.AreaID = new SelectList(unitOfWork.ImageAreaRepository.context.CustomizableAreas, "AreaID", "Name", imagearea.AreaID);
            return View(imagearea);
            #endregion
        }
        //
        // GET: /CustomArea/Edit/5

        public ActionResult Edit(int? id, string searchString)
        {
            #region code

            CustomizableArea customizablearea = unitOfWork.CustomAreaRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (customizablearea == null)
            {
                return HttpNotFound();
            }
            
            //ViewBag.PageID = new SelectList(unitOfWork.CustomAreaRepository.context.Pages, "PageID", "PageID", customizablearea.PageID);
            ViewBag.PageID = unitOfWork.CustomAreaRepository.GetByID(id).PageID;

            return View(customizablearea);

            #endregion
        }

        //
        // POST: /CustomArea/Edit/5

        [HttpPost]
        public ActionResult Edit(CustomizableArea customizablearea, string searchString)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.CustomAreaRepository.Update(customizablearea);
                    unitOfWork.Save();
                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Custom Area updated successfully"); 
                }
            }
            catch (Exception ex)//System.Data.Entity.Validation.DbEntityValidationException e
            {
                //var outputLines = new List<string>();
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    outputLines.Add(string.Format(
                //        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                //        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                //    }
                //}
                //System.IO.File.AppendAllLines(@"C:\_Development\Source\Projects\Xerox\Collateral Creator\trunk\CollateralCreator.AdminWeb\CollateralCreatorAdminWeb\temp\errors.txt", outputLines);
                
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating cusotm area. Please try again");
            }

            ViewBag.PageID = new SelectList(unitOfWork.CustomAreaRepository.context.Pages, "PageID", "PageID", customizablearea.PageID);
            return View(customizablearea);
            #endregion
        }

        //
        // GET: /CustomArea/Delete/5

        public ActionResult Delete(int? id, string searchString)
        {
            #region code
            CustomizableArea customizablearea = unitOfWork.CustomAreaRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (customizablearea == null)
            {
                return HttpNotFound();
            }
            return View(customizablearea);
            #endregion
        }

        //
        // POST: /CustomArea/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region code
            try
            {
                CustomizableArea customizablearea = unitOfWork.CustomAreaRepository.GetByID(id);
                unitOfWork.CustomAreaRepository.Delete(customizablearea);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Custom Area deleted successfully");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting cusotm area. Please try again");
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