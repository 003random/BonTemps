using BonTemps.Models;

namespace BonTemps.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //ToDo: increase test data
            if (!context.Table_layout.Any())
            {
                //Seed table layout
                for (var i = 1; i < 15; i++)//loop 8 times
                {
                    for (var j = 1; j < 15; j++)//loop 8 times
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
                for (var i = 0; i < 300; i++)
                {
                    int seed = new Random().Next(0, Int32.MaxValue);
                    context.Customers.AddOrUpdate(
                        new Customers { Gender = GenderEnum.Man, FirstName = "Customer" + i, Prefix = "", LastName = "Seed" + i, PhoneNumber = "061223123" + i, Email = "Customer" + i + "@localhost", NewsLetter = true, DateCreated = DateTime.Now.AddDays(-new Random(seed).Next(0, 190)) }
                    );
                }
            }
            context.SaveChanges();
            if (!context.Reservations.Any())
            {
                //seed reservations table
                for(var i = 0; i < 300; i++)
                {
                    int seed = new Random().Next(0, Int32.MaxValue);
                    context.Reservations.AddOrUpdate(
                        new Reservations { Date = DateTime.Now.AddDays(-new Random(seed).Next(-10, 190)), Persons = new Random(seed).Next(1, 10), DateCreated = DateTime.Now.AddDays(-new Random(seed).Next(0, 190)), Customer = context.Customers.OrderBy(c => c.Id).Skip(new Random(seed).Next(0, 299)).First() }
                    );
                }
                context.SaveChanges();
            }
            CreateRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login  
        private void CreateRolesandUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            #region admin
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@bontemps.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@bontemps.com", Email = "admin@bontemps.com" };

                manager.Create(user, "Qwerty123#");
                manager.AddToRole(user.Id, "Admin");
            }
            #endregion

            #region Serveerster
            if (!context.Roles.Any(r => r.Name == "Serveerster"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Serveerster" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "serveerster@bontemps.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "serveerster@bontemps.com", Email = "serveerster@bontemps.com" };

                manager.Create(user, "Qwerty123#");
                manager.AddToRole(user.Id, "Serveerster");
            }
            #endregion

            #region Kok
            if (!context.Roles.Any(r => r.Name == "Kok"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Kok" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "kok@bontemps.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "kok@bontemps.com", Email = "kok@bontemps.com" };

                manager.Create(user, "Qwerty123#");
                manager.AddToRole(user.Id, "Kok");
            }
            #endregion

            #region defaultUserRole
            if (!context.Roles.Any(r => r.Name == "Default"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Default" };

                manager.Create(role);
            }
            #endregion
        }
    }
}
