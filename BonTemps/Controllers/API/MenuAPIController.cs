using BonTemps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BonTemps.Controllers.API
{
    public class MenuAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Menus> GetMenus()
        {
            return db.Menus.ToList();
        }
    }
}
