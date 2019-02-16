using AntiTruble.ClassLibrary.Models;
using AntiTruble.Repairs.JsonModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Repairs.Core
{
    public interface IRepairsRepository
    {
        Task<long> RepairApplication(RepairApplicationModel repair);
        Task ChangeRepairStatus(RepairStatusModel status);
        Task UpdateRepairMaster(RepairWithMasterModel model);
        Task<RepairInfo> GetRepairReport(long repairId);
        Task<IEnumerable<RepairInfo>> GetAllRepairs();
        Task<IEnumerable<RepairInfo>> GetRepairsById(long clientId);
        Task UpdateRepairDays(long repairId, int repairDays);
        Task<byte> GetRepairStatus(long personId);
        Task<bool> TryToPayOrder(long repairId);
        Task RemoveRepair(long repairId);
    }
}
