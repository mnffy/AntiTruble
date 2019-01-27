using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Person.JsonModels
{
    public class PersonByIdModel
    {
        [Required, JsonProperty("Id")]
        public long Id { get; set; }
    }
}
