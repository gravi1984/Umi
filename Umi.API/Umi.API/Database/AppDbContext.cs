using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Umi.API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Umi.API.Database
{

    // overwrite identityUser to customize user
    public class AppDbContext : IdentityDbContext<ApplicationUser> // DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public  DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }
        
        // new DataModel
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        
        public DbSet<Order> Orders { get; set; }

        // customize ORM mapping, e.g. change mapped table name, DataAnnotation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<TouristRoute>().HasData(new TouristRoute()
            // {
            //     Id = Guid.NewGuid(),
            //     Title = "test title",
            //     Description = "description",
            //     OriginalPrice = 0,
            //     CreateTime = DateTime.Now
            // });

            // use Json file to seeding test data instead of typing hard in AppDbContext
            //@"sf"=> c# string unescape
            var touristRouteJsonData = File.ReadAllText(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                @"/Database/touristRoutesMockData.json");
            IList<TouristRoute> touristRoutes =
                JsonConvert.DeserializeObject<IList<TouristRoute>>(touristRouteJsonData);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);
            
            var touristRoutePictureJsonData =
                File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                 @"/Database/touristRoutesPictureMockData.json");
            IList<TouristRoutePicture> touristRoutePictures =
                JsonConvert.DeserializeObject<IList<TouristRoutePicture>>(touristRoutePictureJsonData);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristRoutePictures);
            
            // init User - Role seed
            
            // 1. User - Role FK
            modelBuilder.Entity<ApplicationUser>(u =>
            {
                u.HasMany(x => x.UserRoles)
                    .WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });
            
            // 2. add Admin role
            var adminRoleId = "R-000";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                });
            
            // 3. add User 
            var adminUserId = "U-OOO";
            ApplicationUser adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@umi.com",
                NormalizedUserName = "admin@umi.com".ToUpper(),
                Email = "admin@umi.com",
                NormalizedEmail = "admin@umi.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456",
                PhoneNumberConfirmed = false
            };
            var ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "fake123$");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);
            
            
            // 4. add Admin to User
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                });
            
            modelBuilder.Entity<ShoppingCart>(s =>
            {
                s.HasMany(s => s.ShoppingCartItems)
                    .WithOne().HasForeignKey(li => li.ShoppingCardId).IsRequired();
            });

            
            base.OnModelCreating(modelBuilder);
        }
    }
}