namespace Staffinfo.Divers.Models
{
    public class AverageStationDivingTimeModel
    {
        public AverageStationDivingTimeModel()
        {
        }

        public AverageStationDivingTimeModel(int id, string name, double averageDivingTime, int diveNumber)
        {
            Id = id;
            Name = name;
            AverageDivingTime = averageDivingTime;
            DiveNumber = diveNumber;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Average diving time in minutes from the station
        /// </summary>
        public double AverageDivingTime { get; set; }

        /// <summary>
        /// Number of dives from the station
        /// </summary>
        public int DiveNumber { get; set; }
    }
}
