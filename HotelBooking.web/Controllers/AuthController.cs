using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IRepositoryService _repo;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AuthController(IRepositoryService repo, 
			UserManager<AppUser> userManager, 
			SignInManager<AppUser> signInManager, 
			RoleManager<IdentityRole> roleManager)
		{
			_repo = repo;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		public IActionResult Login(string returnUrl=null)
		{
			returnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Content("~/") : returnUrl;
			LoginVM loginVM = new LoginVM()
			{
				RedirectUrl = returnUrl
			};
			return View(loginVM);
		}
		public IActionResult Register()
		{
			return View();
		}
	}
}
