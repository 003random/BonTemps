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
                    int seed = new Random().Next(0, Int32.MaxValue - 301);
                    string[] firstNames = { "Zulema", "Veda", "Thomas", "Dara", "Verna", "Lisandra", "Theodore", "Kelli", "Dawne", "Ka", "Mikaela", "Melania", "Domonique", "Roselle", "Rosamond", "Hilario", "Nathan", "Rossie", "Gennie", "Devin", "Merlyn", "Marta", "Cristy", "Jeanett", "Perla", "Keitha", "Toby", "Adrianna", "Ehtel", "Sherrill", "Cyril", "Jennefer", "Valeria", "Sueann", "Racheal", "Leann", "Karey", "Felix", "Mikel", "Tawny", "Julieann", "Alvin", "Ashton", "Sherlyn", "Xochitl", "Margarette", "Jackqueline", "Filomena", "Maudie", "Beaulah" };
                    string[] lastNames = { "Bush", "Barber", "Brady", "Harmon", "Dudley", "Ball", "Trevino", "Weaver", "Cooley", "Ford", "Cole", "Bartlett", "Burke", "Porter", "Acevedo", "Vaughn", "Flowers", "Evans", "Cobb", "Mccullough", "Aguirre", "Galvan", "Mcdaniel", "Watts", "Krause", "Mathis", "Harvey", "Waters", "Duke", "Russo", "Boone", "Schmitt", "Shah", "Stone", "Spears", "Ponce", "Ochoa", "Yoder", "Buck", "Shields", "Anderson", "Zhang", "Bernard", "Zamora", "Wu", "Ray", "Heath", "Harrison", "Rasmussen", "Bridges" };
                    string firstname = firstNames[new Random(seed + i).Next(0, firstNames.Length - 1)];
                    string lastname = lastNames[new Random(seed - i).Next(0, lastNames.Length - 1)];
                    context.Customers.AddOrUpdate(
                        new Customers { Gender = GenderEnum.Man, FirstName = firstname, Prefix = "", LastName = lastname, PhoneNumber = "06" + new Random(seed).Next(12345678, 99999999).ToString(), Email = firstname + "."+ lastname + "@gmail.com", NewsLetter = true, DateCreated = DateTime.Now.AddDays(-new Random(seed).Next(0, 190)) }
                    );
                }
            }
            if (!context.Allergies.Any())
            {
                context.Allergies.AddOrUpdate(
                    new Allergies { Name = "Pinda", Image = "Pinda.png" },
                    new Allergies { Name = "Noten", Image = "Noten.png" },
                    new Allergies { Name = "Gluten", Image = "Gluten.png" },
                    new Allergies { Name = "Lactose", Image = "Lactose.png" },
                    new Allergies { Name = "Koemelk", Image = "Koemelk.jpg" }
                );
            }
            if (!context.Menus.Any())
            {
                context.Menus.AddOrUpdate(
                    new Menus { Name = "Vers Voorjaarsmenu", Description= "Voorgerecht: Spinaziesalade, Hoofdgerecht: Huisgerookte Zalm met wortels en peterselie saus ,Dessert: Verrassingsdessert", Image = "Mozzerela.jpg" },
                    new Menus { Name = "Veggi Menu", Description = "Voorgerecht: Geitenkaas in bladerdeeg met vijgen en honing , Hoofdgerecht: Buffelmozarella met salieboter, gedroogde tomaten en brie , Dessert: Tiramisu", Image = "Tonijn.jpg" },
                    new Menus { Name = "Meat Menu", Description = "Voorgerecht: Tomatensoep met verse basilicum  , Hoofdgerecht: Gegrilde varkensrib met tijm en kaassaus , Dessert: Sorbetijsselectie", Image = "Varkensrip.jpg" }
                );
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
