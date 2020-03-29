namespace Staffinfo.Divers.Data.Poco
{
    public class DivingTimePoco
    {
        public int DivingTimeId { get; set; }

        public int DiverId { get; set; }

        public DiverPoco Diver { get; set; }

        public int Year { get; set; }

        public int WorkingMinutes { get; set; }
    }
}
