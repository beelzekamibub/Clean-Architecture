using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<Amenity> Amenities{ get; set; }
        public DbSet<AppUser> AppUsers{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Villa>().HasData(
				new Villa
				{
					Id = 1,
					Name = "Royal Villa",
					Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
					ImageUrl = "https://placehold.co/600x400",
					Occupancy = 4,
					Price = 200,
					Sqft = 550,
				},
				new Villa
				{
					Id = 2,
					Name = "Premium Pool Villa",
					Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
					ImageUrl = "https://placehold.co/600x401",
					Occupancy = 4,
					Price = 300,
					Sqft = 550,
				},
				new Villa
				{
					Id = 3,
					Name = "Luxury Pool Villa",
					Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
					ImageUrl = "https://placehold.co/600x402",
					Occupancy = 4,
					Price = 400,
					Sqft = 750,
				});

			modelBuilder.Entity<VillaNumber>().HasData(
				new VillaNumber
				{
					Villa_Number = 101,
					VillaId = 1
				},
				new VillaNumber
				{
					Villa_Number = 102,
					VillaId = 1
				},
				new VillaNumber
				{
					Villa_Number = 103,
					VillaId = 1
				},
				new VillaNumber
				{
					Villa_Number = 104,
					VillaId = 1
				},
				new VillaNumber
				{
					Villa_Number = 201,
					VillaId = 2
				},
				new VillaNumber
				{
					Villa_Number = 202,
					VillaId = 2
				},
				new VillaNumber
				{
					Villa_Number = 203,
					VillaId = 2
				},
				new VillaNumber
				{
					Villa_Number = 204,
					VillaId = 2
				},
				new VillaNumber
				{
					Villa_Number = 301,
					VillaId = 3
				},
				new VillaNumber
				{
					Villa_Number = 302,
					VillaId = 3
				},
				new VillaNumber
				{
					Villa_Number = 303,
					VillaId = 3
				},
				new VillaNumber
				{
					Villa_Number = 304,
					VillaId = 3
				}
			);
			modelBuilder.Entity<Amenity>().HasData(
				new Amenity { Name ="AC",Description="Fully Air Conditioned", AmenityId=1,VillaId=1},
				new Amenity { Name ="Wifi",Description="Unlimited Wifi", AmenityId=2,VillaId=1},
				new Amenity { Name ="Jaccuzi", AmenityId=3,VillaId=1},
				new Amenity { Name ="Swimming Pool", AmenityId=4,VillaId=1},

				new Amenity { Name ="AC",Description="Fully Air Conditioned", AmenityId=5,VillaId=9},
				new Amenity { Name ="Wifi",Description="Unlimited Wifi", AmenityId=6,VillaId=9},
				new Amenity { Name ="Jaccuzi", AmenityId=7,VillaId=9},
				new Amenity { Name ="Swimming Pool", AmenityId=8,VillaId=9},

				new Amenity { Name ="AC",Description="Fully Air Conditioned", AmenityId=9,VillaId=10},
				new Amenity { Name ="Wifi",Description="Unlimited Wifi", AmenityId=10,VillaId=10},
				new Amenity { Name ="Jaccuzi", AmenityId=11,VillaId=10},
				new Amenity { Name ="Swimming Pool", AmenityId=12,VillaId=10}
			);
		}
    }
}
