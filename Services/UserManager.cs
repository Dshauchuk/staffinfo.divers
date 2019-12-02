using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Staffinfo.Divers.Models;
using System;
using System.Diagnostics;

namespace Staffinfo.Divers.Services
{
    public class UserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string IdentityAlias = "Identity";

        public UserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserIdentity GetUserIdentity()
        {
            UserIdentity identity = null;

            if (_httpContextAccessor.HttpContext != null)
            {
                var json = _httpContextAccessor.HttpContext.Session.GetString(IdentityAlias);

                if (!string.IsNullOrEmpty(json))
                {
                    identity = JsonConvert.DeserializeObject<UserIdentity>(json);
                }
            }
            else
            {
                Debug.WriteLine("HttpContext is null");
            }

            return identity;
        }

        public DateTime? GetTokenExpary()
        {
            var identity = GetUserIdentity();

            return identity?.TokenExpire;
        }

        public int? GetUserId()
        {
            var identity = GetUserIdentity();

            return identity?.User?.UserId;
        }

        public string GetAccessToken()
        {
            var identity = GetUserIdentity();

            return identity?.AccessToken;
        }

        public void SetUserIdentity(UserIdentity identity)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString(IdentityAlias, identity.ToString());
            }
            else
            {
                Debug.WriteLine("HttpContext is null");
            }
        }

        public void ClearUserData()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
