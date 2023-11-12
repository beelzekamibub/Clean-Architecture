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
	public class AccountController : Controller
	{
		private readonly IRepositoryService _repo;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IEmailService _emailService;

		public AccountController(IRepositoryService repo, 
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

		public IActionResult AccessDenied()
		{
			return View();
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index","Home");
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
				if(!user.EmailConfirmed)
				{
					TempData["error"] = "Email not confirmed cant login right now";
					return View(loginVM);
				}
				var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password,loginVM.RemeberMe,lockoutOnFailure:false);

                if (result.Succeeded)
                {
					if (string.IsNullOrEmpty(loginVM.RedirectUrl))
					{
						return RedirectToAction("Index", "Home");
					}
                    else
                    {
						return LocalRedirect(loginVM.RedirectUrl);
                    }
                }
				else
				{
					ModelState.AddModelError("", "Invalid login attempt.");
				}
               
                TempData["error"] = "Could not login";
                return View(loginVM);
            }
            else
            {
                TempData["error"] = "Invalid Details";
                return View(loginVM);
            }
        }
        public async Task<IActionResult> Register(string returnUrl = null)
		{
			returnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Content("~/") : returnUrl;
			if (!await _roleManager.RoleExistsAsync(StaticDetails.RoleAdmin))
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
			if(!string.IsNullOrEmpty(returnUrl))
				regsiterVM.RedirectUrl = returnUrl;
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
                    var confirmationLink = Url.Action("ConfirmEmail","Account",new { userId=Id,token=token,returnurl=registerVM.RedirectUrl},Request.Scheme);
					await _emailService.SendEmail(confirmationLink,registerVM.Email);
					if (!string.IsNullOrEmpty(registerVM.SelectedRole))
						await _userManager.AddToRoleAsync(user, registerVM.SelectedRole);
					else
						await _userManager.AddToRoleAsync(user, StaticDetails.RoleCustomer);
					TempData["success"] = "Registered please confirm your email, by clicking on the link emailed to you.";
					return RedirectToAction("Login", new {returnurl=registerVM.RedirectUrl});
                }
				foreach(var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				TempData["error"] = "Could not register user";
				registerVM.Roles = _roleManager.Roles.Select(x => new SelectListItem {
						Text = x.Name,
						Value = x.Name
					});
                return View(registerVM);
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
				//await _signInManager.SignInAsync(user, isPersistent: false);
				TempData["success"] = "Email verfied. You can login now.";
				return RedirectToAction("Index", "Home");
			}
			else
			{
                TempData["success"] = "Email confirmation failed. Cant register with this email.";
                return RedirectToAction("Index", "Home");
            }
		}
    }
}
