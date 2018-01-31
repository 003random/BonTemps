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

namespace BonTemps.Controllers
{
    [Authorize(Roles = "kok,Admin")]
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menus
        public ActionResult Index()
        {
            var menus = db.Allergies.Include(r => r.Name);
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
        public ActionResult Create(allergyMenuViewModel allergyMenuViewModel)
        {
            var menu = new Menus { DateCreated = DateTime.Now, Description = allergyMenuViewModel.Description, Name = allergyMenuViewModel.Name};
            ViewBag.allergies = db.Allergies.ToList();

            //if (allergyMenuViewModel.Image == null)
            //{
            //    Json("Geen afbeelding geupload");
            //}

            //menu.Image = UploadImage(allergyMenuViewModel.Image);

            db.Menus.Add(menu);
            db.SaveChanges();
            return Json("Succesvol opgeslagen");
            }

        // GET: Menus/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menus menus, HttpPostedFileBase picture)
        {
            if (!ModelState.IsValid)
                return View(menus);

            if (picture != null)
            {
                menus.Image = UploadImage(picture);
            }

            db.Entry(menus).State = EntityState.Modified;

            if (picture == null)
            {
                db.Entry(menus).Property(x => x.Image).IsModified = false;
            }
            db.SaveChanges();
            TempData["success"] = "Succesvol bewerkt";
            return RedirectToAction("Index");
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
