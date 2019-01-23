
using AntiTruble.Repairs.DataModels;
using AntiTruble.Repairs.Enums;
using System.Collections.Generic;

namespace AntiTruble.Repairs.Core
{
    public interface IRepairsRepository
    {
        void RepairApplication(string name, string fio, RepairTypes repairType);
        void ChangeRepairStatus(long repairId, RepairStatuses status);
        RepairInfo GetRepairReport(long repairId);
        IEnumerable<RepairInfo> GetAllRepairs();
        RepairStatuses GetRepairStatus(long personId);
        bool TryToPayOrder(long repairId);
    }
}
