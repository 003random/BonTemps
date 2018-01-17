using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public enum GenderEnum
    {
        Overig,
        man,
        woman
            }

    public class Customers
    {
        [Key]
        public int Id { get; set; }

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
        public string PhoneNumber { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "* required")]
        public string Email { get; set; }

        [Display(Name = "Nieuwsrief")]
        [Required(ErrorMessage = "* required")]
        public bool NewsLetter { get; set; }

        public DateTime DateCreated { get; set; }

        public Customers()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}