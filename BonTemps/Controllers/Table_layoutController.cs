using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;

namespace BonTemps.Controllers
{
    public class Table_layoutController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var tableList = _db.Table_layout.ToList();
            var tableLayout = tableList.Select(l => l.LayoutY).Distinct().Select(i => tableList.Where(t => t.LayoutY == i).Distinct().ToList()).ToList();

            ViewBag.tableLayout = tableLayout;
            var minusTwoHours = DateTime.Now.AddHours(-2);
            var plusTwoHours = DateTime.Now.AddHours(2);

            //remove all where the date is not in the minus 2 and plus 2 hours range. (old reservations etc.)
            _db.Reservations_Table_Layout.RemoveRange(_db.Reservations_Table_Layout.Include(r => r.Reservation).Where(r => r.Reservation.Date < minusTwoHours || r.Reservation.Date > plusTwoHours));

            _db.SaveChanges();

            var upcommingReservations = _db.Reservations.Where(r => r.Date > minusTwoHours && r.Date < plusTwoHours).Include(r => r.Customer);
            ViewBag.Reservations = upcommingReservations;
            var reservationsTableLayout = _db.Reservations_Table_Layout.ToList().OrderBy(r => r.Reservation.Id);

            //Convert to modelview
            var reservationsTableLayoutViewModelList = reservationsTableLayout.Select(item => new Table_layout_ReservationsModelView {LayoutX = item.Table_layout.LayoutX, LayoutY = item.Table_layout.LayoutY, ReservationId = item.Reservation.Id}).ToList().OrderBy(r => r.ReservationId);

            ViewBag.ReservationsTableLayoutModelViewList = reservationsTableLayoutViewModelList;

            var today = DateTime.Now;
            var firstReservation = _db.Reservations.Where(t => t.Date >= today).OrderBy(t => t.Date);

            if (!upcommingReservations.Any() && firstReservation.Any())
            {
                TempData["messageInfo"] = "De eerst volgende reservering is om " + firstReservation.First().Date;
            }
            else if (!upcommingReservations.Any() && !firstReservation.Any())
            {
                TempData["messageInfo"] = "Er zijn geen opkomende reserveringen op het moment";
            }

            return View();
        }

        public ActionResult Edit()
        {
            var tableList = _db.Table_layout.ToList();
            var tableLayout = tableList.Select(l => l.LayoutY).Distinct().Select(i => tableList.Where(t => t.LayoutY == i).Distinct().ToList()).ToList();

            ViewBag.tableLayout = tableLayout;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int x, int y, bool table)
        {
            if (x == 0 || y == 0)
                return Json("error, x and/or y are empty");

            var layoutObject = _db.Table_layout.FirstOrDefault(t => t.LayoutX == x && t.LayoutY == y);

            if (layoutObject == null)
                return Json("error, x and/or y are empty");

            layoutObject.IsTable = table;

            _db.Table_layout.Attach(layoutObject);
            var entry = _db.Entry(layoutObject);
            entry.Property(e => e.IsTable).IsModified = true;

            _db.SaveChanges();

            return Json("Succes");
        }


        [HttpPost]
        public ActionResult Save(List<Table_layout_ReservationsModelView> list)
        {
            if (!list.Any())
                return Json("Error: Empty list.");

            var all = from t in _db.Reservations_Table_Layout select t;
            _db.Reservations_Table_Layout.RemoveRange(all);
            _db.SaveChanges();

            foreach (var item in list)
            {
                if (item.ReservationId == 0)
                    continue;

                var tableLayout = _db.Table_layout.FirstOrDefault(t => t.LayoutX == item.LayoutX && t.LayoutY == item.LayoutY);
                var reservation = _db.Reservations.FirstOrDefault(r => r.Id == item.ReservationId);

                if (reservation != null && tableLayout != null)
                    _db.Reservations_Table_Layout.Add(new Reservations_Table_Layout { Table_layout = tableLayout, Reservation = reservation });

            }

            _db.SaveChanges();
            return Json("Success");
        }
    }
}
