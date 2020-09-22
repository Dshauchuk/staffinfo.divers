namespace Staffinfo.Divers.Models
{
    public class MinStationModel
    {
        public MinStationModel()
        {
        }

        public MinStationModel(int id, string name, int diversCount)
        {
            StationId = id;
            StationName = name;
            DiversCount = diversCount;
        }

        public int StationId { get; set; }

        public string StationName { get; set; }

        public int DiversCount { get; set; }
    }
}
