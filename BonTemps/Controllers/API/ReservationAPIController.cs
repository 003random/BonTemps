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

        public bool MakeReservation(ReservationViewModel reservationCustomer)
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

            if (string.IsNullOrEmpty(customer.Email) && string.IsNullOrEmpty(customer.FirstName) &&
                string.IsNullOrEmpty(customer.LastName) && customer.Gender == 0 &&
                customer.PhoneNumber == 0) return false;

            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            return true;
        }
    }
}