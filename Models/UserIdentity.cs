using Newtonsoft.Json;
using System;

namespace Staffinfo.Divers.Models
{
    public class UserIdentity
    {
        public UserIdentity(User user, string accessToken, string refreshToken, DateTime tokenExpire)
        {
            User = user;
            AccessToken = accessToken;
            TokenExpire = tokenExpire;
            RefreshToken = refreshToken;
        }

        public UserIdentity()
        {

        }

        public User User { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime TokenExpire { get; set; }

        public override string? ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
