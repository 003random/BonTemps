using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using OfficeOpenXml;

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
            var roleId = user.Roles.First().RoleId;
            var role = _db.Roles.FirstOrDefault(u => u.Id == roleId).Name;
            ViewBag.roles = _db.Roles.ToList();
            
            return View(new RolesUsersModelView{ Role = role, UserId = user.Id, Username = user.UserName });
        }


        public ActionResult Delete(string id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            _db.Users.Remove(user);
            _db.SaveChanges();
            TempData["success"] = "Succesvol verwijderd";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(RolesUsersModelView rolesUsersModelView)
        {
            ViewBag.roles = _db.Roles.ToList();

            if (!string.IsNullOrEmpty(rolesUsersModelView.UserId) && rolesUsersModelView.Role != null)
            {
                var context = new ApplicationDbContext();
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = _db.Users.FirstOrDefault(u => u.Id == rolesUsersModelView.UserId);

                var roles = await userManager.GetRolesAsync(rolesUsersModelView.UserId);
                await userManager.RemoveFromRolesAsync(rolesUsersModelView.UserId, roles.ToArray());

                userManager.AddToRole(rolesUsersModelView.UserId, rolesUsersModelView.Role);
                
                TempData["success"] = "Succesvol bewerkt!";
                return View();
            }
            TempData["error"] = "een of meer is/zijn incorrect";
            return View(rolesUsersModelView);
        }

        public EmptyResult ExportToExcel()
        {
            var userRoles = new List<RolesViewModel>();
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            
            var pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Bedrijf:";
            ws.Cells["B1"].Value = "Bon Temps";
            ws.Cells["A2"].Value = "Gemaakt op";
            ws.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} op {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A5"].Value = "Gebruikersnaam";
            ws.Cells["B5"].Value = "email adres";
            ws.Cells["C5"].Value = "telefoon nummer";

            var rowStart = 6;

            //Get all the usernames
            foreach (var user in userStore.Users)
            {
                
                if (user != null)
                {
                    ws.Cells[$"A{rowStart}"].Value = user.UserName;
                    ws.Cells[$"B{rowStart}"].Value = user.Email;
                    ws.Cells[$"C{rowStart}"].Value = user.PhoneNumber ?? "0";

                    rowStart++;
                }
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Gebruikers_Bon_Temps.xlsx");
            Response.End();

            return new EmptyResult();
        }
    }
}