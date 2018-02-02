using BonTemps.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BonTemps.Controllers
{
    public class ReservationAPIController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public IEnumerable<Reservations> GetReservations()
        {
            return _db.Reservations.ToList();
        }

        [HttpPost]
        [EnableCors("*","*","*")]
        public IHttpActionResult MakeReservation(ReservationViewModel reservationCustomer)
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
            
            if ((reservationCustomer.Date.DayOfWeek == DayOfWeek.Monday) ||
                (reservationCustomer.Date.DayOfWeek == DayOfWeek.Tuesday && (reservationCustomer.Date.Hour < tuesdayStartHour || reservationCustomer.Date.Hour > tuesdayStopHour)) ||
                (reservationCustomer.Date.DayOfWeek == DayOfWeek.Wednesday && (reservationCustomer.Date.Hour < wednesdayStartHour || reservationCustomer.Date.Hour > wednesdayStopHour)) ||
                (reservationCustomer.Date.DayOfWeek == DayOfWeek.Thursday && (reservationCustomer.Date.Hour < thursdayStartHour || reservationCustomer.Date.Hour > thursdayStopHour)) ||
                (reservationCustomer.Date.DayOfWeek == DayOfWeek.Friday && (reservationCustomer.Date.Hour < fridayStartHour || reservationCustomer.Date.Hour > fridayStopHour)) ||
                (reservationCustomer.Date.DayOfWeek == DayOfWeek.Saturday && (reservationCustomer.Date.Hour < saturdayStartHour || reservationCustomer.Date.Hour > saturdayStopHour)) ||
                (reservationCustomer.Date.DayOfWeek == DayOfWeek.Sunday && (reservationCustomer.Date.Hour < sundayStartHour || reservationCustomer.Date.Hour > sundayStopHour)))
            {
                var culture = new CultureInfo("nl-NL");
                return Json("Op " + culture.DateTimeFormat.GetDayName(reservationCustomer.Date.DayOfWeek) + " " + Convert.ToString(reservationCustomer.Date, CultureInfo.InvariantCulture).Split(' ')[1] + " is Bon Temps gesloten.");
            }
            
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
                return Json("er zijn nog " + freeSeats + " stoelen vrij op " + reservationCustomer.Date);
            }
            
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

            if (string.IsNullOrEmpty(customer.Email) || string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.LastName) || (customer.Gender != GenderEnum.Man &&  customer.Gender != GenderEnum.Vrouw) || Convert.ToUInt64(customer.PhoneNumber) == 0)
                return Json("Niet alles is ingevuld. Controlleer de waardes en probeer het opnieuw.");

            _db.Customers.Add(customer);
            //_db.SaveChanges();

            reservation.Customer = customer;

            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            return Json("Reservering sucesvol geplaatst.");
        }
    }
}