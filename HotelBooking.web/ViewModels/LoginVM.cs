using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Web.ViewModels
{
	public class LoginVM
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password{ get; set; }
        [Display(Name ="Rememebr Me")]
        public bool RemeberMe { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
