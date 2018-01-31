using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Menus
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Naam")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public string Image { get; set; }

        public Menus()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}