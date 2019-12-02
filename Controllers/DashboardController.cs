using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;

namespace staffinfo.divers.Controllers
{
    [JwtAuthorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}