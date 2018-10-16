using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HL_V1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string HomeTown { get; set; }
        public System.DateTime? BirthDate { get; set; }

        public enum userTypes
        {
            Nutritionist=1,NormalUser=2
        }

        public userTypes userType { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        public virtual ICollection<Reservation> Reservations_NId { get; set; }
        public virtual ICollection<Reservation> Reservations_UId { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<HL_V1.Models.RegisterViewModel> RegisterViewModels { get; set; }

        public System.Data.Entity.DbSet<HL_V1.Models.ArticleApproveViewModel> ArticleApproveViewModels { get; set; }

        public System.Data.Entity.DbSet<HL_V1.Models.Reservation> Reservations { get; set; }

        //public System.Data.Entity.DbSet<HL_V1.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}