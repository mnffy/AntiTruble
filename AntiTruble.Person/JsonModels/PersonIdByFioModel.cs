using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Person.JsonModels
{
    public class PersonIdByFioModel
    {
        [Required, JsonProperty("fio")]
        public string Fio { get; set; }
    }
}
