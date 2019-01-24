using System;

namespace AntiTruble.Repairs.JsonModel
{
    public class RepairApplicationModel
    {
        public string ClientFIO { get; set; } 
        public string MasterFIO { get; set; }
        public byte RepairType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
