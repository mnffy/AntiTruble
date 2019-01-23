using System;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Repairs.Models
{
    public partial class Repairs
    {
        [Key]
        public long RepairId { get; set; }
        public byte? RepairType { get; set; }
        public byte? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? Client { get; set; }
        public long? Master { get; set; }
    }
}
