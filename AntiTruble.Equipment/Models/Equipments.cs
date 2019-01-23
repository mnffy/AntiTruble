
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Equipment.Models
{
    public partial class Equipments
    {
        [Key]
        public long EquipmentId { get; set; }
        public string Name { get; set; }
        public byte? EquipmentType { get; set; }
        public long? OwnerId { get; set; }
    }
}
