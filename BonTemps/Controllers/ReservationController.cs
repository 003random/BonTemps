using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using System.Web.UI.WebControls;
using OfficeOpenXml;

namespace BonTemps.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = _db.Reservations.Include(r => r.Customer);
            return View(reservations.ToList());
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            TempData["message"] = DateTime.Now;
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservationViewModel reservationCustomer)
        {
            var customer = new Customers
            {
                Email = reservationCustomer.Email,
                FirstName = reservationCustomer.FirstName,
                Prefix = reservationCustomer.Prefix,
                LastName = reservationCustomer.LastName,
                Gender = reservationCustomer.Gender,
                PhoneNumber = reservationCustomer.PhoneNumber,
                NewsLetter = reservationCustomer.NewsLetter
            };

            var reservation = new Reservations
            {
                Date = reservationCustomer.Date,
                Persons = reservationCustomer.Persons,
                Customer = customer
            };

            if (ModelState.IsValid)
            {
                _db.Reservations.Add(reservation);
                _db.SaveChanges();
                TempData["success"] = "Reservering succesvol aangemaakt";
                return RedirectToAction("Index");
            }

            return View(reservationCustomer);
        }


        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = _db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            return View(reservations);
        }


        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            //ToDo: use viewmodel
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var reservation = _db.Reservations.Include(i => i.Customer).SingleOrDefault(x => x.Id == id);

            if (reservation == null)
            {
                return HttpNotFound();
            }

            if (id != null)
            {
                var reservationViewModel = new ReservationViewModel
                {
                    FirstName = reservation.Customer.FirstName,
                    Prefix = reservation.Customer.Prefix,
                    LastName = reservation.Customer.LastName,
                    Gender = reservation.Customer.Gender,
                    Email = reservation.Customer.Email,
                    PhoneNumber = reservation.Customer.PhoneNumber,
                    NewsLetter = reservation.Customer.NewsLetter,
                    Date = reservation.Date,
                    Persons = reservation.Persons,
                    CustomerId = reservation.Customer.Id,
                    ReservationId = (int)id
                };

                return View(reservationViewModel);
            }
            return HttpNotFound();
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationViewModel reservationCustomer)
        {
            var reservation = _db.Reservations.Include(i => i.Customer).SingleOrDefault(x => x.Id == reservationCustomer.ReservationId);


            if (reservation != null)
            {
                reservation.Customer.Email = reservationCustomer.Email;
                reservation.Customer.FirstName = reservationCustomer.FirstName;
                reservation.Customer.Prefix = reservationCustomer.Prefix;
                reservation.Customer.LastName = reservationCustomer.LastName;
                reservation.Customer.Gender = reservationCustomer.Gender;
                reservation.Customer.PhoneNumber = reservationCustomer.PhoneNumber;
                reservation.Customer.NewsLetter = reservationCustomer.NewsLetter;
                
                reservation.Date = reservationCustomer.Date;
                reservation.Persons = reservationCustomer.Persons;

                if (ModelState.IsValid)
                {
                    _db.Entry(reservation).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(reservationCustomer);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = _db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            return View(reservations);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var reservations = _db.Reservations.Find(id);
            _db.Reservations_Table_Layout.RemoveRange(_db.Reservations_Table_Layout.Include(r => r.Reservation).Where(r => r.Reservation.Id == reservations.Id));

            if (reservations != null)
                _db.Reservations.Remove(reservations);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<Reservations> Reservations()
        {
            var reservations = (from e in _db.Reservations
                            select e).ToList();
            return reservations;
        }


        public EmptyResult ExportToExcel()
        {

            List<Reservations> Reservation = _db.Reservations.ToList();
            List<Customers> Customer = _db.Customers.ToList();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

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

            int rowStart = 6;

            foreach (var item in Customer)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.FirstName;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Prefix;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.LastName;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Gender;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.PhoneNumber;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.NewsLetter;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Email;
            }

            foreach (var item in Reservation)
            {
                ws.Cells[string.Format("I{0}", rowStart)].Value = Convert.ToString(item.Date);
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.Persons;

                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content - disposition", "attachment: filename = " + "BonTempsReservations.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            return new EmptyResult();
        }

    }
}
