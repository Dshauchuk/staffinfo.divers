using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Staffinfo.Divers.Models
{
    /// <summary>
    /// Error response object to give more info with HTTP status code
    /// </summary>
    [DataContract]
    public partial class ErrorResponse
    {
        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse()
        {

        }

        [Required]
        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
