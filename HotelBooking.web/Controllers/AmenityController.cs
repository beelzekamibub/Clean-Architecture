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
	[Authorize(Roles = StaticDetails.RoleAdmin)]
	public class AmenityController : Controller
	{
		
		private readonly IRepositoryService _repo;
		public AmenityController(IRepositoryService repo)
		{
			_repo=repo;
		}
		public async Task<IActionResult> Index()
		{
			var amenities = await _repo.Amenity.GetAllByFilter(null, includeJoinsOn: "Villa");
			return View(amenities);
		}

		public async Task<IActionResult> Create()
		{
			var l = await _repo.Villa.GetAllByFilter();

            IEnumerable<SelectListItem> VillasSelect = l.Select(x=>new SelectListItem
			{
				Text= x.Name,
				Value=x.Id.ToString(),
			});
			ViewData["VillasSelect"]=VillasSelect;
			ViewBag.VillasSelect = VillasSelect;
			return View(new AmenityVM { VillaList=VillasSelect });
		}

		[HttpPost]
		public async Task<IActionResult> Create(AmenityVM obj)
		{
			ModelState.Remove("VillaList");
			ModelState.Remove("Amenity.Villa");

            if (ModelState.IsValid)
			{
				await _repo.Amenity.Add(obj.Amenity);
				await _repo.Amenity.Save();
				TempData["success"] = "Amenity was added successfully.";
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
                TempData["error"] = "Amenity could not be added.";
				return View(obj);
			}
		}
		[HttpGet]
		public async Task<IActionResult> Update(int amenityId)
		{
			IEnumerable<Amenity> vns=await _repo.Amenity.GetAllByFilter(x => x.AmenityId == amenityId);
			if (vns.Count()==0)
			{
				TempData["error"] = "Could not find data for this Amenity";
				return RedirectToAction("Error", "Home");
			}
			var l =await _repo.Villa.GetAllByFilter();

            AmenityVM amenityVM = new()
			{
				VillaList = l.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
				Amenity = await _repo.Amenity.GetByFilter(x => x.AmenityId == amenityId)
            };
			return View(amenityVM);
		}

		[HttpPost]
		public async Task<IActionResult> Update(AmenityVM obj)
		{
            ModelState.Remove("VillaList");
            ModelState.Remove("Amenity.Villa");
            if (ModelState.IsValid)
            {
                _repo.Amenity.Update(obj.Amenity);
                await _repo.Amenity.Save();
                TempData["success"] = "Amenity was Updated.";
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
                TempData["error"] = "Amenity could not be added.";
                return View(obj);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int amenityId)
        {
			IEnumerable<Amenity> vns = await _repo.Amenity.GetAllByFilter(x => x.AmenityId == amenityId);
			if (vns.Count() == 0)
			{
				TempData["error"] = "Could not find data for this Amenity";
                return RedirectToAction("Error", "Home");
            }
			var l = await _repo.Villa.GetAllByFilter();
            AmenityVM amenityVM = new()
            {
                VillaList = l.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Amenity = await _repo.Amenity.GetByFilter(x => x.AmenityId == amenityId)
            };
            return View(amenityVM);
        }
        [HttpPost]
		public async Task<IActionResult> Delete(AmenityVM amenityVM)
		{
			_repo.Amenity.Remove(amenityVM.Amenity);
			await _repo.Amenity.Save();
			TempData["success"] = "Amenity was deleted.";
			return RedirectToAction(nameof(Index));
		}
	}
}
