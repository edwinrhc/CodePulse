using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        override protected void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            var readerRoleId = "71239d38-94bc-47c1-ae16-9da222859f70";
            var writerRoleId = "855caaef-bb9a-4f4b-8967-f972b698944b";

            // Create Reader and Write Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id =  readerRoleId,
                    Name= "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId

                },
                new IdentityRole(){

                    Id = writerRoleId,
                    Name= "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };
            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create an Admin User
            var adminUserId = "58437530-16e5-4cb8-b44f-2a8d6ee96465";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles To Admin 

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new ()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new ()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                },
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
