using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using OfficeOpenXml;
using System.Globalization;

namespace BonTemps.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            //Test2
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Gender,FirstName,Prefix,LastName,PhoneNumber,Email,NewsLetter")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customers);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customers customers = db.Customers.Find(id);
            db.Customers.Remove(customers);
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

        public EmptyResult ExportToExcel()
        {

            var customer = db.Customers.ToList();
            var pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Bedrijf:";
            ws.Cells["B1"].Value = "Bon Temps";
            ws.Cells["A2"].Value = "Gemaakt op";
            ws.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} op {0:H: mm tt}", DateTimeOffset.Now);
            ws.Cells["A3"].Value = "Reserveringen";

            ws.Cells["A5"].Value = "Voornaam";
            ws.Cells["B5"].Value = "Tussenvoegsel";
            ws.Cells["C5"].Value = "Achternaam";
            ws.Cells["D5"].Value = "Geslacht";
            ws.Cells["E5"].Value = "Mobiel";
            ws.Cells["F5"].Value = "Nieuwsbrief";
            ws.Cells["G5"].Value = "E-mail";

            ws.Cells["I5"].Value = "Datum";
            ws.Cells["J5"].Value = "Aantal Personen";

            var rowStart = 6;

            foreach (var item in customer)
            {
                if (customer != null)
                {
                    ws.Cells[$"A{rowStart}"].Value = item.FirstName;
                    ws.Cells[$"B{rowStart}"].Value = item.Prefix;
                    ws.Cells[$"C{rowStart}"].Value = item.LastName;
                    ws.Cells[$"D{rowStart}"].Value = item.Gender;
                    ws.Cells[$"E{rowStart}"].Value = item.PhoneNumber;
                    ws.Cells[$"F{rowStart}"].Value = item.NewsLetter;
                    ws.Cells[$"G{rowStart}"].Value = item.Email;



                    rowStart++;
                }
            }


            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Klanten_Bon_Temps.xlsx");
            Response.End();

            return new EmptyResult();
        }
    }
}
