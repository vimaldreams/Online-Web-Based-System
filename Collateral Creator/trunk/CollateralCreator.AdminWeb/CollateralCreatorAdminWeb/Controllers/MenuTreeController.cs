using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;
using CollateralCreatorAdminWeb.Extensions;
using CollateralCreatorAdminWeb.DAL;

namespace CollateralCreatorAdminWeb.Controllers
{
    public class MenuTreeController : Controller
    {
        #region MemberVariable
        private int NodeID { get; set; }
        private string NodeIDs { get; set; }
        private MenuTree menuTree = null;
        private MenuTreeRecursive menuTreeRecursive = null;
        #endregion

        #region Entities
        private UnitOfWork unitOfWork = new UnitOfWork();
        #endregion
        //
        // GET: /MenuTree/

        public ActionResult Index()
        {
            #region code
            ViewBag.Languages = new SelectList(unitOfWork.MenuTreeRepository.context.Languages, "LanguageID", "LanguageName"); //get drop down list of languages
            return View();
            #endregion
        }

        //
        // POST: /MenuTree/Index/5
        [HttpPost]
        public ActionResult Index(int languageID, string parentNode, string childNodes)
        {
            #region code

            ViewBag.Languages = new SelectList(unitOfWork.MenuTreeRepository.context.Languages, "LanguageID", "LanguageName"); //get drop down list of languages

            try
            {
                int counter = 1;

                if (ModelState.IsValid)
                {                    
                    if (parentNode != string.Empty)
                    {
                        //insert menutree table parent data
                        menuTree = new MenuTree();

                        menuTree.ParentNodeID = 1;
                        menuTree.TranslationKey = parentNode;
                        menuTree.LanguageID = languageID;

                        unitOfWork.MenuTreeRepository.Insert(menuTree);
                        unitOfWork.Save();

                        //get the identity of menutree entity
                        NodeID = menuTree.NodeID;

                        NodeIDs += "'" + NodeID + "'" + ',';

                        //insert menutreerecursive table parent data
                        menuTreeRecursive = new MenuTreeRecursive();

                        menuTreeRecursive.ParentNodeID = 1;
                        menuTreeRecursive.NodeID = NodeID;
                        menuTreeRecursive.Length = 1;

                        unitOfWork.MenuTreeRecursiveRepository.Insert(menuTreeRecursive);
                        unitOfWork.Save();
                    }
                                        
                    foreach (string childNode in childNodes.Split(','))
                    {
                        //insert menutree table child data
                        menuTree = new MenuTree();

                        menuTree.ParentNodeID = NodeID;
                        menuTree.TranslationKey = childNode;
                        menuTree.LanguageID = languageID;

                        unitOfWork.MenuTreeRepository.Insert(menuTree);
                        unitOfWork.Save();

                        //insert menutreerecursive table parent data
                        menuTreeRecursive = new MenuTreeRecursive();

                        menuTreeRecursive.ParentNodeID = NodeID;
                        menuTreeRecursive.NodeID = NodeID + counter;
                        menuTreeRecursive.Length = 2;

                        unitOfWork.MenuTreeRecursiveRepository.Insert(menuTreeRecursive);
                        unitOfWork.Save();

                        int nodeincrement = NodeID + counter;
                        NodeIDs += "'" + nodeincrement + "'" + ',';

                        counter++;
                    }
                }
                
                return Json(new { message = "MenuTree nodes " + NodeIDs + "saved successfully."  });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
            
            #endregion
        }

        //
        // GET: /MenuTree/Details/5

        public ActionResult Details(int id)
        {
            #region code
            return View();
            #endregion
        }

        //
        // GET: /MenuTree/Create

        public ActionResult Create()
        {
            #region code
            return View();
            #endregion
        }

        //
        // POST: /MenuTree/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            #region code
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            #endregion
        }

        //
        // GET: /MenuTree/Edit/5

        public ActionResult Edit(int id)
        {
            #region code
            return View();
            #endregion
        }

        //
        // POST: /MenuTree/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            #region code
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            #endregion
        }

        //
        // GET: /MenuTree/Delete/5

        public ActionResult Delete(int id)
        {
            #region code
            return View();
            #endregion
        }

        //
        // POST: /MenuTree/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            #region code
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            #endregion
        }
    }
}
