using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BonTemps.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=BonTempsDatabase", throwIfV1Schema: false)
        {
        }

        public DbSet<Allergies> Allergies { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Mail_credentials> Mail_credentials { get; set; }
        public DbSet<Menus_Allergies> Menus_Allergies { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Reservations_Table_Layout> Reservations_Table_Layout { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Table_layout> Table_layout { get; set; }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}