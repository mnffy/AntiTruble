using AntiTruble.ClassLibrary.Models;
using AntiTruble.Repairs.JsonModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Repairs.Core
{
    public interface IRepairsRepository
    {
        Task RepairApplication(RepairApplicationModel repair);
        Task ChangeRepairStatus(RepairStatusModel status);
        Task<RepairInfo> GetRepairReport(long repairId);
        Task<IEnumerable<RepairInfo>> GetAllRepairs();
        Task<byte> GetRepairStatus(long personId);
        Task<bool> TryToPayOrder(long repairId);
    }
}
