using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Data.Services.Contracts;
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
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDiverService _diverService;
        private readonly IRescueStationService _rescueStationService;
        private readonly IDivingTimeRepository _divingTimeRepository;
        private readonly IReportService _reportService;
        private readonly IReportDataService _reportDataService;
        private readonly IMapper _mapper;

        public DiversController(IDivingTimeRepository divingTimeRepository, IRescueStationService rescueStationService, 
                                IDiverService diverService, IReportService reportService, IReportDataService reportDataService, 
                                IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _divingTimeRepository = divingTimeRepository;
            _rescueStationService = rescueStationService;
            _diverService = diverService;
            _reportService = reportService;
            _reportDataService = reportDataService;
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

        public async Task<IActionResult> ExportExcel()
        {
            return File(await _reportService.GenerateExcelReport(), System.Net.Mime.MediaTypeNames.Application.Octet, "Report.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int diverId, EditDiverModel model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var divingTimes = await _divingTimeRepository.GetListAsync(diverId);
            model.WorkingTime = _mapper.Map<List<DivingTime>>(divingTimes);
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
        public async Task<ActionResult> UploadPhoto(IFormFile uploadedFile, int diverId)
        {
            if (uploadedFile != null)
            {
                using var fileStream = uploadedFile.OpenReadStream();
                byte[] bytes = new byte[uploadedFile.Length];
                fileStream.Read(bytes, 0, (int)uploadedFile.Length);
                string base64ImageRepresentation = "data:" + uploadedFile.ContentType + ";base64," + Convert.ToBase64String(bytes);

                await _diverService.AddPhotoAsync(base64ImageRepresentation, diverId);
            }
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
        public async Task<JsonResult> GetListJson([FromQuery]FilterOptions filter = null)
        {
            var divers = await _diverService.GetAsync(filter);

            return Json(divers);
        }
    }
}