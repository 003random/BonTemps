using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class allergyMenuViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Allergies { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}