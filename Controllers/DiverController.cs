using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Controllers
{
    [ApiController]
    [Route("api/divers")]
    public class DiverController : ControllerBase
    {
        private readonly IDiverService _diverService;

        public DiverController(IDiverService diverService)
        {
            _diverService = diverService;
        }
        
        /// <summary>
        /// Create a new diver
        /// </summary>
        /// <param name="model"></param>
        /// <response code="201">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpPost]
        [ProducesResponseType(statusCode: 201, type: typeof(Diver))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAsync([FromBody, Required]EditDiverModel model)
        {
            var added = await _diverService.AddDiverAsync(model);

            return Created("api/divers", added);
        }

        /// <summary>
        /// Modify diver
        /// </summary>
        /// <param name="diverId"></param>
        /// <param name="model"></param>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not found</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpPut("{diverId:int}")]
        [ProducesResponseType(statusCode: 200, type: typeof(Diver))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateAsync([FromRoute]int diverId, [FromBody, Required]EditDiverModel model)
        {
            var updated = await _diverService.EditDiverAsync(diverId, model);

            return Ok(updated);
        }

        /// <summary>
        /// Get a diver by id
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not found</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpGet("{diverId:int}")]
        [ProducesResponseType(statusCode: 200, type: typeof(Diver))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> GetAsync([FromRoute]int diverId)
        {
            var diver = await _diverService.GetAsync(diverId);

            return Ok(diver);
        }

        /// <summary>
        /// Get a list of divers
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(IEnumerable<Diver>))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> GetListAsync([FromQuery]FilterOptions filter = null)
        {
            var divers = await _diverService.GetAsync(filter);

            return Ok(divers);
        }

        /// <summary>
        /// Delete a diver by id
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not found</response>
        /// <response code="500">An unexpeced error occurred.</response>
        [HttpDelete("{diverId:int}")]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteAsync([FromRoute]int diverId)
        {
            await _diverService.DeleteAsync(diverId);

            return NoContent();
        }
    }
}