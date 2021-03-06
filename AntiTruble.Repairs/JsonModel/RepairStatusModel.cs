﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Repairs.JsonModel
{
    public class RepairStatusModel
    {
        [Required, JsonProperty("RepairId")]
        public long RepairId { get; set; }
        [Required, JsonProperty("Status")]
        public byte Status { get; set; }
    }
}
