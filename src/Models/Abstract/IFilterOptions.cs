﻿using System;

namespace Staffinfo.Divers.Models.Abstract
{
    /// <summary>
    /// Options to select divers
    /// </summary>
    public interface IFilterOptions
    {
        /// <summary>
        /// Id of requested station id to select its own divers
        /// </summary>
        int? RescueStationId { get; set; }

        /// <summary>
        /// Minimum medical examination date
        /// </summary>
        string? MedicalExaminationStartDate { get; set; }

        /// <summary>
        /// Maximum medical examination date
        /// </summary>
        string? MedicalExaminationEndDate { get; set; }

        /// <summary>
        /// Min qualification
        /// </summary>
        int? MinQualification { get; set; }

        /// <summary>
        /// Max qualification
        /// </summary>
        int? MaxQualification { get; set; }

        /// <summary>
        /// String that is contained in the diver's name
        /// </summary>
        string NameQuery { get; set; }

        /// <summary>
        /// Minimum number of diving hours
        /// </summary>
        int MinHours { get; set; }

        /// <summary>
        /// Maximum number of diving hours
        /// </summary>
        int MaxHours { get; set; }
    }
}