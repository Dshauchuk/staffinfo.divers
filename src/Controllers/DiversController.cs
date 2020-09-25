using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace staffinfo.divers.Controllers
{
    [JwtAuthorize]
    public class DiversController : Controller
    {
        private readonly IDiverService _diverService;
        private readonly IMapper _mapper;

        public DiversController(IDiverService diverService, IMapper mapper)
        {
            _diverService = diverService;
            _mapper = mapper;
        }

        public IActionResult Index(int? stationId = null)
        {
            return View(stationId);
        }

        public async Task<IActionResult> Edit(int diverId)
        {
            var diver = await _diverService.GetAsync(diverId);
            var editModel = _mapper.Map<EditDiverModel>(diver);
            editModel.RescueStationId = (int)diver.RescueStation.StationId;

            ViewData["Title"] = "Изменить Информацию о Водолазе";
            ViewData["Action"] = "Update";

            return View("Edit", editModel);
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
        public async Task<IActionResult> Update(int diverId, EditDiverModel model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var prevDiver = await _diverService.GetAsync(diverId);
            model.WorkingTime = prevDiver.WorkingTime;
            model.PhotoUrl = prevDiver.PhotoUrl;
            var diver = await _diverService.EditDiverAsync(diverId, model);
            
            return View("Details", diver);
        }

        [HttpPost]
        public async Task<IActionResult> AddDivingTime(DivingTime time)
        {
            await _diverService.AddDivingTimeAsync(time);

            return CreatedAtAction("Divers", time);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDivingTime(DivingTime time)
        {
            await _diverService.ChangeDivingTimeAsync(time);

            return CreatedAtAction("Divers", time);
        }

        [HttpPost]
        public async Task<ActionResult> UploadPhoto(string uploadedFileBase64, int diverId)
        {
            if (!string.IsNullOrEmpty(uploadedFileBase64))
            {
                await _diverService.AddPhotoAsync(uploadedFileBase64, diverId);
            }
            return View("Details", await _diverService.GetAsync(diverId));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDiverPhoto(int diverId)
        {
            await _diverService.DeletePhotoAsync(diverId);

            return View("Details", await _diverService.GetAsync(diverId));
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteDivingTime(int diverId, int year, int minutes)
        {
            await _diverService.DeleteDivingTimeAsync(diverId, year);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDiver(int diverId)
        {
            await _diverService.DeleteAsync(diverId);

            return RedirectToAction("Index", "Divers");
        }

        [HttpPost]
        public async Task<IActionResult> Add(EditDiverModel model)
        {
            var diver = await _diverService.AddDiverAsync(model);

            return View("Details", diver);
        }

        [HttpGet]
        public async Task<JsonResult> GetListWorkingTimeJson(int diverId)
        {
            var workingTime = (await _diverService.GetListWorkingTimeByDiverAsync(diverId));
            return Json(workingTime);
        }

        [HttpGet]
        public async Task<JsonResult> GetListJson([FromQuery]FilterOptions filter = null)
        {
            var divers = await _diverService.GetAsync(filter);

            return Json(divers);
        }
    }
}