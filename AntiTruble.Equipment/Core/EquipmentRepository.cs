using System.Collections.Generic;
using AntiTruble.Equipment.DataModels;
using AntiTruble.Equipment.Enums;
using AntiTruble.Equipment.Models;

namespace AntiTruble.Equipment.Core
{
    public class EquipmentRepository : IEquipmentRepository
    {
        public void AddDefect(string defectName, decimal? price = 0, DefectStatuses status = DefectStatuses.NotRepaired, long? equipmentId = null)
        {
            throw new System.NotImplementedException();
        }

        public void AddEquipment(string name, EquipmentTypes type, List<EquipmentDefects> defects, string phoneNumber)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEquipment(long equipmentId)
        {
            throw new System.NotImplementedException();
        }

        public EquipmentInfo SearchEquipments(long personId)
        {
            throw new System.NotImplementedException();
        }
    }
}
