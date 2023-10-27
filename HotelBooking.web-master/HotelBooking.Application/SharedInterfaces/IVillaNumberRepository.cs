using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.SharedInterfaces
{
	public interface IVillaNumberRepository:IGenericRepository<VillaNumber>
	{
		void Update(VillaNumber villaNumber);
		void Save();
	}
}
