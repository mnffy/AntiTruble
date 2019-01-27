using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Repairs.JsonModel
{
    public class RepairApplicationModel
    {
        [Required, JsonProperty("ClientFIO")]
        public string ClientFIO { get; set; }
        [Required, JsonProperty("MasterFIO")]
        public string MasterFIO { get; set; }
        [Required, JsonProperty("RepairType")]
        public byte RepairType { get; set; }
        [Required, JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }
        [Required, JsonProperty("EndDate")]
        public DateTime EndDate { get; set; }
    }
}
