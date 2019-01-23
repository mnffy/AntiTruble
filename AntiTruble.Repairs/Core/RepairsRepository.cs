using AntiTruble.Repairs.DataModels;
using AntiTruble.Repairs.Enums;

namespace AntiTruble.Repairs.Core
{
    public class RepairsRepository : IRepairsRepository
    {
        public void ChangeRepairStatus(long repairId, RepairStatuses status)
        {
            throw new System.NotImplementedException();
        }

        public RepairInfo GetRepairReport(long repairId)
        {
            throw new System.NotImplementedException();
        }

        public RepairStatuses GetRepairStatus(long personId)
        {
            throw new System.NotImplementedException();
        }

        public void RepairApplication(string name, string fio, RepairTypes repairType)
        {
            throw new System.NotImplementedException();
        }

        public bool TryToPayOrder(long repairId)
        {
            throw new System.NotImplementedException();
        }
    }
}
