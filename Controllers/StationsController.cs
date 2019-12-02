using Microsoft.AspNetCore.Mvc;

namespace staffinfo.divers.Controllers
{
    public class StationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}