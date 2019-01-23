using AntiTruble.Equipment.Enums;
using AntiTruble.Equipment.Models;
using AntiTruble.Person.Models;
using System.Collections.Generic;

namespace AntiTruble.Equipment.DataModels
{
    public class EquipmentInfo
    {
        public long EquipmentId { get; set; }
        public string Name { get; set; }
        public EquipmentTypes EquipmentType { get; set; }
        public IEnumerable<EquipmentDefects> Defects { get; set; }
    }
}
