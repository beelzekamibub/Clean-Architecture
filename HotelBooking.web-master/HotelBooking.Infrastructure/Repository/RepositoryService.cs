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
	public class RepositoryService : IRepositoryService
	{
		private readonly ApplicationDbContext _db;

		public RepositoryService(ApplicationDbContext db)
		{
			_db = db;
			Villa=new VillaRepository(_db);
			VillaNumber=new VillaNumberRepository(_db);
			Amenity= new AmenityRepository(_db);
		}

		public IVillaRepository Villa { get; private set; }
		public IVillaNumberRepository VillaNumber { get; private set; }
		public IAmenityRepository Amenity { get; private set; }
	}
}
