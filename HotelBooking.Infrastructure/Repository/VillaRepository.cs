using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Repository
{
	public class VillaRepository : GenericRepository<Villa>, IVillaRepository
	{
		private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db):base(db)
        {
			_db = db;
        }
		public void Save()
		{
			_db.SaveChanges();
		}
		public void Update(Villa villa)
		{
			_db.Villas.Update(villa);
		}
	}
}
