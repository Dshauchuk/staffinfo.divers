using Microsoft.AspNetCore.Http;
using Staffinfo.Divers.Services;
using Staffinfo.Divers.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Infrastructure.Middleware
{
    internal class JwtManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAccountService _accountService;
        private readonly UserManager _userManager;

        public JwtManagerMiddleware(RequestDelegate next,
            IAccountService accountService,
            UserManager userManager)
        {
            this._next = next;

            _accountService = accountService;
            _userManager = userManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var identity = _userManager.GetUserIdentity();

            if (identity?.TokenExpire != null && (DateTime.UtcNow > identity?.TokenExpire))
            {
                await _accountService.RefreshAsync(identity.User.UserId, identity.RefreshToken);
            }

            var jwt = _userManager.GetAccessToken();
            if (!string.IsNullOrEmpty(jwt))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + jwt);
            }
            await _next.Invoke(context);
        }
    }
}
