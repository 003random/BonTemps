using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using DocumentFormat.OpenXml.Bibliography;

namespace BonTemps.Controllers
{
    [Authorize(Roles = "kok,Admin")]
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menus
        public ActionResult Index()
        {
            return View(db.Menus.ToList());
        }

        // GET: Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = db.Menus.Find(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            ViewBag.allergies = db.Allergies.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(allergyMenuViewModel allergyMenuViewModel, HttpPostedFileBase picture)
        {
            var menu = new Menus { DateCreated = DateTime.Now, Description = allergyMenuViewModel.Description, Name = allergyMenuViewModel.Name };
            ViewBag.allergies = db.Allergies.ToList();

            if (picture == null)
            {
                TempData["warning"] = "Geen afbeelding geupload";
                return View(allergyMenuViewModel);
            }

            menu.Image = UploadImage(picture);

            db.Menus.Add(menu);
            db.SaveChanges();

            if (allergyMenuViewModel.Allergies.StartsWith(","))
            {
                allergyMenuViewModel.Allergies = allergyMenuViewModel.Allergies.Substring(1);
            }

            foreach (var allergy in allergyMenuViewModel.Allergies.Split(','))
            {
                var intAllergy = Convert.ToInt16(allergy);
                var allergyObj = new Menus_Allergies
                {
                    Allergie = db.Allergies.FirstOrDefault(a => a.Id == intAllergy),
                    Menu = menu
                };

                db.Menus_Allergies.Add(allergyObj);
            }

            db.SaveChanges();
            TempData["success"] = "Sucesvol opgeslagen";
            return View();
        }

        // GET: Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.allergies = db.Allergies.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = db.Menus.Find(id);
            if (menus == null)
            {
                return HttpNotFound();
            }

            var arr = db.Menus_Allergies.Where(m => m.Menu.Id == menus.Id).Select(m => m.Allergie).Select(m => m.Id);

            var allergies = string.Join(",", arr);

            var modelview = new allergyMenuViewModel
            {
                Description = menus.Description,
                Name = menus.Name,
                Allergies = allergies
            };
            return View(modelview);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(allergyMenuViewModel allergyMenuViewModel, HttpPostedFileBase picture)
        {
            ViewBag.allergies = db.Allergies.ToList();

            if (string.IsNullOrEmpty(allergyMenuViewModel.Name) || string.IsNullOrEmpty(allergyMenuViewModel.Description))
            {
                TempData["error"] = "Ongeldige waardes";
                return View(allergyMenuViewModel);
            }

            var menu = new Menus { DateCreated = DateTime.Now, Description = allergyMenuViewModel.Description, Name = allergyMenuViewModel.Name };
            ViewBag.allergies = db.Allergies.ToList();

            if (!ModelState.IsValid)
                return View(allergyMenuViewModel);

            if (picture != null)
            {
                menu.Image = UploadImage(picture);
            }

            db.Entry(menu).State = EntityState.Modified;

            if (picture == null)
            {
                db.Entry(menu).Property(x => x.Image).IsModified = false;
            }

            db.SaveChanges();

            db.Menus_Allergies.RemoveRange(db.Menus_Allergies.Where(m => m.Menu.Id == menu.Id));

            if (allergyMenuViewModel.Allergies.StartsWith(","))
            {
                allergyMenuViewModel.Allergies = allergyMenuViewModel.Allergies.Substring(1);
            }

            foreach (var allergy in allergyMenuViewModel.Allergies.Split(','))
            {
                var intAllergy = Convert.ToInt16(allergy);
                var allergyObj = new Menus_Allergies
                {
                    Allergie = db.Allergies.FirstOrDefault(a => a.Id == intAllergy),
                    Menu = menu
                };

                db.Menus_Allergies.Add(allergyObj);
            }

            db.SaveChanges();
            TempData["success"] = "Sucesvol opgeslagen";
            return View();
        }

        // GET: Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = db.Menus.Find(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menus menus = db.Menus.Find(id);
            db.Menus.Remove(menus);
            db.SaveChanges();
            TempData["success"] = "Succesvol verwijderd";
            return RedirectToAction("Index");
        }

        public string UploadImage(HttpPostedFileBase image)
        {
            var uploadPath = Server.MapPath("~/Content/Uploads/menu-images");
            Directory.CreateDirectory(uploadPath);
            var fileGuid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(image.FileName);
            var newFilename = fileGuid + extension;
            image.SaveAs(Path.Combine(uploadPath, newFilename));
            return newFilename;
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
