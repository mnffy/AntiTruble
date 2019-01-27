using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Equipment.JsonModels
{
    public class RemovingEquipmentModel
    {
        [Required, JsonProperty("EquipmentId")]
        public long EquipmentId { get; set; }
    }
}
