using AntiTruble.Repairs.DataModels;
using AntiTruble.Repairs.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Repairs.Core
{
    public interface IRepairsRepository
    {
        Task RepairApplication(string clientFIO, string masterFIO, long masterId, RepairTypes repairType, DateTime startDate, DateTime endDate);
        Task ChangeRepairStatus(long repairId, RepairStatuses status);
        Task<RepairInfo> GetRepairReport(long repairId);
        Task<IEnumerable<RepairInfo>> GetAllRepairs();
        Task<RepairStatuses> GetRepairStatus(long personId);
        Task<bool> TryToPayOrder(long repairId);
    }
}
