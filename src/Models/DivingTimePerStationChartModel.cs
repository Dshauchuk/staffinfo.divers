namespace Staffinfo.Divers.Models
{
    public class DivingTimePerStationChartModel
    {
        public DivingTimePerStationChartModel()
        {
        }

        public DivingTimePerStationChartModel(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}
