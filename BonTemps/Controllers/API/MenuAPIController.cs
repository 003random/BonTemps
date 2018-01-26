using BonTemps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BonTemps.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MenuAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Menus> GetMenus()
        {

            return db.Menus.ToList();
        }
    }
}
