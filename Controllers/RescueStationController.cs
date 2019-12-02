using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Infrastructure.Attributes;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Controllers
{
    [ApiController]
    [Route("api/stations")]
    public class RescueStationController: ControllerBase
    {
        private readonly IRescueStationService _rescueStationService;

        public RescueStationController(IRescueStationService rescueStationService)
        {
            _rescueStationService = rescueStationService;
        }

        /// <summary>
        /// Create a new rescue station
        /// </summary>
        /// <param name="model"></param>
        /// <response code="201">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpPost]
        [ProducesResponseType(statusCode: 201, type: typeof(RescueStation))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAsync([FromBody]EditRescueStationModel model)
        {
            var added = await _rescueStationService.AddStationAsync(model);

            return Created("api/stations", added);
        }

        /// <summary>
        /// Modify rescue station
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not found</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpPut("{stationId:int}")]
        [ProducesResponseType(statusCode: 200, type: typeof(RescueStation))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateAsync([FromRoute]int stationId, [FromBody, Required]EditRescueStationModel model)
        {
            var updated = await _rescueStationService.EditStationAsync(stationId, model);

            return Ok(updated);
        }

        /// <summary>
        /// Get a rescue station by id
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not found</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpGet("{stationId:int}")]
        [ProducesResponseType(statusCode: 200, type: typeof(RescueStation))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> GetAsync([FromRoute]int stationId)
        {
            var station = await _rescueStationService.GetAsync(stationId);

            return Ok(station);
        }

        /// <summary>
        /// Get a list of rescue stations
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [JwtAuthorize]
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(IEnumerable<RescueStation>))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> GetListAsync()
        {
            var stations = await _rescueStationService.GetAsync();

            return Ok(stations);
        }

        /// <summary>
        /// Delete a rescue station by id
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not found</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpDelete("{stationId:int}")]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteAsync([FromRoute]int stationId)
        {
            await _rescueStationService.DeleteAsync(stationId);

            return NoContent();
        }
    }
}