using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.SharedInterfaces
{
	public interface IRepositoryService
	{
		IVillaNumberRepository VillaNumber{ get;}
        IVillaRepository Villa { get; }
        IAmenityRepository Amenity { get;}
    }
}
