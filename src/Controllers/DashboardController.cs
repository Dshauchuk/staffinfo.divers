using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace staffinfo.divers.Controllers
{
    [JwtAuthorize]
    public class DashboardController : Controller
    {
        private readonly IDiverService _diverService;
        private readonly IRescueStationService _rescueStationService;

        public DashboardController(IDiverService diverService, IRescueStationService rescueStationService)
        {
            _diverService = diverService;
            _rescueStationService = rescueStationService;
        }

        public async Task<IActionResult> Index()
        {
            var loadDivers = _diverService.GetDiversPerStationAsync();
            var loadDivingTime = _diverService.GetDivingTimePerStationAsync();
            var loadAverageDivingTime = _diverService.GetAverageDivingTimePerStationAsync();

            ViewBag.diversCount = (await loadDivers).OrderByDescending(c => c.DiversCount).Take(10).Select(t => t.DiversCount);
            ViewBag.topStationsName = (await loadDivers).OrderByDescending(c => c.DiversCount).Take(10).Select(t => t.StationName);
            ViewBag.stationsName = (await loadDivingTime).Select(t => t.Name);
            ViewBag.totalDivingTime = (await loadDivingTime).Select(t => t.TotalDivingTime);
            ViewBag.averageDivingTime = (await loadAverageDivingTime).Select(t => t.AverageDivingTime);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RedirectToDivers(int index)
        {
            var models = (await _diverService.GetDiversPerStationAsync()).OrderByDescending(c => c.DiversCount).Take(10).ToArray();

            return RedirectToAction("Index", "Divers", new { stationId = models[index].StationId });
        }
    }
}