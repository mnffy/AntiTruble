using AntiTruble.Equipment.DataModels;
using AntiTruble.Equipment.JsonModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Equipment.Core
{
    public interface IEquipmentRepository
    {
        Task AddEquipment(string name, byte type, IEnumerable<EquipmentInfoParamModel> defects, string fio);
        Task RemoveEquipment(long equipmentId);
        Task<IEnumerable<EquipmentInfo>> SearchEquipments(long personId);
    }
}
