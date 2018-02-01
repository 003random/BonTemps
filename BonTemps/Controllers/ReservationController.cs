using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using WebGrease.Css.Extensions;

namespace BonTemps.Controllers
{
    [Authorize(Roles = "Admin,Serveerster")]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index(DateTime? begindate, DateTime? enddate)
        {
            if (begindate != default(DateTime) && enddate != default(DateTime))
            {
                var reservations = (_db.Reservations.Include(r => r.Customer).Where(r => r.Date > begindate && r.Date < enddate));
                return View(reservations.ToList());
            }
            return View(_db.Reservations.ToList());
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
            const int seats = 40;
            const int mondayStartHour = 0;
            const int mondayStopHour = 0;

            const int tuesdayStartHour = 10;
            const int tuesdayStopHour = 24;

            const int wednesdayStartHour = 10;
            const int wednesdayStopHour = 25;

            const int thursdayStartHour = 10;
            const int thursdayStopHour = 26;

            const int fridayStartHour = 10;
            const int fridayStopHour = 27;

            const int saturdayStartHour = 10;
            const int saturdayStopHour = 29;

            const int sundayStartHour = 10;
            const int sundayStopHour = 24;


            var first = DateTime.Now.AddHours(-2);
            var last = DateTime.Now.AddHours(2);
            var reservationsInScope = _db.Reservations.Where(r => r.DateCreated >= first && r.DateCreated <= last);

            var count = 0;
            foreach (var r in reservationsInScope)
            {
                count += r.Persons;
            }

            var freeSeats = seats - count;

            if (freeSeats < reservationCustomer.Persons)
            {
                TempData["warning"] = " er zijn nog " + freeSeats + " stoelen vrij op " + reservationCustomer.Date;
                return View(reservationCustomer);
            }

            if ((reservationCustomer.Date.DayOfWeek == DayOfWeek.Monday) ||
                (reservationCustomer.Date.DayOfWeek == DayOfWeek.Tuesday && (reservationCustomer.Date.Hour < tuesdayStartHour || reservationCustomer.Date.Hour > tuesdayStopHour)) ||
                 (reservationCustomer.Date.DayOfWeek == DayOfWeek.Wednesday && (reservationCustomer.Date.Hour < wednesdayStartHour || reservationCustomer.Date.Hour > wednesdayStopHour)) ||
                  (reservationCustomer.Date.DayOfWeek == DayOfWeek.Thursday && (reservationCustomer.Date.Hour < thursdayStartHour || reservationCustomer.Date.Hour > thursdayStopHour)) ||
                   (reservationCustomer.Date.DayOfWeek == DayOfWeek.Friday && (reservationCustomer.Date.Hour < fridayStartHour || reservationCustomer.Date.Hour > fridayStopHour)) ||
                    (reservationCustomer.Date.DayOfWeek == DayOfWeek.Saturday && (reservationCustomer.Date.Hour < saturdayStartHour || reservationCustomer.Date.Hour > saturdayStopHour)) ||
                     (reservationCustomer.Date.DayOfWeek == DayOfWeek.Sunday && (reservationCustomer.Date.Hour < sundayStartHour || reservationCustomer.Date.Hour > sundayStopHour)))
            {
                var culture = new CultureInfo("nl-NL");
                TempData["warning"] = "Op " + culture.DateTimeFormat.GetDayName(reservationCustomer.Date.DayOfWeek) + " " + Convert.ToString(reservationCustomer.Date, CultureInfo.InvariantCulture).Split(' ')[1] + " is Bon Temps gesloten.";
                return View(reservationCustomer);
            }

            var customer = new Customers
            {
                Email = reservationCustomer.Email,
                FirstName = reservationCustomer.FirstName,
                Prefix = reservationCustomer.Prefix,
                LastName = reservationCustomer.LastName,
                DateOfBirth = reservationCustomer.DateOfBirth,
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

            if (id != null && reservation.Customer != null)
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

        public EmptyResult ExportToExcel()
        {

            var reservation = _db.Reservations.Include(r => r.Customer).ToList();
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

            foreach (var item in reservation)
            {
                if (item.Customer != null)
                {
                    ws.Cells[$"A{rowStart}"].Value = item.Customer.FirstName;
                    ws.Cells[$"B{rowStart}"].Value = item.Customer.Prefix;
                    ws.Cells[$"C{rowStart}"].Value = item.Customer.LastName;
                    ws.Cells[$"D{rowStart}"].Value = item.Customer.Gender;
                    ws.Cells[$"E{rowStart}"].Value = item.Customer.PhoneNumber;
                    ws.Cells[$"F{rowStart}"].Value = item.Customer.NewsLetter;
                    ws.Cells[$"G{rowStart}"].Value = item.Customer.Email;

                    ws.Cells[$"I{rowStart}"].Value = Convert.ToString(item.Date, CultureInfo.InvariantCulture);
                    ws.Cells[$"J{rowStart}"].Value = item.Persons;

                    rowStart++;
                }
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Reserveringen_Bon_Temps.xlsx");
            Response.End();

            return new EmptyResult();
        }

    }
}
