using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Web.ViewModels
{
	public class RegisterVM
	{
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password))]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }

		[Required]
        [EmailAddress]
        public string Email{ get; set; }

        [Required]
        public string Name { get; set; }

		[Required]
		[Display(Name = "User Name")]
		public string UserName { get; set; }

		[DataType(DataType.PhoneNumber)]
        [Display(Name="Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? RedirectUrl { get; set; }

	}
}
