namespace Staffinfo.Divers.Models
{
    public class DivingTimePerStationModel
    {
        public DivingTimePerStationModel()
        {
        }

        public DivingTimePerStationModel(int id, string name, int count)
        {
            Id = Id;
            Name = name;
            Count = count;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}
