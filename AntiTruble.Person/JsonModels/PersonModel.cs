using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Person.JsonModels
{
    public class PersonModel
    {
        [Required, JsonProperty("Fio")]
        public string Fio { get; set; }
        [Required, JsonProperty("Password")]
        public string Password { get; set; }
        [Required, JsonProperty("Role", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Role { get; set; }
        [Required, JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Required, JsonProperty("Address")]
        public string Address { get; set; }
        [Required, JsonProperty("Balance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Balance { get; set; }
    }
}
