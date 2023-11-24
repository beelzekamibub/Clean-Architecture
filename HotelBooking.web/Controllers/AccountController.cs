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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
				if (user == null)
				{
					ModelState.AddModelError("Email", "No such user");
					return View(loginVM);
				}
				if(!user.EmailConfirmed)
				{
					TempData["error"] = "Email not confirmed cant login right now";
					return View(loginVM);
				}
                if (await _userManager.IsLockedOutAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Account locked out due to multiple failed login attempts. Try again later.");
                    return View(loginVM);
                }
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password,loginVM.RemeberMe,lockoutOnFailure:true);

                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
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
                    if (result.IsLockedOut)
                    {
                        await _userManager.AccessFailedAsync(user);
                        ModelState.AddModelError(string.Empty, "Invalid login attempt. Account locked out. Try again later.");
                        return View(loginVM);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
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
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "No User with this email address exists");
                    return View(forgotPasswordViewModel);
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var reseturl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, Request.Scheme);
                await _emailService.SendEmail("Reset your password", reseturl, forgotPasswordViewModel.Email);
				TempData["Success"] = "Email sent";
                return View(forgotPasswordViewModel);
            }
            ModelState.AddModelError("Email", "Incorrect email");
            return View(forgotPasswordViewModel);
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string code)
        {
            if (code == null)
                return View("Error");
            ResetVM resetViewModel = new ResetVM { code = code, UserId = userId };
            return View(resetViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetVM resetViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(resetViewModel.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View(resetViewModel);
                }
                var result = await _userManager.ResetPasswordAsync(user, resetViewModel.code, resetViewModel.Password);

                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError("Password", "Passwords do not satisfy criterion");
                return View(resetViewModel);
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
					await _emailService.SendEmail("Confirm your email",confirmationLink,registerVM.Email);
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
				return RedirectToAction("Login", "Account");
			}
			else
			{
                TempData["success"] = "Email confirmation failed. Cant register with this email.";
				return RedirectToAction("Register", "Account");
			}
		}
    }
}
