using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task3.Models;
using Task3.Services;
using Task3.Filters;
using PagedList;
using PagedList.Mvc;

namespace Task3.Controllers
{
    [CustomAuthorizeFilter("Admin", "Normal", "Test")]
    public class CategoriesController : Controller
    {
        private DBModel db = new DBModel();

     
        // GET: Categories
        public ActionResult Index(int page =1, int pagesize =10)
        {
            var listCategories = db.categories.ToList();
            PagedList<Category> _categoryList = new PagedList<Category>( listCategories, page, pagesize);
            return View(_categoryList);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,IsActive,CreatedBy,CreatedDate,ModifiedDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                Category _category = new Category();
                _category.CategoryId = category.CategoryId;
                _category.CategoryName = category.CategoryName;
                _category.IsActive = category.IsActive;
                _category.CreatedBy = (int) Session["UserId"];
                _category.CreatedDate = DateTime.Now;
                _category.ModifiedDate = null;
                db.categories.Add(_category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorizeFilter("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,IsActive,CreatedBy,CreatedDate,ModifiedDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                var _category = db.categories.Where(m => m.CategoryId == category.CategoryId).SingleOrDefault();
                _category.CategoryName = category.CategoryName;
                _category.IsActive = category.IsActive;
                _category.CreatedBy = category.CreatedBy;
                _category.CreatedDate = _category.CreatedDate;
                _category.ModifiedDate = DateTime.Now;

                db.Entry(_category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [CustomAuthorizeFilter("Admin")]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.categories.Find(id);
            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
