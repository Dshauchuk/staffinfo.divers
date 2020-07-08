using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using System;
using System.Collections.Generic;
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

            ViewBag.diversPerStationChartSource = await loadDivers;
            ViewBag.divingTimePerStationChartSource = await loadDivingTime;
            ViewBag.averageDivingTimePerStationChartSource = await loadAverageDivingTime;

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