using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;
using Microsoft.AspNet.Identity;

namespace BonTemps.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser != null) ViewBag.username = currentUser.UserName;

            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            int months = 5;

            int[] bestellingen = new int[months];

            for (var i = 0; i < months; i++)
            {
                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month, 1);
                DateTime first;
                DateTime last;
                string keymonth;

                if (i != 0)
                {
                    first = month.AddMonths(-i);
                    last = month.AddMonths(-(i - 1)).AddDays(-1);
                    keymonth = month.AddMonths(-(i - 1)).AddDays(-1).ToString("MMM");
                }
                else
                {
                    first = month.AddMonths(0);
                    last = month.AddMonths(1).AddDays(-1);
                    keymonth = month.AddMonths(1).AddDays(-1).ToString("MMM");
                }
                int count = db.Customers.Count(m => m.DateCreated >= first && m.DateCreated <= last);
                bestellingen[i] = db.Reservations.Count(m => m.DateCreated >= first && m.DateCreated <= last);
                dictionary.Add(keymonth, count);
            }
            ViewBag.lastMonthsData = dictionary.Reverse();
            ViewBag.lastMonthsReservations = bestellingen.Reverse();

            Dictionary<string, int> lastWeekReservations = new Dictionary<string, int>();
            int[] customerCount = new int[7];

            for (var i = 0; i < 6; i++)
            {
                DateTime startDateTime = DateTime.Today;
                DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1);
                DateTime first;
                DateTime last;

                first = startDateTime.AddDays(-i);
                last = endDateTime.AddDays(-i);

                int count = db.Reservations.Count(m => m.DateCreated >= first && m.DateCreated <= last);
                int custCount = db.Customers.Count(m => m.DateCreated >= first && m.DateCreated <= last);

                lastWeekReservations.Add(first.DayOfWeek.ToString(), count);
                customerCount[i] = custCount;
            }
            ViewBag.lastWeekReservations = lastWeekReservations;
            ViewBag.lastWeekCustomers = customerCount;


            return View();
        }
    }
}