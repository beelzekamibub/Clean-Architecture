using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Repository
{
	public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
	{
		ApplicationDbContext _db;
        public AmenityRepository(ApplicationDbContext db):base(db)
        {
			_db = db;
        }
        public async Task Save()
		{
			await _db.SaveChangesAsync();
		}
		public void Update(Amenity amenity)
		{
			_db.Amenities.Update(amenity);
		}
	}
}
