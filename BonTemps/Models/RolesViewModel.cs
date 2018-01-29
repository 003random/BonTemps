using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonTemps.Models
{
    public class RolesViewModel
    {
        public IEnumerable<string> RoleNames { get; set; }
        public string UserName { get; set; }
    }
}