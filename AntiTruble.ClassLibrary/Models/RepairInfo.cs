using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using System;
using System.Collections.Generic;

namespace AntiTruble.ClassLibrary.Models
{
    public class RepairInfo
    {
        public long RepairId { get; set; }
        public RepairTypes RepairType { get; set; }
        public RepairStatuses Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PersonModel Client { get; set; }
        public PersonModel Master { get; set; }
        public IEnumerable<EquipmentInfo> Equipments { get; set; }
        public decimal Cost { get; set; }
    }

    public class PersonModel
    {
        public long PersonId { get; set; }
        public string Fio { get; set; }
        public string Password { get; set; }
        public byte? Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal? Balance { get; set; }
    }
}
