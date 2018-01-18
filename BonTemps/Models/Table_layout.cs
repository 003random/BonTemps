using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class Table_layout
    {
        [Key]
        public int TableLayoutId { get; set; }

        [Index("Layouts", 1, IsUnique = true)]
        [Required(ErrorMessage = "* required")]
        public int LayoutX { get; set; }

        [Index("Layouts", 2, IsUnique = true)]
        [Required(ErrorMessage = "* required")]
        public int LayoutY { get; set; }

        [Required(ErrorMessage = "* required")]
        public bool IsTable { get; set; }
    }
}