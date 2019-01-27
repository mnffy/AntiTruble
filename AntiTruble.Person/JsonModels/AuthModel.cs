using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Person.JsonModels
{
    public class AuthModel
    {
        [Required, JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Required, JsonProperty("Password")]
        public string Password { get; set; }
    }
}
