﻿using System;
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

        [Display(Name = "Telefoon nummer")]
        [Required(ErrorMessage = "* required")]
        public int PhoneNumber { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "* required")]
        public string Email { get; set; }

        [Display(Name = "Nieuwsbrief")]
        [Required(ErrorMessage = "* required")]
        public bool NewsLetter { get; set; }

        List<Menus> Menus = new List<Menus>();
    }
}