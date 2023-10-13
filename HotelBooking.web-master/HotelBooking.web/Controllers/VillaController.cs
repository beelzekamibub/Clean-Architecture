using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

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
            _db.Villas.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
