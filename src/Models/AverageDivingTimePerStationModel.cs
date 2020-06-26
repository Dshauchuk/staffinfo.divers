namespace Staffinfo.Divers.Models
{
    public class AverageDivingTimePerStationModel
    {
        public AverageDivingTimePerStationModel()
        {
        }

        public AverageDivingTimePerStationModel(int id, string name, double average, int count)
        {
            Id = id;
            Name = name;
            Average = average;
            Count = count;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public double Average { get; set; }

        public int Count { get; set; }
    }
}
