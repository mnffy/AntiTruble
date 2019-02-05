using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Equipment.JsonModels
{
    public class EquipmentParamModel
    {
        [Required, JsonProperty("Name")]
        public string Name { get; set; }
        [Required, JsonProperty("EquipmentType")]
        public byte EquipmentType { get; set; }
        [Required, JsonProperty("RepairId")]
        public long RepairId { get; set; }
        [Required, JsonProperty("Defects")]
        public IEnumerable<EquipmentInfoParamModel> Defects { get; set; }
    }
}
