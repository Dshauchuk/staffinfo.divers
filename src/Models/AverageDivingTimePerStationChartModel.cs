namespace Staffinfo.Divers.Models
{
    public class AverageDivingTimePerStationChartModel
    {
        public AverageDivingTimePerStationChartModel()
        {
        }

        public AverageDivingTimePerStationChartModel(string name, double average, int count)
        {
            Name = name;
            Average = average;
            Count = count;
        }

        public string Name { get; set; }

        public double Average { get; set; }

        public int Count { get; set; }
    }
}
