using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class RolesUsersModelView
    {
        public string UserId { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Gebruikersnaam")]
        public string Username { get; set; }
    }
}