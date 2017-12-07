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
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Reservations> GetReservations()
        {
            return db.Reservations.ToList();
        }

        public bool MakeReservation(Reservations reservations)
        {
            return true;
        }
    }
}