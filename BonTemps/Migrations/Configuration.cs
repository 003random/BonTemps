using BonTemps.Models;

namespace BonTemps.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BonTemps.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BonTemps.Models.ApplicationDbContext context)
        {
            //ToDo: increase test data
            if (!context.Table_layout.Any())
            {
                //Seed table layout
                for (var i = 1; i < 9; i++)//loop 8 times
                {
                    for (var j = 1; j < 9; j++)//loop 8 times
                    {
                        context.Table_layout.AddOrUpdate(
                            new Table_layout { LayoutX = i, LayoutY = j, IsTable = true }
                        );
                    }
                }
            }

            if (!context.Customers.Any())
            {
                //seed customers table
                for (var i = 0; i < 33; i++)
                {
                    context.Customers.AddOrUpdate(
                        new Customers { Gender = GenderEnum.man, FirstName = "Customer" + i, Prefix = "", LastName = "Seed" + i, PhoneNumber = 061223123 + i, Email = "Customer" + i + "@localhost", NewsLetter = true, DateCreated = DateTime.Now.AddDays(-new Random().Next(0, 190)) }
                    );
                }
            }
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login  
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup I am creating first Admin Role and creating a default Admin User   
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }

            if (context.Users.Any(u => u.UserName == "admin@bontemps.com"))
            {
                var user = new ApplicationUser();
                user.UserName = "admin@bontemps.com";
                user.Email = "admin@bontemps.com";

                string userPWD = "Qwerty123#";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin  
                if (chkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Admin");

                }
            }


            // Create Serveerster role
            if (!roleManager.RoleExists("Serveerster"))
            {

                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = "Serveerster"
                };
                roleManager.Create(role);
            }

            if (context.Users.Any(u => u.UserName == "serveerster@bontemps.com"))
            {
                var user = new ApplicationUser();
                user.UserName = "serveerster@bontemps.com";
                user.Email = "serveerster@bontemps.com";

                string userPWD = "Qwerty123#";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin  
                if (chkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Serveerster");

                }
            }

            // Create Kok role
            if (!roleManager.RoleExists("kok"))
            {

                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = "kok"
                };
                roleManager.Create(role);
            }

            if (context.Users.Any(u => u.UserName == "kok@bontemps.com"))
            {
                var user = new ApplicationUser();
                user.UserName = "kok@bontemps.com";
                user.Email = "kok@bontemps.com";

                string userPWD = "Qwerty123#";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role kok  
                if (chkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "kok");

                }
            }

        }
    }
}
