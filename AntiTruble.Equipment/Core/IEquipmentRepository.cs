using AntiTruble.ClassLibrary.Models;
using AntiTruble.Equipment.JsonModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Equipment.Core
{
    public interface IEquipmentRepository
    {
        Task AddEquipment(string name, byte type, IEnumerable<EquipmentInfoParamModel> defects, long repairId);
        Task RemoveEquipment(long equipmentId);
        Task<IEnumerable<EquipmentInfo>> SearchEquipmentsByRepair(long repairId);
        Task<IEnumerable<EquipmentInfo>> SearchEquipmentsByPerson(long personId);
        Task<IEnumerable<EquipmentInfo>> GetAllEquipments();
    }
}
