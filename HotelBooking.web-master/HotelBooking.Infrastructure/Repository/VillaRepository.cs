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
	public class VillaRepository : IVillaRepository
	{
		private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db)
        {
			_db = db;
        }
        public void Add(Villa villa)
		{
			_db.Add(villa);
		}

		public IEnumerable<Villa> GetAllByFilter(Expression<Func<Villa, bool>>? filter = null, string? includeJoinsOn = null)
		{
			IQueryable<Villa> query = _db.Villas;
			if(filter != null)
			{
				query.Where(filter); //filter is a lambda expression on villa model that returns a bool
			}
			if(!string.IsNullOrEmpty(includeJoinsOn))
			{
				//Villa,VillaNumber -- case sensitive
				string[] joinsOn = includeJoinsOn.Split(',',StringSplitOptions.RemoveEmptyEntries);
				foreach(string on in  joinsOn)
				{
					query=query.Include(on);
				}
			}
			return query.ToList();
		}

		public Villa GetByFilter(Expression<Func<Villa, bool>> filter, string? includeJoinsOn = null)
		{
			IQueryable<Villa> query = _db.Villas;
			if (filter != null)
			{
				query.Where(filter); //filter is a lambda expression on villa model that returns a bool
			}
			if (!string.IsNullOrEmpty(includeJoinsOn))
			{
				//Villa,VillaNumber -- case sensitive
				string[] joinsOn = includeJoinsOn.Split(',', StringSplitOptions.RemoveEmptyEntries);
				foreach (string on in joinsOn)
				{
					query = query.Include(on);
				}
			}
			return query.FirstOrDefault();
		}

		public void Remove(Villa villa)
		{
			//_db.Remove(villa);
			_db.Villas.Where(x=>x.Id==villa.Id).ExecuteDelete();
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
