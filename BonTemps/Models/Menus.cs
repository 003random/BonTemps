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
        [Required(ErrorMessage = "* required")]
        public string Name { get; set; }

        public int MenuType { get; set; }
    }
}