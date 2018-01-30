using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using System.IO;

namespace BonTemps.Controllers
{
    public class AllergiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Allergies
        public ActionResult Index()
        {
            return View(db.Allergies.ToList());
        }

        // GET: Allergies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Allergies allergies = db.Allergies.Find(id);
            if (allergies == null)
            {
                return HttpNotFound();
            }
            return View(allergies);
        }

        // GET: Allergies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Allergies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Allergies allergies, HttpPostedFileBase picture)
        {
            if (picture == null)
            {
                TempData["error"] = "No image uploaded";
                return View(allergies);
            }

            allergies.Image = UploadImage(picture);

            if (ModelState.IsValid)
            {
                db.Allergies.Add(allergies);
                db.SaveChanges();
                TempData["success"] = "Succesvol opgeslagen";
                return RedirectToAction("Index");
            }

            return View(allergies);
        }

        // GET: Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var allergy = db.Allergies.Find(id);
            if (allergy == null)
            {
                return HttpNotFound();
            }
            return View(allergy);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Allergies allergies, HttpPostedFileBase picture)
        {
            if (!ModelState.IsValid)
                return View(allergies);

            if (picture != null)
            {
                allergies.Image = UploadImage(picture);
            }

            db.Entry(allergies).State = EntityState.Modified;

            if (picture == null)
            {
                db.Entry(allergies).Property(x => x.Image).IsModified = false;
            }
            db.SaveChanges();
            TempData["success"] = "Succesvol bewerkt";
            return RedirectToAction("Index");
        }

        // GET: Allergies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Allergies allergies = db.Allergies.Find(id);
            if (allergies == null)
            {
                return HttpNotFound();
            }
            return View(allergies);
        }

        // POST: Allergies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Allergies allergies = db.Allergies.Find(id);
            db.Allergies.Remove(allergies);
            db.SaveChanges();
            TempData["success"] = "Succesvol verwijderd";
            return RedirectToAction("Index");
        }


        public string UploadImage(HttpPostedFileBase image)
        {
            var uploadPath = Server.MapPath("~/Content/Uploads/allergy-images");
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
