using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Staffinfo.Divers.Models
{
    public class EditDiverModel
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        public int? RescueStationId { get; set; }

        public DateTime? MedicalExaminationDate { get; set; }

        public string Address { get; set; }

        public int Qualification { get; set; }

        [Required]
        public string PersonalBookNumber { get; set; }

        [Required]
        public DateTime? PersonalBookIssueDate { get; set; }

        [Required]
        public string PersonalBookProtocolNumber { get; set; }

        public List<DivingTime> WorkingTime { get; set; }
    }
}
