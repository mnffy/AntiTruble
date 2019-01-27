
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Equipment.JsonModels
{
    public class EquipmentInfoParamModel
    {
        [Required, JsonProperty("DefectName")]
        public string DefectName { get; set; }
        [Required, JsonProperty("Price")]
        public decimal Price { get; set; }
    }
}
