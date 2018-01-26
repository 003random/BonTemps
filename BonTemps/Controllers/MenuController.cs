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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menus menus, HttpPostedFileBase picture)
        {
            if (picture == null)
            {
                TempData["error"] = "No image uploaded";
                return View(menus);
            }

            menus.Image = UploadImage(picture);

            if (!ModelState.IsValid)
                return View(menus);

            db.Menus.Add(menus);
            db.SaveChanges();
            TempData["success"] = "Succesvol opgeslagen";
            return RedirectToAction("Create");

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
            if (picture != null)
            {
                menus.Image = UploadImage(picture);
            }

            if (ModelState.IsValid)
            {
                db.Entry(menus).State = EntityState.Modified;
                db.SaveChanges();
                TempData["success"] = "Succesvol bewerkt";
                return RedirectToAction("Index");
            }
            return View(menus);
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
