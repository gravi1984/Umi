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
    public class AppDbContext : IdentityDbContext<IdentityUser> // DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public  DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

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
            
            
                
                
                
            base.OnModelCreating(modelBuilder);
        }
    }
}