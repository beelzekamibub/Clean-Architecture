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
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM() {
                Villas = _repo.Villa.GetAllByFilter(includeJoinsOn: "Amenities"),
                Nights=1,
                CheckInDate=DateTime.Now,
            };
            return View(homeVM);
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