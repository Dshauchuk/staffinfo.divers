using System;
using System.Collections.Generic;

namespace Staffinfo.Divers.Data.Poco
{
    public class DiverPoco
    {
        public int DiverId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? RescueStationId { get; set; }

        public RescueStationPoco RescueStation { get; set; }

        public DateTime? MedicalExaminationDate { get; set; }

        public string Address { get; set; }

        public int Qualification { get; set; }

        public string PersonalBookNumber { get; set; }

        public DateTime? PersonalBookIssueDate { get; set; }

        public string PersonalBookProtocolNumber { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public List<DivingTimePoco> WorkingTime { get; set; }
    }
}
