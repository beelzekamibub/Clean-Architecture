using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
	public class Booking
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User{ get; set; }

		[Required]
		public int VillaId { get; set; }
		[ForeignKey("Id")]
		public Villa Villa{ get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [Required]
        public double TotalCost { get; set; }
        public int Nights { get; set; }
        public string? Status { get; set; }//approved,checked in,checked out,cancelled 

        [Required]
        public DateTime BookingDate { get; set; }
		[Required]
		public DateTime CheckInDate { get; set; }
		[Required]
		public DateTime CheckOut { get; set; }
        public bool IsPaymentSuccesful { get; set; } = false;
        public DateTime PaymentDate { get; set; }
        public string? StripeSessionId { get; set; }
        public string? StringPaymentIntentId { get; set; }
        public DateTime ActualCheckInDate { get; set; }
		public DateTime ActualCheckOutDate { get; set; }
        public int VillaNumber { get; set; }

	}
}
