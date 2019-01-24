using System.Collections.Generic;

namespace AntiTruble.Equipment.JsonModels
{
    public class EquipmentParamModel
    {
        public string Name { get; set; }
        public byte EquipmentType { get; set; }
        public string Fio { get; set; }
        public IEnumerable<EquipmentInfoParamModel> Defects { get; set; }
    }
}
