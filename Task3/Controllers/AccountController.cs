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

namespace Task3.Controllers
{
    public class AccountController : Controller
    {
        private DBModel db = new DBModel();

        [CustomAuthenticationFilter]
        [CustomAuthorizeFilter("Admin")]
        // GET: Account
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }


        // GET: Account/Create
        public ActionResult Register()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name");
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserId,UserName,RoleId,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var checkUserExist = db.Users.Where(m => m.UserName == user.UserName).SingleOrDefault();
                if(checkUserExist != null)
                {
                    ModelState.AddModelError("", "UserName already Exist, Enter another UserName.");
                }
                else
                {
                    User _user = new User();
                    _user.UserId = user.UserId;
                    _user.UserName = user.UserName;
                    _user.RoleId = user.RoleId;
                    _user.Role = user.Role;
                    _user.Password = user.Password;
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }

            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", user.RoleId);
            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User users)
        {
            if(ModelState.IsValid)
            {
                var checkInputs = db.Users.Where(m => m.UserName == users.UserName && m.Password == users.Password).SingleOrDefault();
                if ( checkInputs!= null)
                {
                    Session["UserId"] = checkInputs.UserId;
                    Session["UserName"] = checkInputs.UserName;

                    if (Session["UserID"] == null)
                    {
                        ModelState.AddModelError("", "Invalid UserName or Password1.");
                        return View(users);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Products");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password.");
                    return View(users);
                }
            }
            else
            {
                return View(users);
            } 
        }

      

        public ActionResult Logout()
        {
            Session["UserId"] = string.Empty;
            Session["UserName"] = string.Empty;
            return RedirectToAction("Login");
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
