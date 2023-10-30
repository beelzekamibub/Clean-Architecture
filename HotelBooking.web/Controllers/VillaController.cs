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
        public async Task<IActionResult> Index()
        {
            var villa = await _repo.Villa.GetAllByFilter();
            return View(villa);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Villa obj)
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

				await _repo.Villa.Add(obj);
                await _repo.Villa.Save();
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
        public async Task<IActionResult> Update(int villaId)
        {
            Villa? villa =await _repo.Villa.GetByFilter(x => x.Id == villaId);
            //same as
            villa = await _repo.Villa.GetByFilter(x => x.Id == villaId);
            if (villa == null)
            {
                TempData["error"] = "Villa was not found.";
                return RedirectToAction("Error","Home");
            }
            return View(villa);
        }

		[HttpPost]
		public async Task<IActionResult> Update(Villa obj)
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
                await _repo.Villa.Save();
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
        public async Task<IActionResult> Delete(int villaId)
        {
            Villa? villa=await _repo.Villa.GetByFilter(x=>x.Id== villaId);
            if(villa!=null)
                return View(villa);
            TempData["error"] = "Villa was not found.";
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Villa villa)
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
            await _repo.Villa.Save();
            TempData["success"] = "Villa was deleted.";
            return RedirectToAction(nameof(Index));
        }
	}
}
