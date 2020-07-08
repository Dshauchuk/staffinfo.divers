namespace Staffinfo.Divers.Models
{
    public class MinStationModel
    {
        public MinStationModel()
        {
        }

        public MinStationModel(int id, string name, int diversCount)
        {
            Id = id;
            Name = name;
            DiversCount = diversCount;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int DiversCount { get; set; }
    }
}
