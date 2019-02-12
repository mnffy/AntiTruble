using System.Collections.Generic;

namespace AntiTruble.ClassLibrary.Models
{
    public class EquipmentWithDefectsModel
    {
        public string EquipmentId { get; set; }
        public string RepairId { get; set; }
        public IEnumerable<EquipmentDefectsParam> Defects { get; set; }
    }
}
