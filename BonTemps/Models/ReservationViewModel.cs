using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BonTemps.Models
{
    public class ReservationViewModel
    {
        public int CustomerId { get; set; }

        public int ReservationId { get; set; }
        
        [Required(ErrorMessage = "* required")]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "* required")]
        [Display(Name = "Aantal personen")]
        public int Persons { get; set; }

        [Display(Name = "Geslacht")]
        [Required(ErrorMessage = "* required")]
        public GenderEnum Gender { get; set; }

        [Display(Name = "Voornaam")]
        [Required(ErrorMessage = "* required")]
        public string FirstName { get; set; }

        [Display(Name = "Tussenvoegsel")]
        public string Prefix { get; set; }

        [Display(Name = "Achternaam")]
        [Required(ErrorMessage = "* required")]
        public string LastName { get; set; }

        [Display(Name = "Geboortedatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "* required")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(14)]
        [MinLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "must be numeric")]
        [Display(Name = "Mobiel")]
        public string PhoneNumber { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "* required")]
        public string Email { get; set; }

        [Display(Name = "Nieuwsbrief")]
        [Required(ErrorMessage = "* required")]
        public bool NewsLetter { get; set; }

        public List<int> Menus { get; set; }
    }
}