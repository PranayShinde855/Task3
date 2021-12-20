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
using System.Threading.Tasks;

namespace Task3.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorizeFilter("Admin", "Normal", "Test")]
    public class ProductsController : Controller
    {       
        private DBModel db = new DBModel();

        [CustomAuthorizeFilter("Admin", "Normal", "Test")]

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10)
        {
            var listProducts = await db.products.ToListAsync();
            PagedList<Product> _productsList = new PagedList<Product>(listProducts, page, pageSize);
            return View(_productsList);
        }

        [HttpGet]
        // GET: Products
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpGet]
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
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
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpGet]
        // GET: Products/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [CustomAuthorizeFilter("Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
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
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete
        [HttpPost, ActionName("Delete")]
        [CustomAuthorizeFilter("Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = db.products.Find(id);
            db.products.Remove(product);
            await db.SaveChangesAsync();
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
