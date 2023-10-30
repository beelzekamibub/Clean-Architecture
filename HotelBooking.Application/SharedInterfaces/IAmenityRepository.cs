using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.SharedInterfaces
{
	public interface IAmenityRepository:IGenericRepository<Amenity>
	{
		void Update(Amenity amenity);
		Task Save();
	}
}
