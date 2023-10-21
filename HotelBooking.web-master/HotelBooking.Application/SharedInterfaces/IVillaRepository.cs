using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.SharedInterfaces
{
	public interface IVillaRepository
	{
		Task<IEnumerable<Villa>> GetAllByFilter(Expression<Func<Villa, bool>>? filter = null, string? includeJoinsOn = null);
		//this function takes a linq expression that applies a function on the villa model, and the function returns a boolean
		// the expression is nullable for when we have to get all
		Task<IEnumerable<Villa>> GetByFilter(Expression<Func<Villa, bool>> filter, string? includeJoins = null);
		// when we wan to load a single villa model based on some filter the filter can not be null
		void Add(Villa villa);
		void Update(Villa villa);
		void Remove(Villa villa);
		void Save();
	}
}
