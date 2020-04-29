using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.Id).HasMaxLength(127));

			modelBuilder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(127));
			
            modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(127));

            
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(127));
			modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(127));
			modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
            
			modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
			modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(127));
			modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
			modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(127));
			modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(127));

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(e => e.Id).HasMaxLength(127));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(e => e.RoleId).HasMaxLength(127));

            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(e => e.Id).HasMaxLength(127));
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(e => e.UserId).HasMaxLength(127));

		}
    }

    
}
