﻿namespace Staffinfo.Divers.Models
{
    public class EditUserModel
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public bool NeedToChangePwd { get; set; }

        public string Role { get; set; }
    }
}