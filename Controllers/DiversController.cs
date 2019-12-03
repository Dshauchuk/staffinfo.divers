using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace staffinfo.divers.Controllers
{
    public class DiversController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}