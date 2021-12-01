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
using NPOI.SS.Formula.Functions;

namespace Task3.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorizeFilter("Admin", "Normal", "Test")]
    public class ProductsController : Controller
    {       
        private DBModel db = new DBModel();

        [CustomAuthorizeFilter("Admin", "Normal", "Test")]

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var listProducts = db.products.ToList();
            PagedList<Product> _productsList = new PagedList<Product>(listProducts, page, pageSize);
            return View(_productsList);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductName,CategoryId,CreatedBy,CreatedDate,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                Product _product = new Product();
                _product.Id = product.Id;
                _product.ProductName = product.ProductName;
                _product.CategoryId = product.CategoryId;
                _product.Category = product.Category;
                _product.CreatedBy = (int)this.Session["UserId"];
                _product.CreatedDate = DateTime.Now;
                _product.ModifiedDate = null;
                db.products.Add(_product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorizeFilter("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,CategoryId,CreatedBy,CreatedDate,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                var userId = Session["UserId"];
                var _product = db.products.Where(m => m.Id == product.Id).SingleOrDefault();
                _product.ProductName = product.ProductName;
                _product.CategoryId = product.CategoryId;
                _product.Category = product.Category;
                _product.CreatedBy = (int)userId;
                _product.CreatedDate = _product.CreatedDate;
                _product.ModifiedDate = DateTime.Now;
                db.Entry(_product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [CustomAuthorizeFilter("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.products.Find(id);
            db.products.Remove(product);
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
