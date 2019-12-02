using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Staffinfo.Divers.Models
{
    [DataContract]
    public class EditRescueStationModel
    {
        [DataMember(Name = "stationName")]
        [Required(ErrorMessage = "Нужно указать название спасательной станции")]
        public string StationName { get; set; }

        public int StationId { get; set; }
    }
}
