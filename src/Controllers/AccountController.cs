using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Services.Contracts;

namespace Staffinfo.Divers.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        ///// <summary>
        ///// Register a new user
        ///// </summary>
        ///// <remarks>Create a new user</remarks>
        ///// <param name="model"></param>
        ///// <response code="201">Successful.</response>
        ///// <response code="400">Invalid parameters.</response>
        ///// <response code="401">Not authorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="500">An unexpeced error occurred.</response>
        //[HttpPost]
        ////[Authorize(Roles = "admin")]
        //[AllowAnonymous]
        //[ProducesResponseType(statusCode: 201, type: typeof(UserIdentity))]
        //[ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        //public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        //{
        //    var identity = await _accountService.RegisterAsync(model);

        //    return Created("api/accounts", identity);
        //}

        ///// <summary>
        ///// Login
        ///// </summary>
        ///// <param name="model"></param>
        ///// <response code="200">Successful.</response>
        ///// <response code="400">Invalid parameters.</response>
        ///// <response code="401">Not authorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="404">Not found</response>
        ///// <response code="500">An unexpeced error occurred.</response>
        //[HttpPost("login")]
        //[AllowAnonymous]
        //[ProducesResponseType(statusCode: 200, type: typeof(UserIdentity))]
        //[ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        //public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        //{
        //    var identity = await _accountService.LoginAsync(model);

        //    return Ok(identity);
        //}

        ///// <summary>
        ///// Refresh token
        ///// </summary>
        ///// <param name="model"></param>
        ///// <response code="200">Successful.</response>
        ///// <response code="400">Invalid parameters.</response>
        ///// <response code="401">Not authorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="404">Not found</response>
        ///// <response code="500">An unexpeced error occurred.</response>
        //[HttpPut("token/refresh")]
        //[AllowAnonymous]
        //[ProducesResponseType(statusCode: 200, type: typeof(UserIdentity))]
        //[ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        //[ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        //public async Task<IActionResult> RefreshAsync([FromBody, Required] RefreshTokenModel model)
        //{
        //    var identity = await _accountService.RefreshAsync(model.UserId, model.RefreshToken);

        //    return Ok(identity);
        //}
    }
}