using AntiTruble.Equipment.DataModels;
using AntiTruble.Person.Models;
using AntiTruble.Repairs.Enums;
using System;
using System.Collections.Generic;

namespace AntiTruble.Repairs.DataModels
{
    public class RepairInfo
    {
        public long RepairId { get; set; }
        public RepairTypes RepairType { get; set; }
        public RepairStatuses Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Persons Client { get; set; }
        public Persons Master { get; set; }
        public IEnumerable<EquipmentInfo> Equipments { get; set; }
        public decimal Cost { get; set; }
    }
}
