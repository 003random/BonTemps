using BonTemps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace BonTemps.Controllers
{
    [Authorize(Roles = "kok,Admin")]
    public class ChefController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chef
        public ActionResult Index()
        {
            var first = DateTime.Now.AddHours(-2);
            var last = DateTime.Now.AddHours(2);
            var orders = db.Orders.Include(o => o.Menu).Include(o => o.Reservation).Include(o => o.Reservation.Customer).Where(o => o.Reservation.Date > first && o.Reservation.Date < last).ToList();
            return View(orders);
        }
    }
}