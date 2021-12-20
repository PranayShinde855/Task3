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
using System.Threading.Tasks;

namespace Task3.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorizeFilter("Admin", "Normal", "Test")]
    public class CategoriesController : Controller
    {
        private DBModel db = new DBModel();

     
        [HttpGet]
        // GET: Categories
        public async Task<ActionResult> Index(int page =1, int pagesize =10)
        {
            //listing data
            var listCategories = await db.categories.ToListAsync();
            // Pagination
            PagedList<Category> _categoryList = new PagedList<Category>( listCategories, page, pagesize);
            return View(_categoryList);
        }
               
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.categories.FindAsync(id);
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
        public async Task<ActionResult> Create(Category category)
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
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        
        [HttpGet]
        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [CustomAuthorizeFilter("Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var _category = await db.categories.Where(m => m.CategoryId == category.CategoryId).SingleOrDefaultAsync();
                _category.CategoryName = category.CategoryName;
                _category.IsActive = category.IsActive;
                _category.CreatedBy = category.CreatedBy;
                _category.CreatedDate = _category.CreatedDate;
                _category.ModifiedDate = DateTime.Now;

                db.Entry(_category).State = EntityState.Modified;
               await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        [HttpGet]
        [CustomAuthorizeFilter("Admin")]
        // GET: Categories/Delete
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete
        [HttpGet]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.categories.FindAsync(id);
           db.categories.Remove(category);
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
