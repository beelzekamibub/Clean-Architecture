using HotelBooking.Application.SharedInterfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HotelBooking.Web.Controllers
{
    
    public class VillaController : Controller
    {
        private readonly IRepositoryService _repo;
        private readonly IWebHostEnvironment _environment;
		public VillaController(IRepositoryService repo, IWebHostEnvironment environment)
        {
            _repo = repo;
            _environment = environment;
        }
        public IActionResult Index()
        {
            var villa = _repo.Villa.GetAllByFilter();
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
                if (obj.Image != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string VillasPath=Path.Combine(_environment.WebRootPath, @"Images\Villas");
                    string imagePath = Path.Combine(VillasPath,filename);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\Images\Villas\" + filename;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }

				_repo.Villa.Add(obj);
                _repo.Villa.Save();
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
            Villa? villa = _repo.Villa.GetByFilter(x => x.Id == villaId);
            //same as
            villa = _repo.Villa.GetByFilter(x=>x.Id==villaId);
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
                if (obj.Image != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string VillasPath = Path.Combine(_environment.WebRootPath, @"Images\Villas");
                    string imagePath = Path.Combine(VillasPath, filename);
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_environment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\Images\Villas\" + filename;
                }
                
                _repo.Villa.Update(obj);
                _repo.Villa.Save();
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
            Villa? villa=_repo.Villa.GetByFilter(x=>x.Id== villaId);
            if(villa!=null)
                return View(villa);
            TempData["error"] = "Villa was not found.";
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
			if (!string.IsNullOrEmpty(villa.ImageUrl))
			{
				var oldImagePath = Path.Combine(_environment.WebRootPath, villa.ImageUrl.TrimStart('\\'));
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}
			}
			_repo.Villa.Remove(villa);
            _repo.Villa.Save();
            TempData["success"] = "Villa was deleted.";
            return RedirectToAction(nameof(Index));
        }
	}
}
