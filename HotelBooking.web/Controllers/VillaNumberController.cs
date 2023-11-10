using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Application.Utility;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers
{
	[Authorize(Roles =StaticDetails.RoleAdmin)]
	public class VillaNumberController : Controller
	{
		
		private readonly IRepositoryService _repo;
		public VillaNumberController(IRepositoryService repo)
		{
			_repo=repo;
		}
		public async Task<IActionResult> Index()
		{
			var villaNumbers = await _repo.VillaNumber.GetAllByFilter(null,includeJoinsOn:"Villa");
			return View(villaNumbers);
		}

		public async Task<IActionResult> Create()
		{
			var l=await _repo.Villa.GetAllByFilter();

            IEnumerable<SelectListItem> VillasSelect = l.Select(x=>new SelectListItem
			{
				Text= x.Name,
				Value=x.Id.ToString(),
			});
			ViewData["VillasSelect"]=VillasSelect;
			ViewBag.VillasSelect = VillasSelect;
			return View(new VillaNumberVM { VillaList=VillasSelect });
		}

		[HttpPost]
		public async Task<IActionResult> Create(VillaNumberVM obj)
		{
			ModelState.Remove("VillaList");
			ModelState.Remove("VillaNumber.Villa");
			IEnumerable<VillaNumber> vns=await _repo.VillaNumber.GetAllByFilter(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
			if (vns.Count()>=1)
			{ 
				TempData["error"] = "This Villa Number Already exists.";
				var l = await _repo.Villa.GetAllByFilter();

                obj.VillaList= l.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                return View(obj);
            }

            if (ModelState.IsValid)
			{
				await _repo.VillaNumber.Add(obj.VillaNumber);
				await _repo.VillaNumber.Save();
				TempData["success"] = "Villa Number was created.";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				var l = await _repo.Villa.GetAllByFilter();
                obj.VillaList = l.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                TempData["error"] = "Villa could not be added.";
				return View(obj);
			}
		}
		[HttpGet]
		public async Task<IActionResult> Update(int villaNumberId)
		{
			IEnumerable<VillaNumber> vns=await _repo.VillaNumber.GetAllByFilter(x => x.Villa_Number == villaNumberId);
			if (vns.Count()==0)
			{
				TempData["error"] = "Could not find data for this villa room number";
				return RedirectToAction("Error", "Home");
			}
			var l = await _repo.Villa.GetAllByFilter();

            VillaNumberVM villaNumberVM = new()
			{
				VillaList = l.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
				VillaNumber = await _repo.VillaNumber.GetByFilter(x=>x.Villa_Number == villaNumberId)
            };
			return View(villaNumberVM);
		}

		[HttpPost]
		public async Task<IActionResult> Update(VillaNumberVM obj)
		{
            ModelState.Remove("VillaList");
            ModelState.Remove("VillaNumber.Villa");
            if (ModelState.IsValid)
            {
                _repo.VillaNumber.Update(obj.VillaNumber);
                await _repo.VillaNumber.Save();
                TempData["success"] = "Villa Number was Updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
				var l = await _repo.Villa.GetAllByFilter();
                obj.VillaList = l.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                TempData["error"] = "Villa could not be added.";
                return View(obj);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int villaNumberId)
        {
			IEnumerable<VillaNumber> vns = await _repo.VillaNumber.GetAllByFilter(x => x.Villa_Number == villaNumberId);
			if (vns.Count() == 0)
			{
				TempData["error"] = "Could not find data for this villa room number";
                return RedirectToAction("Error", "Home");
            }
			var l = await _repo.Villa.GetAllByFilter();
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = l.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = await _repo.VillaNumber.GetByFilter(x => x.Villa_Number == villaNumberId)
            };
            return View(villaNumberVM);
        }
        [HttpPost]
		public async Task<IActionResult> Delete(VillaNumberVM villaNumberVM)
		{
			_repo.VillaNumber.Remove(villaNumberVM.VillaNumber);
			await _repo.VillaNumber.Save();
			TempData["success"] = "Villa Number was deleted.";
			return RedirectToAction(nameof(Index));
		}
	}
}
