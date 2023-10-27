using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers
{
	public class AmenityController : Controller
	{
		
		private readonly IRepositoryService _repo;
		public AmenityController(IRepositoryService repo)
		{
			_repo=repo;
		}
		public IActionResult Index()
		{
			var amenities = _repo.Amenity.GetAllByFilter(null,includeJoinsOn:"Villa");
			return View(amenities);
		}

		public IActionResult Create()
		{
			IEnumerable<SelectListItem> VillasSelect = _repo.Villa.GetAllByFilter().Select(x=>new SelectListItem
			{
				Text= x.Name,
				Value=x.Id.ToString(),
			});
			ViewData["VillasSelect"]=VillasSelect;
			ViewBag.VillasSelect = VillasSelect;
			return View(new AmenityVM { VillaList=VillasSelect });
		}

		[HttpPost]
		public IActionResult Create(AmenityVM obj)
		{
			ModelState.Remove("VillaList");
			ModelState.Remove("Amenity.Villa");

/*			if (_repo.Amenity.Any(x=>x.AmenityId==obj.Amenity.AmenityId))
			{ 
				TempData["error"] = "This Amenity Already exists.";
				obj.VillaList= _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                return View(obj);
            }*/

            if (ModelState.IsValid)
			{
				_repo.Amenity.Add(obj.Amenity);
				_repo.Amenity.Save();
				TempData["success"] = "Amenity was added successfully.";
				return RedirectToAction(nameof(Index));
			}
			else
			{
                obj.VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                TempData["error"] = "Amenity could not be added.";
				return View(obj);
			}
		}
		[HttpGet]
		public IActionResult Update(int amenityId)
		{
			IEnumerable<Amenity> vns=_repo.Amenity.GetAllByFilter(x => x.AmenityId == amenityId);
			if (vns.Count()==0)
			{
				TempData["error"] = "Could not find data for this Amenity";
				return RedirectToAction("Error", "Home");
			}
			AmenityVM amenityVM = new()
			{
				VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
				Amenity = _repo.Amenity.GetByFilter(x=>x.AmenityId == amenityId)
            };
			return View(amenityVM);
		}

		[HttpPost]
		public IActionResult Update(AmenityVM obj)
		{
            ModelState.Remove("VillaList");
            ModelState.Remove("Amenity.Villa");
            if (ModelState.IsValid)
            {
                _repo.Amenity.Update(obj.Amenity);
                _repo.Amenity.Save();
                TempData["success"] = "Amenity was Updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                obj.VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                TempData["error"] = "Amenity could not be added.";
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult Delete(int amenityId)
        {
			IEnumerable<Amenity> vns = _repo.Amenity.GetAllByFilter(x => x.AmenityId == amenityId);
			if (vns.Count() == 0)
			{
				TempData["error"] = "Could not find data for this Amenity";
                return RedirectToAction("Error", "Home");
            }
            AmenityVM amenityVM = new()
            {
                VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Amenity = _repo.Amenity.GetByFilter(x => x.AmenityId == amenityId)
            };
            return View(amenityVM);
        }
        [HttpPost]
		public IActionResult Delete(AmenityVM amenityVM)
		{
			_repo.Amenity.Remove(amenityVM.Amenity);
			_repo.Amenity.Save();
			TempData["success"] = "Amenity was deleted.";
			return RedirectToAction(nameof(Index));
		}
	}
}
