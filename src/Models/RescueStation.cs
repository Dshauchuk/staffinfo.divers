using System;

namespace Staffinfo.Divers.Models
{
    public class RescueStation
    {
        public int StationId { get; set; }

        public string StationName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}