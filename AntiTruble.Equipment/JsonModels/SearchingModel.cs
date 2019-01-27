using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Equipment.JsonModels
{
    public class SearchingModel
    {
        [Required, JsonProperty("PersonId")]
        public long PersonId{ get; set; }
    }
}
