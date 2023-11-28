using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.SharedInterfaces
{
	public interface IBookingRepository:IGenericRepository<Booking>
	{
		void Update(Booking booking);
		Task Save();
	}
}
