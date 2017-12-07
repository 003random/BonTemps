using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;

namespace BonTemps.Controllers
{
    public class Table_layoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Table_layout
        public ActionResult Index()
        {
            return View(db.Table_layout.ToList());
        }

        // GET: Table_layout/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table_layout table_layout = db.Table_layout.Find(id);
            if (table_layout == null)
            {
                return HttpNotFound();
            }
            return View(table_layout);
        }

        // GET: Table_layout/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Table_layout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TableLayoutId,LayoutX,LayoutY,IsTable")] Table_layout table_layout)
        {
            if (ModelState.IsValid)
            {
                db.Table_layout.Add(table_layout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table_layout);
        }

        // GET: Table_layout/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table_layout table_layout = db.Table_layout.Find(id);
            if (table_layout == null)
            {
                return HttpNotFound();
            }
            return View(table_layout);
        }

        // POST: Table_layout/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TableLayoutId,LayoutX,LayoutY,IsTable")] Table_layout table_layout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(table_layout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table_layout);
        }

        // GET: Table_layout/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table_layout table_layout = db.Table_layout.Find(id);
            if (table_layout == null)
            {
                return HttpNotFound();
            }
            return View(table_layout);
        }

        // POST: Table_layout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Table_layout table_layout = db.Table_layout.Find(id);
            db.Table_layout.Remove(table_layout);
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
