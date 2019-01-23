
using AntiTruble.Equipment.DataModels;
using AntiTruble.Equipment.Enums;
using AntiTruble.Repairs.Models;
using System.Collections.Generic;

namespace AntiTruble.Equipment.Core
{
    public interface IEquipmentRepository
    {
        void AddEquipment(string name, EquipmentTypes type, List<EquipmentDefects> defects, string phoneNumber);
        void RemoveEquipment(long equipmentId);
        EquipmentInfo SearchEquipments(long personId);
        void AddDefect(string defectName, decimal? price = 0, DefectStatuses status = DefectStatuses.NotRepaired, long? equipmentId = null);
        void UpdateDefectStatus(long defectId, DefectStatuses status);
        
    }
}
