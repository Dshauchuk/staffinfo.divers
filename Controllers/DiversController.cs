using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using System.Threading.Tasks;

namespace staffinfo.divers.Controllers
{
    [JwtAuthorize]
    public class DiversController : Controller
    {
        private readonly IDiverService _diverService;

        public DiversController(IDiverService diverService)
        {
            _diverService = diverService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int stationId)
        {
            //var diver = await _diverService.GetAsync(stationId);
            //var editModel = _mapper.Map<EditRescueStationModel>(station);

            ViewData["Title"] = "Изменить Информацию о Водолазе";
            ViewData["Action"] = "Update";

            return View("Edit", new EditDiverModel());
        }

        public async Task<IActionResult> Details(int diverId)
        {
            var diver = await _diverService.GetAsync(diverId);

            return View("Details", diver);
        }

        public IActionResult New()
        {
            ViewData["Title"] = "Новый Водолаз";
            ViewData["Action"] = "Add";

            return View("Edit", new EditDiverModel());
        }
           
        [HttpPost]
        public async Task<IActionResult> AddDivingTime(DivingTime time)
        {
            await _diverService.AddDivingTime(time);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDivingTime(int diverId, int year)
        {
            await _diverService.DeleteDivingTime(diverId, year);

            return NoContent();
        } 

        [HttpPost]
        public async Task<IActionResult> Add(EditDiverModel model)
        {
            _ = await _diverService.AddDiverAsync(model);

            return RedirectToAction("Index", "Divers");
        }

        [HttpGet]
        public async Task<JsonResult> GetListJson([FromQuery]FilterOptions filter = null)
        {
            var divers = await _diverService.GetAsync(filter);

            return Json(divers);
        }
    }
}