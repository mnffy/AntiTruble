using AntiTruble.Equipment.DataModels;
using AntiTruble.Equipment.Enums;
using AntiTruble.Equipment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Equipment.Core
{
    public interface IEquipmentRepository
    {
        Task AddEquipment(string name, EquipmentTypes type, List<EquipmentDefects> defects, string fio);
        Task RemoveEquipment(long equipmentId);
        Task<IEnumerable<EquipmentInfo>> SearchEquipments(long personId);
    }
}
