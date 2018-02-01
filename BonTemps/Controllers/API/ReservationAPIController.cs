using BonTemps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        public IHttpActionResult MakeReservation(ReservationViewModel reservationCustomer)
        {
            const int seats = 40;

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

            if (string.IsNullOrEmpty(customer.Email) || string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.LastName) || customer.Gender == 0 || Convert.ToInt32(customer.PhoneNumber) == 0)
                return Json("Niet alles is ingevuld. Controlleer de waardes en probeer het opnieuw.");

            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            return Json("Reservering sucesvol geplaatst.");
        }
    }
}