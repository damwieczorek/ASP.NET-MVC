using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication2.Models;

namespace ProjekcikASP.Models
{
    // Możesz dodać dane profilu dla użytkownika, dodając więcej właściwości do klasy User. Odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=317594, aby dowiedzieć się więcej.
    public class User : IdentityUser
    {
        public virtual ICollection<Movies> Movie { get; set; }
        public virtual ICollection<RentHistory> RentHistory { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<RentHistory> RentHistory { get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Rating> Rating { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

      
    }
}