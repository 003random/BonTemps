using BonTemps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace BonTemps.Controllers.API
{
    public class MenuAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Menus> GetMenus()
        {
            //var jsonSerialiser = new JavaScriptSerializer();
            //var json = jsonSerialiser.Serialize(db.Menus.ToList());

            return db.Menus.ToList();
        }
    }
}
