using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Allergies
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Naam")]
        [Required(ErrorMessage = "* required")]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public string Image { get; set; }

        public Allergies()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}