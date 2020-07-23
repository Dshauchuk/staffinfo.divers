using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using System;
using System.Collections.Generic;
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

            ViewBag.diversCount = (await loadDivers).Select(t => t.DiversCount).ToList();
            ViewBag.topStationsName = (await loadDivers).Select(t => t.Name).ToList();
            ViewBag.stationsName = (await loadDivingTime).Select(t => t.Name).ToList();
            ViewBag.totalDivingTime = (await loadDivingTime).Select(t => t.TotalDivingTime).ToList();
            ViewBag.averageDivingTime = (await loadAverageDivingTime).Select(t => t.AverageDivingTime).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RedirectToDivers(int index)
        {
            List<MinStationModel> models = await _diverService.GetDiversPerStationAsync();

            return RedirectToAction("Index", "Divers", new { stationId = models[index].Id });
        }
    }
}