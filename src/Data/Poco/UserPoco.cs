using System;

namespace Staffinfo.Divers.Data.Poco
{
    public class UserPoco
    {
        public int UserId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Login { get; set; }

        public bool NeedToChangePwd { get; set; }

        public string PwdHash { get; set; }

        public string RefreshToken { get; set; }

        public DateTimeOffset? TokenRefreshTimestamp { get; set; }

        public string Role { get; set; }

        public DateTimeOffset RegistrationTimestamp { get; set; }
    }
}