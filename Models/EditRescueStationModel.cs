using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Staffinfo.Divers.Models
{
    [DataContract]
    public class EditRescueStationModel
    {
        [DataMember(Name = "stationName")]
        [Required(ErrorMessage = "Нужно указать название спасательной станции")]
        [StringLength(100, ErrorMessage = "Длина названия должна быть от 5 до 100 символов", MinimumLength = 5)]
        public string StationName { get; set; }

        public int StationId { get; set; }
    }
}
