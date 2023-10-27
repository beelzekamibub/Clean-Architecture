using HotelBooking.Domain.Entities;

namespace HotelBooking.Web.ViewModels
{
	public class HomeVM
	{
        public IEnumerable<Villa>? Villas { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
