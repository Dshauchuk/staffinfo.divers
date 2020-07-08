namespace Staffinfo.Divers.Models
{
    public class StationDivingTimeModel
    {
        public StationDivingTimeModel()
        {
        }

        public StationDivingTimeModel(int id, string name, int totalDivingTime)
        {
            Id = id;
            Name = name;
            TotalDivingTime = totalDivingTime;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Total diving time in minutes from the station
        /// </summary>
        public int TotalDivingTime { get; set; }
    }
}
