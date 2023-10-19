using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
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
			var villaNumbers = _db.VillaNumbers.ToList();
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
			return View();
		}

		[HttpPost]
		public IActionResult Create(VillaNumber obj)
		{
			ModelState.Remove("Villa");
			if (_db.VillaNumbers.Any(x => x.Villa_Number == obj.Villa_Number))
			{ 
				TempData["error"] = "This Villa Number Already exists.";
				return View(obj);
            }

            if (ModelState.IsValid)
			{
				_db.VillaNumbers.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Villa Number was created.";
				return RedirectToAction("Index", "VillaNumber");
			}
			else
			{
				TempData["error"] = "Villa could not be added.";
				return View(obj);
			}
		}
		[HttpGet]
		public IActionResult Update(int villaId)
		{
			Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == villaId);
			//same as
			villa = _db.Villas.Find(villaId);
			if (villa == null)
			{
				TempData["error"] = "Villa was not found.";
				return RedirectToAction("Error", "Home");
			}
			return View(villa);
		}

		[HttpPost]
		public IActionResult Update(Villa obj)
		{
			if (ModelState.IsValid)
			{
				_db.Villas.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Villa was updated.";
				return RedirectToAction("Index", "Villa");
			}
			else
			{
				TempData["error"] = "Villa was not found.";
				return View(obj);
			}
		}
		[HttpGet]
		public IActionResult Delete(int villaId)
		{
			Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == villaId);
			if (villa != null)
				return View(villa);
			TempData["error"] = "Villa was not found.";
			return RedirectToAction("Error", "Home");
		}
		[HttpPost]
		public IActionResult Delete(Villa villa)
		{
			var AlteredRows = _db.Villas.Where(x => x.Id == villa.Id).ExecuteDelete();
			if (AlteredRows == 0)
			{
				TempData["error"] = "Villa was not deleted.";
				return RedirectToAction("Error", "Home");
			}
			TempData["success"] = "Villa was deleted.";
			return RedirectToAction("Index");
		}
	}
}
