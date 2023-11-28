namespace HotelBooking.Application.SharedInterfaces
{
	public interface IRepositoryService
	{
		IVillaNumberRepository VillaNumber{ get;}
        IVillaRepository Villa { get; }
        IAmenityRepository Amenity { get;}
		IBookingRepository Booking { get; }
    }
}
