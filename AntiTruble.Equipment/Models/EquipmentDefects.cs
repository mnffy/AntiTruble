
namespace AntiTruble.Repairs.Models
{
    public partial class EquipmentDefects
    {
        public long DefectId { get; set; }
        public string DefectName { get; set; }
        public decimal? Price { get; set; }
        public byte? Status { get; set; }
        public long? EquipmentId { get; set; }
    }
}
