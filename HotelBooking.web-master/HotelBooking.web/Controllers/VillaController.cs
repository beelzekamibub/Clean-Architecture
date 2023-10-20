using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers
{
    
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            var villa = _db.Villas.ToList();
            return View(villa);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if(obj.Name==obj.Description)
            {

                ModelState.AddModelError("Description","Description and name can not be same.");
            }
            if(ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa was created.";
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction("Error","Home");
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
                return RedirectToAction(nameof(Index));
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
            Villa? villa=_db.Villas.FirstOrDefault(x=>x.Id== villaId);
            if(villa!=null)
                return View(villa);
            TempData["error"] = "Villa was not found.";
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            var AlteredRows=_db.Villas.Where(x=>x.Id== villa.Id).ExecuteDelete();
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
