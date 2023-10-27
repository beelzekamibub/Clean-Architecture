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
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{

		internal DbSet<T> dbSet;
		public GenericRepository(ApplicationDbContext db)
		{
			dbSet=db.Set<T>();
		}
		public void Add(T obj)
		{
			dbSet.Add(obj);
		}

		public bool Any(Expression<Func<T, bool>> filter)
		{
			return dbSet.Any(filter);
		}

		public IEnumerable<T> GetAllByFilter(Expression<Func<T, bool>>? filter = null, string? includeJoinsOn = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter); //filter is a lambda expression on villa model that returns a bool
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
			return query.ToList();
		}

		public T GetByFilter(Expression<Func<T, bool>> filter, string? includeJoinsOn = null)
		{
			IQueryable<T> query = dbSet;
			query = query.Where(filter);
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

		public void Remove(T obj)
		{
			dbSet.Attach(obj);
			dbSet.Remove(obj);
		}
	}
}
