using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Models;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Modifies user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        [HttpPut("{userId:int}")]
        [ProducesResponseType(statusCode: 200, type: typeof(User))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> ModifyUser([FromRoute]int userId, [FromBody]EditUserModel model)
        {
            var user = await _userService.ModifyUserAsync(userId, model);

            return Ok(user);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("{userId:int}")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserIdentity))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteAsync([FromRoute]int userId)
        {
            await _userService.DeleteUserAsync(userId);

            return NoContent();
        }
    }
}