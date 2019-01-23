using System.Collections.Generic;
using System.Threading.Tasks;
using AntiTruble.Repairs.DataModels;
using AntiTruble.Repairs.Enums;
using AntiTruble.Repairs.Models;

namespace AntiTruble.Repairs.Core
{
    public class RepairsRepository : IRepairsRepository
    {
        private readonly AntiTruble_RepairsContext _context;
        public RepairsRepository(AntiTruble_RepairsContext context)
        {
            _context = context;
        }

        public Task ChangeRepairStatus(long repairId, RepairStatuses status)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<RepairInfo>> GetAllRepairs()
        {
            throw new System.NotImplementedException();
        }

        public Task<RepairInfo> GetRepairReport(long repairId)
        {
            throw new System.NotImplementedException();
        }

        public Task<RepairStatuses> GetRepairStatus(long personId)
        {
            throw new System.NotImplementedException();
        }

        public Task RepairApplication(string name, string fio, RepairTypes repairType)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryToPayOrder(long repairId)
        {
            throw new System.NotImplementedException();
        }
    }
}
