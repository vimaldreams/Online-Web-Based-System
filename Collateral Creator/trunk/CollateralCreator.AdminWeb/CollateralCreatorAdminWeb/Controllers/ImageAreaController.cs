using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.IO;
using PagedList;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;
using CollateralCreatorAdminWeb.Extensions;
using CollateralCreatorAdminWeb.DAL;

namespace CollateralCreatorAdminWeb.Controllers
{
    public class ImageAreaController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /ImageArea/

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

                var imageareas = unitOfWork.ImageAreaRepository
                                            .Get(filter: c => c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null,
                                                orderBy: q => q.OrderByDescending(c => c.ImageAreaID),
                                                includeProperties: "CustomizableArea");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                    imageareas = unitOfWork.ImageAreaRepository
                                            .Get(filter: c => c.CustomizableArea.Page.Template.Name.ToLower().Contains(searchString.ToLower()) && c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null,
                                                orderBy: q => q.OrderByDescending(c => c.ImageAreaID),
                                                includeProperties: "CustomizableArea");
                    pageSize = imageareas.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(imageareas.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var imageareas = unitOfWork.ImageAreaRepository
                                          .Get(filter: c => c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null && c.CustomizableArea.AreaID == areaID && c.CustomizableArea.PageID == pageID && c.CustomizableArea.Page.TemplateID == templateID,
                                              orderBy: q => q.OrderByDescending(c => c.ImageAreaID),
                                              includeProperties: "CustomizableArea");

                //find if a search string has been inputted - this will be the template ID
                if (!String.IsNullOrEmpty(searchString))
                {
                     imageareas = unitOfWork.ImageAreaRepository
                                            .Get(filter: c => c.CustomizableArea.Page.Template.Name.ToLower().Contains(searchString.ToLower()) && c.CustomizableArea.Page.DocumentID == null && c.CustomizableArea.Page.TemplateID != null,
                                                orderBy: q => q.OrderByDescending(c => c.ImageAreaID),
                                                includeProperties: "CustomizableArea");
                    pageSize = imageareas.Count();
                }

                if (pageSize == 0)
                {
                    return RedirectToAction("Index").WithNotification(NotificationStatus.Error, "No Templates matching the filter criteria.");
                }
                else
                {
                    return View(imageareas.ToPagedList(pageNumber, pageSize));
                }            
            }

            #endregion
        }

        /// <summary>
        /// Render Image bytes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileContentResult Display(int id)
        {
            #region code
            byte[] image = unitOfWork.ImageAreaRepository.GetByID(id).Image;
            return new FileContentResult(image, "image/jpeg");
            #endregion
        }

        //
        // GET: /ImageArea/Details/5

        public ActionResult Details(int id = 0)
        {
            #region code
            ImageArea imagearea = unitOfWork.ImageAreaRepository.GetByID(id);
            if (imagearea == null)
            {
                return HttpNotFound();
            }
            return View(imagearea);
            #endregion
        }

        //
        // GET: /ImageArea/Create

        public ActionResult Create(string searchString)
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
        public ActionResult Create(ImageArea imagearea)
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
        // GET: /ImageArea/Edit/5

        public ActionResult Edit(int? id, string searchString)
        {
            #region code
            ImageArea imagearea = unitOfWork.ImageAreaRepository.GetByID(id);
            ViewBag.currentFilter = searchString;
            if (imagearea == null)
            {
                return HttpNotFound();
            }
            
            //ViewBag.AreaID = new SelectList(unitOfWork.ImageAreaRepository.context.CustomizableAreas, "AreaID", "Name", imagearea.AreaID);

            ViewBag.CustomAreaName = unitOfWork.CustomAreaRepository.GetByID(imagearea.AreaID).Name;

            ViewBag.ImageAreaID = imagearea.ImageAreaID;
            return View(imagearea);
            #endregion
        }

        //
        // POST: /ImageArea/Edit/5

        [HttpPost]
        public ActionResult Edit(ImageArea imagearea, string searchString)
        {
            #region code
            try
            {
                if (ModelState.IsValid)
                {                    
                    //get uploaded image bytes
                    if ((Request.Files["partnerLogo"].ContentLength > 0) && (!String.IsNullOrEmpty(Request.Files["partnerLogo"].FileName)))
                    {
                        byte[] fileData = null;

                        HttpPostedFileBase image = Request.Files["partnerLogo"];
                        using (var binaryReader = new BinaryReader(image.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                            imagearea.Image = fileData;
                        }                        
                    }

                    unitOfWork.ImageAreaRepository.Update(imagearea);
                    unitOfWork.Save();
                    return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Image Area updated successfully");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in updating image area. Please try again");
            }

            ViewBag.AreaID = new SelectList(unitOfWork.ImageAreaRepository.context.CustomizableAreas, "AreaID", "Name", imagearea.AreaID);
            return View(imagearea);
            #endregion
        }

        //
        // GET: /ImageArea/Delete/5

        public ActionResult Delete(int? id, string searchString)
        {
            #region code
            ImageArea imagearea = unitOfWork.ImageAreaRepository.GetByID(id);
            ViewBag.currentFilter = searchString;

            if (imagearea == null)
            {
                return HttpNotFound();
            }
            return View(imagearea);
            #endregion
        }

        //
        // POST: /ImageArea/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            #region code
            try
            {
                ImageArea imagearea = unitOfWork.ImageAreaRepository.GetByID(id);
                unitOfWork.ImageAreaRepository.Delete(imagearea);
                unitOfWork.Save();
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Success, "Image Area deleted successfully");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { currentFilter = searchString }).WithNotification(NotificationStatus.Error, "Error in deleting image area. Please try again");
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