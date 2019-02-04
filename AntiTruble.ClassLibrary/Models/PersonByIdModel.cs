using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.ClassLibrary.Models
{
    public class PersonByIdModel
    {
        [Required, JsonProperty("Id")]
        public long Id { get; set; }
    }
}
