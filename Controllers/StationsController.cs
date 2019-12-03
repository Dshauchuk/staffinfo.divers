using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using System.Threading.Tasks;

namespace staffinfo.divers.Controllers
{
    [JwtAuthorize]
    public class StationsController : Controller
    {
        private readonly IRescueStationService _rescueStationService;
        private readonly IMapper _mapper;

        public StationsController(IRescueStationService rescueStationService, IMapper mapper)
        {
            _rescueStationService = rescueStationService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            ViewData["Title"] = "Новая Спасательная Станция";
            ViewData["Action"] = "Add";

            return View("Edit", new EditRescueStationModel());
        }
        
        public async Task<IActionResult> Edit(int stationId)
        {
            var station = await _rescueStationService.GetAsync(stationId);
            var editModel = _mapper.Map<EditRescueStationModel>(station);

            ViewData["Title"] = "Изменить Спасательную Станцию";
            ViewData["Action"] = "Update";

            return View("Edit", editModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EditRescueStationModel model)
        {
            _ = await _rescueStationService.AddStationAsync(model);

            return RedirectToAction("Index", "Stations");
        }


        [HttpPost]
        public async Task<IActionResult> Update(EditRescueStationModel model)
        {
            _ = await _rescueStationService.EditStationAsync(model);

            return View("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int stationId)
        {
            await _rescueStationService.DeleteAsync(stationId);

            return NoContent();
        }

        [HttpGet]
        public async Task<JsonResult> GetListJson()
        {
            var stations = await _rescueStationService.GetAsync();

            return Json(stations);
        }
    }
}