namespace Staffinfo.Divers.Models
{
    public class DiversPerStationChartModel
    {
        public DiversPerStationChartModel()
        {
        }

        public DiversPerStationChartModel(int id, string name, int count)
        {
            Id = id;
            Name = name;
            Count = count;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}
