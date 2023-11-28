using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers
{
	public class BookingController : Controller
	{
		private readonly IRepositoryService _repositoryService;

		public BookingController(IRepositoryService repositoryService)
		{
			_repositoryService = repositoryService;
		}

		public async Task<IActionResult> Book(int villaId,DateTime checkIn, int nights)
		{
            checkIn=checkIn.AddHours(12);
            Booking booking = new Booking()
			{
				VillaId = villaId,
				Villa = await _repositoryService.Villa.GetByFilter(u => u.Id == villaId, includeJoinsOn: "Amenities"),
				CheckInDate = checkIn,
				Nights = nights,
				CheckOut = checkIn.AddDays(nights)
			};
			booking.TotalCost = booking.Villa.Price * nights;
			
			return View(booking);
		}
	}
}
