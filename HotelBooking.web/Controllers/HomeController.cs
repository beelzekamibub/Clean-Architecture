using HotelBooking.Application.SharedInterfaces;
using HotelBooking.web.Models;
using HotelBooking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelBooking.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryService _repo;
        public HomeController(IRepositoryService repo)
        {
            _repo = repo;            
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM() {
                Villas = await _repo.Villa.GetAllByFilter(includeJoinsOn: "Amenities"),
                Nights=1,
                CheckInDate=DateTime.Now,
            };
			return View(homeVM);
        }
        [HttpPost]
		public async Task<IActionResult> Index(HomeVM homeVM)
		{
            homeVM.Villas = await _repo.Villa.GetAllByFilter(includeJoinsOn: "Amenities");
			return View(homeVM);
		}
        [HttpPost]
        public async Task<IActionResult> GetVillasByDate(int nights, HomeVM model)
        {

            var villas = (await _repo.Villa.GetAllByFilter(includeJoinsOn: "Amenities")).ToList();
			foreach (var villa in villas)
			{
				if (villa.Id % 2 == 0)
					villa.IsAvailable = false;
			}
            HomeVM homeVM = new()
            {
                CheckInDate = model.CheckInDate,
                Nights= nights,
                Villas = villas
            };
            return View("Index",homeVM);
		}
		[HttpPost]
		public async Task<IActionResult> GetVillasByDateAjax(int nights, DateTime CheckInDate)
		{
			
			var villas = (await _repo.Villa.GetAllByFilter(includeJoinsOn: "Amenities")).ToList();
			foreach (var villa in villas)
			{
				if (villa.Id % 2 == 0)
					villa.IsAvailable = false;
			}
			HomeVM homeVM = new()
			{
				CheckInDate = CheckInDate,
				Nights = nights,
				Villas = villas
			};
			return View("Index", homeVM);
		}
		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}