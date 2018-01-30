using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace BonTemps.Controllers
{
    public class Role_manageController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Role_manage
        public ActionResult Index()
        {
            var userRoles = new List<RolesViewModel>();
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            //Get all the usernames
            foreach (var user in userStore.Users)
            {
                var r = new RolesViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id
                };
                userRoles.Add(r);
            }
            //Get all the Roles for our users
            foreach (var user in userRoles)
            {
                user.RoleNames = userManager.GetRoles(userStore.Users.First(s => s.UserName == user.UserName).Id);
            }

            return View(userRoles);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.username = user.UserName;
            var role = user.Roles.First().RoleId;
            ViewBag.role = _db.Roles.FirstOrDefault(u => u.Id == role).Name;
            ViewBag.roles = _db.Roles.ToList();
            ViewBag.userid = user.Id;
            
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string userId, string roleId)
        {
            if (!string.IsNullOrEmpty(userId) && roleId != null)
            {
                var context = new ApplicationDbContext();
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);

                userManager.RemoveFromRoles(userId, user.Roles.First().RoleId);
                userManager.AddToRole(userId, roleId);
                
                TempData["success"] = "Succesvol bewerkt!";
                return View();
            }
            TempData["error"] = "een of meer is/zijn incorrect";
            return View();
        }
    }
}