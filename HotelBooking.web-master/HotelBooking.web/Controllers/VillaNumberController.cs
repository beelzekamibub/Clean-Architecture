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
		private readonly ApplicationDbContext _db;
		public VillaNumberController(ApplicationDbContext context)
		{
			_db = context;
		}
		public IActionResult Index()
		{
			var villaNumbers = _db.VillaNumbers.Include(x=>x.Villa).ToList();
			return View(villaNumbers);
		}

		public IActionResult Create()
		{
			IEnumerable<SelectListItem> VillasSelect = _db.Villas.ToList().Select(x=>new SelectListItem
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
			if (_db.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number))
			{ 
				TempData["error"] = "This Villa Number Already exists.";
				obj.VillaList= _db.Villas.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                return View(obj);
            }

            if (ModelState.IsValid)
			{
				_db.VillaNumbers.Add(obj.VillaNumber);
				_db.SaveChanges();
				TempData["success"] = "Villa Number was created.";
				return RedirectToAction(nameof(Index));
			}
			else
			{
                obj.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
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
			if (!_db.VillaNumbers.Any(x => x.Villa_Number == villaNumberId))
			{
				TempData["error"] = "Could not find data for this villa room number";
				return RedirectToAction("Error", "Home");
			}
			VillaNumberVM villaNumberVM = new()
			{
				VillaList = _db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
				VillaNumber = _db.VillaNumbers.FirstOrDefault(x=>x.Villa_Number == villaNumberId)
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
                _db.VillaNumbers.Update(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number was Updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                obj.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
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
            if (!_db.VillaNumbers.Any(x => x.Villa_Number == villaNumberId))
            {
                TempData["error"] = "Could not find data for this villa room number";
                return RedirectToAction("Error", "Home");
            }
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
            };
            return View(villaNumberVM);
        }
        [HttpPost]
		public IActionResult Delete(VillaNumberVM villaNumberVM)
		{
			var AlteredRows = _db.VillaNumbers.Where(x => x.Villa_Number == villaNumberVM.VillaNumber.Villa_Number).ExecuteDelete();
			if (AlteredRows == 0)
			{
				TempData["error"] = "Villa was not deleted.";
				return RedirectToAction("Error", "Home");
			}
			TempData["success"] = "Villa was deleted.";
			return RedirectToAction(nameof(Index));
		}
	}
}
