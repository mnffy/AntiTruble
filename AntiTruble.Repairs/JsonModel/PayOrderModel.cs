using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Repairs.JsonModel
{
    public class PayOrderModel
    {
        [Required, JsonProperty("RepairId")]
        public long RepairId { get; set; }
    }
}
