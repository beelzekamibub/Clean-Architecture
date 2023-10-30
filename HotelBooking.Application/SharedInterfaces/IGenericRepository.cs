using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.SharedInterfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllByFilter(Expression<Func<T, bool>>? filter = null, string? includeJoinsOn = null);
		//this function takes a linq expression that applies a function on the villa model, and the function returns a boolean
		// the expression is nullable for when we have to get all
		Task<T> GetByFilter(Expression<Func<T, bool>> filter, string? includeJoinsOn = null);
		// when we want to load a single villa model based on some filter the filter can not be null
		Task Add(T obj);
		Task<bool> Any(Expression<Func<T,bool>> filter);
        void Remove(T obj);
	}
}
