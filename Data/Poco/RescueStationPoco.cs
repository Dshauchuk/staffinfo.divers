using System;

namespace Staffinfo.Divers.Data.Poco
{
    public class RescueStationPoco
    {
        public int StationId { get; set; }

        public string StationName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
