using HotelBooking.Application.SharedInterfaces;
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
		}

		public IVillaRepository Villa { get; set; }
		public IVillaNumberRepository VillaNumber { get; set; }
	}
}
