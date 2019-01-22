
namespace AntiTruble.Repairs.Models
{
    public partial class Equipments
    {
        public long EquipmentId { get; set; }
        public string Name { get; set; }
        public byte? EquipmentType { get; set; }
        public long? DefectId { get; set; }
        public long? OwnerId { get; set; }
    }
}
