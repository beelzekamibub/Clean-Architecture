using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers
{
	public class VillaNumberController : Controller
	{
		
		private readonly IRepositoryService _repo;
		public VillaNumberController(IRepositoryService repo)
		{
			_repo=repo;
		}
		public IActionResult Index()
		{
			var villaNumbers = _repo.VillaNumber.GetAllByFilter(null,includeJoinsOn:"Villa");
			return View(villaNumbers);
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
			return View(new VillaNumberVM { VillaList=VillasSelect });
		}

		[HttpPost]
		public IActionResult Create(VillaNumberVM obj)
		{
			ModelState.Remove("VillaList");
			ModelState.Remove("VillaNumber.Villa");
			IEnumerable<VillaNumber> vns=_repo.VillaNumber.GetAllByFilter(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
			if (vns.Count()>=1)
			{ 
				TempData["error"] = "This Villa Number Already exists.";
				obj.VillaList= _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                return View(obj);
            }

            if (ModelState.IsValid)
			{
				_repo.VillaNumber.Add(obj.VillaNumber);
				_repo.VillaNumber.Save();
				TempData["success"] = "Villa Number was created.";
				return RedirectToAction(nameof(Index));
			}
			else
			{
                obj.VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                TempData["error"] = "Villa could not be added.";
				return View(obj);
			}
		}
		[HttpGet]
		public IActionResult Update(int villaNumberId)
		{
			IEnumerable<VillaNumber> vns=_repo.VillaNumber.GetAllByFilter(x => x.Villa_Number == villaNumberId);
			if (vns.Count()==0)
			{
				TempData["error"] = "Could not find data for this villa room number";
				return RedirectToAction("Error", "Home");
			}
			VillaNumberVM villaNumberVM = new()
			{
				VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
				VillaNumber = _repo.VillaNumber.GetByFilter(x=>x.Villa_Number == villaNumberId)
            };
			return View(villaNumberVM);
		}

		[HttpPost]
		public IActionResult Update(VillaNumberVM obj)
		{
            ModelState.Remove("VillaList");
            ModelState.Remove("VillaNumber.Villa");
            if (ModelState.IsValid)
            {
                _repo.VillaNumber.Update(obj.VillaNumber);
                _repo.VillaNumber.Save();
                TempData["success"] = "Villa Number was Updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                obj.VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                TempData["error"] = "Villa could not be added.";
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult Delete(int villaNumberId)
        {
			IEnumerable<VillaNumber> vns = _repo.VillaNumber.GetAllByFilter(x => x.Villa_Number == villaNumberId);
			if (vns.Count() == 0)
			{
				TempData["error"] = "Could not find data for this villa room number";
                return RedirectToAction("Error", "Home");
            }
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _repo.Villa.GetAllByFilter().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = _repo.VillaNumber.GetByFilter(x => x.Villa_Number == villaNumberId)
            };
            return View(villaNumberVM);
        }
        [HttpPost]
		public IActionResult Delete(VillaNumberVM villaNumberVM)
		{
			_repo.VillaNumber.Remove(villaNumberVM.VillaNumber);
			_repo.VillaNumber.Save();
			TempData["success"] = "Villa Number was deleted.";
			return RedirectToAction(nameof(Index));
		}
	}
}
