using AntiTruble.ClassLibrary.Enums;
using System.Collections.Generic;

namespace AntiTruble.ClassLibrary.Models
{
    public class EquipmentInfo
    {
        public long EquipmentId { get; set; }
        public string Name { get; set; }
        public EquipmentTypes EquipmentType { get; set; }
        public IEnumerable<EquipmentDefectsModel> Defects { get; set; }
        public long? Owner { get; set; }
    }

    public class EquipmentDefectsModel
    {
        public long DefectId { get; set; }
        public string DefectName { get; set; }
        public decimal? Price { get; set; }
        public long? EquipmentId { get; set; }
    }

    public class EquipmentInfoParam
    {
        public string EquipmentId { get; set; }
        public string Name { get; set; }
        public string EquipmentType { get; set; }
        public string Owner { get; set; }
        public IEnumerable<EquipmentDefectsParam> Defects { get; set; }
    }

    public class EquipmentDefectsParam
    {
        public string DefectName { get; set; }
        public string Price { get; set; }
    }
}
