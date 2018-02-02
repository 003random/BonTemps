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
    [Authorize(Roles = "Admin,Serveerster")]
    public class OrderController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menus
        public ActionResult Index()
        {
            return View(db.Orders.Include(o => o.Reservation).Include(o => o.Reservation.Customer).Include(o => o.Menu).ToList());
        }

        // GET: Menus/Create
        public ActionResult Create(int? reservationId)
        {
            var minusTwoHours = DateTime.Now.AddHours(-2);
            ViewBag.reservations = db.Reservations.Include(r => r.Customer).Where(r => r.Date > minusTwoHours).ToList();
            ViewBag.reservationId = reservationId;
            ViewBag.menus = db.Menus.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(string menus, int? reservationId)
        {
            ViewBag.menus = db.Menus.ToList();
            var minusTwoHours = DateTime.Now.AddHours(-2);
            ViewBag.reservations = db.Reservations.Include(r => r.Customer).Where(r => r.Date > minusTwoHours).ToList();

            if (reservationId == null || reservationId == 0)
            {
                ViewBag.reservationId = reservationId;
                TempData["error"] = "ReserveringId is leeg";
                return View();
            }
            ViewBag.menus = db.Menus.ToList();
            
            if (menus.StartsWith(","))
            {
                menus = menus.Substring(1);
            }

            foreach (var menu in menus.Split(','))
            {
                var intMenu = Convert.ToInt16(menu);
                var orderObj = new Orders
                {
                    Menu = db.Menus.FirstOrDefault(a => a.Id == intMenu),
                    Reservation = db.Reservations.FirstOrDefault(a => a.Id == reservationId)
                };

                db.Orders.Add(orderObj);
            }

            db.SaveChanges();
            TempData["success"] = "Sucesvol aangemaakt";
            return View();
        }

        // GET: Menus/Create
        public ActionResult Edit(int? id)
        {
            ViewBag.reservationId = id;
            var arr = db.Orders.Where(m => m.Reservation.Id == id).Select(m => m.Menu).Select(m => m.Id);
            ViewBag.menuArr = string.Join(",", arr);

            var minusTwoHours = DateTime.Now.AddHours(-2);
            ViewBag.reservations = db.Reservations.Include(r => r.Customer).Where(r => r.Date > minusTwoHours).ToList();
            ViewBag.menus = db.Menus.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string menus, int? reservationId)
        {
            ViewBag.reservationId = reservationId;
            ViewBag.menus = db.Menus.ToList();
            var minusTwoHours = DateTime.Now.AddHours(-2);
            ViewBag.reservations = db.Reservations.Include(r => r.Customer).Where(r => r.Date > minusTwoHours).ToList();

            if (reservationId == null || reservationId == 0)
            {
                ViewBag.reservationId = reservationId;
                TempData["error"] = "ReserveringId is leeg";
                return View();
            }
            ViewBag.menus = db.Menus.ToList();

            if (menus.StartsWith(","))
            {
                menus = menus.Substring(1);
            }

            db.Orders.RemoveRange(db.Orders.Where(m => m.Reservation.Id == reservationId));

            foreach (var menu in menus.Split(','))
            {
                var intMenu = Convert.ToInt16(menu);
                var orderObj = new Orders
                {
                    Menu = db.Menus.FirstOrDefault(a => a.Id == intMenu),
                    Reservation = db.Reservations.FirstOrDefault(a => a.Id == reservationId)
                };

                db.Orders.Add(orderObj);
            }

            db.SaveChanges();
            TempData["success"] = "Sucesvol opgeslagen";

            var arr = db.Orders.Where(m => m.Reservation.Id == reservationId).Select(m => m.Menu).Select(m => m.Id);
            ViewBag.menuArr = string.Join(",", arr);

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
        }
}
