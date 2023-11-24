using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Web.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
