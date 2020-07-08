using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Models
{
    public class DiversOptions
    {
        public int RescueStationId { get; set; }

        public int MedicalExaminationStartDate { get; set; }

        public int MedicalExaminationEndDate { get; set; }

        public int MinQualification { get; set; }

        public int MaxQualification { get; set; }

        public int NameQuery { get; set; }

        public int MinHours { get; set; }

        public int MaxHours { get; set; }
    }
}
