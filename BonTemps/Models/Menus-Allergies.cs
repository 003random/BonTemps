using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Menus_Allergies
    {
        [Key]
        public int Id { get; set; }

        public virtual Menus Menu { get; set; }

        public virtual Allergies Allergie { get; set; }
    }
}