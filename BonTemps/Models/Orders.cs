using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }

        public virtual Menus Menu { get; set; }

        public virtual Reservations Reservation { get; set; }
        public DateTime DateCreated { get; set; }

        public Orders()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}