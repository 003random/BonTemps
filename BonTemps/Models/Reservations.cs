using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Reservations
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "* required")]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "* required")]
        [Display(Name = "Aantal personen")]
        public int Persons { get; set; }

        public virtual Customers Customer { get; set; }

        public DateTime DateCreated { get; set; }

        public Reservations()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}