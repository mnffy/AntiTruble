using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.ClassLibrary.Models
{
    public class PersonIdByFioModel
    {
        [Required, JsonProperty("fio")]
        public string Fio { get; set; }
    }
}
