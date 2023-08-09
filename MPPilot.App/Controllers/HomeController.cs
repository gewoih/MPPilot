using Microsoft.AspNetCore.Mvc;

namespace MPPilot.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}