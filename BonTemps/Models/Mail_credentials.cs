using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Mail_credentials
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "SMTP server")]
        [Required(ErrorMessage = "* required")]
        public string SMTPServer { get; set; }

        [Display(Name = "Gebruikersnaam")]
        [Required(ErrorMessage = "* required")]
        public string UserName { get; set; }

        [Display(Name = "Wachtwoord")]
        [Required(ErrorMessage = "* required")]
        public string Password { get; set; }

        [Display(Name = "Poort")]
        [Required(ErrorMessage = "* required")]
        public int Port { get; set; }
    }
}