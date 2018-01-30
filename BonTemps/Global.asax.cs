using BonTemps.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BonTemps
{
    public class MvcApplication : HttpApplication
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        protected void Application_Start()
        {   
            //System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("nl-NL");
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

            var store = new UserStore<ApplicationUser>(_db);
            var manager = new ApplicationUserManager(store);
            var username = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == username);
                if (user == null) return;
                var roles = manager.GetRolesAsync(user.Id).Result;
                Session["roles"] = roles;
            }
        }
    }
}
