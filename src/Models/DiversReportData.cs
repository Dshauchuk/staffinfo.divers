using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Models
{
    public class DiversReportData
    {
        public string Name { get; set; }

        public string StationName { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime? MedicalExaminationDate { get; set; }

        public int? TotalHours { get; set; }

        public int? HoursThisYear { get; set; }


    }
}
