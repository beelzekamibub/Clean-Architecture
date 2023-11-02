using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelBooking.Application.Utility;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IRepositoryService _repo;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IEmailService _emailService;

		public AuthController(IRepositoryService repo, 
			UserManager<AppUser> userManager, 
			SignInManager<AppUser> signInManager, 
			RoleManager<IdentityRole> roleManager,
			IEmailService emailService)
		{
			_emailService = emailService;
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
		public async Task<IActionResult> Register()
		{
			if(!await _roleManager.RoleExistsAsync(StaticDetails.RoleAdmin))
				await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleAdmin));
            if (!await _roleManager.RoleExistsAsync(StaticDetails.RoleCustomer))
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleCustomer));

			RegisterVM regsiterVM = new RegisterVM()
			{
				Roles = _roleManager.Roles.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Name
				})
			};

            return View(regsiterVM);
		}
		[HttpPost]
		[AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
			if (ModelState.IsValid)
			{
                AppUser user = new AppUser()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    //EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAt = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var Id = user.Id;
                    var confirmationLink = Url.Action("ConfirmEmail","Auth",new { userId=Id,token=token},Request.Scheme);
					await _emailService.SendEmail(confirmationLink,registerVM.Email);
					TempData["success"] = "Registered please confirm your email, by clicking on the link emailed to you.";
					return RedirectToAction("Login");
                }
				TempData["error"] = "Could not register user";
                RegisterVM regsiterVM = new RegisterVM()
                {
                    Roles = _roleManager.Roles.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Name
                    })
                };

                return View(regsiterVM);
            }
            else
            {
                TempData["error"] = "Invalid Details";
                return View(registerVM);
            }
        }
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
			{
				TempData["error"] = "Email was not confirmed.";
				return RedirectToAction("Index", "Home");
			}
			var user = await _userManager.FindByIdAsync(userId);
			if(user == null)
			{
				TempData["error"] = "No User exists with this token/email Id.";
                return RedirectToAction("Index", "Home");
            }
			var res=await _userManager.ConfirmEmailAsync(user, token);
			if(res.Succeeded)
			{
				TempData["success"] = "Email verfied. You can login now.";
				return RedirectToAction("Login");
			}
			else
			{
                TempData["success"] = "Email confirmation failed. Cant register with this email.";
                return RedirectToAction("Register");
            }
		}
    }
}
