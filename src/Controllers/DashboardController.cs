using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Models.Abstract;
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
            ViewBag.dataSource = await GetDiversPerStation();

            ViewBag.dataSource1 = await GetDivingTimePerStation();

            ViewBag.dataSource2 = await GetAverageDivingTimePerStation();

            return View(ViewBag.dataSource);
        }

        [HttpPost]
        public async Task<IActionResult> RedirectToDivers(int index)
        {
            List<DiversPerStationChartModel> models = await GetDiversPerStation();

            RescueStation station = await _rescueStationService.GetAsync(models[index].Id);

            return RedirectToAction("Index", "Divers", station);
        }

        private async Task<List<DiversPerStationChartModel>> GetDiversPerStation()
        {
            var stations = (await _rescueStationService.GetAsync()).ToList();
            var divers = (await _diverService.GetAsync(null as IFilterOptions)).ToList();

            List<DiversPerStationChartModel> chartModels = new List<DiversPerStationChartModel>();

            foreach (RescueStation rescueStation in stations)
            {
                chartModels.Add(new DiversPerStationChartModel()
                {
                    Id = rescueStation.StationId,
                    Name = rescueStation.StationName,
                    Count = divers.Where(c => c.RescueStation.StationId == rescueStation.StationId).ToList().Count
                });
            }

            chartModels = chartModels.OrderByDescending(c => c.Count).Take(10).ToList();

            return chartModels;
        }

        private async Task<List<DivingTimePerStationChartModel>> GetDivingTimePerStation()
        {
            var stations = (await _rescueStationService.GetAsync()).ToList();
            var divers = (await _diverService.GetAsync(null as IFilterOptions)).ToList();

            List<DivingTimePerStationChartModel> chartModels = new List<DivingTimePerStationChartModel>();

            foreach (RescueStation rescueStation in stations)
            {
                chartModels.Add(new DivingTimePerStationChartModel()
                {
                    Name = rescueStation.StationName,
                    Count = 0
                });
            }

            foreach (Diver diver in divers)
            {
                chartModels.First(c => string.Equals(c.Name, diver.RescueStation.StationName)).Count += diver.WorkingTime.Sum(c => c.WorkingMinutes);
            }

            return chartModels;
        }

        private async Task<List<AverageDivingTimePerStationChartModel>> GetAverageDivingTimePerStation()
        {
            var stations = (await _rescueStationService.GetAsync()).ToList();
            var divers = (await _diverService.GetAsync(null as IFilterOptions)).ToList();

            List<AverageDivingTimePerStationChartModel> chartModels = new List<AverageDivingTimePerStationChartModel>();

            foreach (RescueStation rescueStation in stations)
            {
                chartModels.Add(new AverageDivingTimePerStationChartModel()
                {
                    Name = rescueStation.StationName,
                    Average = 0,
                    Count = 0
                });
            }

            foreach (Diver diver in divers)
            {
                var chartModel = chartModels.First(c => string.Equals(c.Name, diver.RescueStation.StationName));
                chartModel.Average += diver.WorkingTime.Sum(c => c.WorkingMinutes);
                chartModel.Count += diver.WorkingTime.Count;
            }

            foreach(AverageDivingTimePerStationChartModel model in chartModels)
            {
                model.Average = Math.Round(model.Average / model.Count, 1);
            }

            return chartModels;
        }
    }
}