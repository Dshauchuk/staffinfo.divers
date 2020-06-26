using Staffinfo.Divers.Models.Abstract;
using System;

namespace Staffinfo.Divers.Models
{
    /// <inheritdoc/>
    public class FilterOptions : IFilterOptions
    {
        /// <inheritdoc/>
        public int? RescueStationId { get; set; }

        /// <inheritdoc/>
        public string? MedicalExaminationStartDate { get; set; }

        /// <inheritdoc/>
        public string? MedicalExaminationEndDate { get; set; }

        /// <inheritdoc/>
        public int? MinQualification { get; set; }

        /// <inheritdoc/>
        public int? MaxQualification { get; set; }

        /// <inheritdoc/>
        public string NameQuery { get; set; }

        /// <inheritdoc/>
        public int MinHours { get; set; }

        /// <inheritdoc/>
        public int MaxHours { get; set; }
    }
}
