using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Reservations_Table_Layout
    {
        [Key]
        public int Id { get; set; }

        public virtual Reservations Reservation { get; set; }

        public virtual Table_layout Table_layout { get; set; }
    }
}