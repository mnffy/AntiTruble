using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using AntiTruble.Equipment.JsonModels;
using AntiTruble.Repairs.JsonModel;
using AntiTruble.Repairs.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace AntiTruble.Repairs.Core
{
    public class RepairsRepository : IRepairsRepository
    {
        private readonly AntiTruble_RepairsContext _context;
        public RepairsRepository(AntiTruble_RepairsContext context)
        {
            _context = context;
        }

        public async Task ChangeRepairStatus(RepairStatusModel status)
        {
            var repair = await _context.Repairs.FirstOrDefaultAsync(x => x.RepairId == status.RepairId);
            if (repair == null)
                throw new Exception("Repair not found");
            repair.Status = status.Status;
            await _context.SaveChangesAsync();
        }
        public async Task<byte> GetRepairStatus(long personId)
        {
            var repair = await _context.Repairs.FirstOrDefaultAsync(x => x.Client == personId);
            if (repair == null)
                throw new Exception("Repair not found");
            return repair.Status.Value;
        }
        public async Task<RepairInfo> GetRepairReport(long repairId)
        {
            var repair = await _context.Repairs.FirstOrDefaultAsync(x => x.RepairId == repairId);
            var repairInfo = new RepairInfo();
            if (repair == null)
                throw new Exception("Repair not found");
            var personMksResultWithClient = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.PersonMksUrl,
                      new RestRequest("/GetPersonById", Method.POST)
                          .AddHeader("Content-type", "application/json")
                          .AddJsonBody(new
                          {
                              id = repair.Client
                          })));
            if (!personMksResultWithClient.Success)
                throw new Exception(personMksResultWithClient.Data);
            var personMksResultWithMaster = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.PersonMksUrl,
                      new RestRequest("/GetPersonById", Method.POST)
                          .AddHeader("Content-type", "application/json")
                          .AddJsonBody(new
                          {
                              id = repair.Master
                          })));
            if (!personMksResultWithMaster.Success)
                throw new Exception(personMksResultWithMaster.Data);
            var master = JsonConvert.DeserializeObject<PersonModel>(personMksResultWithMaster.Data);
            var client = JsonConvert.DeserializeObject<PersonModel>(personMksResultWithClient.Data);
            var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                     new RestRequest("/SearchEquipmentsByRepair", Method.POST)
                         .AddHeader("Content-type", "application/json")
                         .AddParameter(new Parameter("repairId", repairId, ParameterType.RequestBody))));
            if (!equipmentMksResult.Success)
                throw new Exception(equipmentMksResult.Data);
            var equipmentsInfo = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data);
            var cost = default(decimal);
            foreach (var equip in equipmentsInfo)
                cost += equip.Defects.Sum(x => x.Price.Value);
            repairInfo.Status = (RepairStatuses)repair.Status.Value;
            repairInfo.StartDate = repair.StartDate.Value;
            repairInfo.EndDate = repair.EndDate.Value;
            repairInfo.RepairId = repair.RepairId;
            repairInfo.RepairType = (RepairTypes)repair.RepairType.Value;
            repairInfo.Master = new PersonModel
            {
                Address = master.Address,
                Balance = master.Balance,
                Fio = master.Fio,
                Password = master.Password,
                PersonId = master.PersonId,
                PhoneNumber = master.PhoneNumber,
                Role = master.Role
            };
            repairInfo.Client = new PersonModel
            {
                Address = client.Address,
                Balance = client.Balance,
                Fio = client.Fio,
                Password = client.Password,
                PersonId = client.PersonId,
                PhoneNumber = client.PhoneNumber,
                Role = client.Role
            };
            repairInfo.Equipments = equipmentsInfo;
            repairInfo.Cost = cost;
            return repairInfo;
        }
        public async Task<IEnumerable<RepairInfo>> GetAllRepairs()
        {
            var repairIds = _context.Repairs.Select(x => x.RepairId);
            var result = new List<RepairInfo>();
            foreach (var repairId in repairIds)
            {
                try
                {
                    result.Add(await GetRepairReport(repairId));
                }
                catch
                {
                    continue;
                }
            }
            return result;
        }

        public async Task<IEnumerable<RepairInfo>> GetRepairsById(long clientId)
        {
            var repairIds = _context.Repairs.Where(x => x.Client == clientId).Select(x => x.RepairId);
            var result = new List<RepairInfo>();
            foreach (var repairId in repairIds)
            {
                try
                {
                    result.Add(await GetRepairReport(repairId));
                }
                catch
                {
                    continue;
                }
            }
            return result;
        }

        public async Task<bool> TryToPayOrder(long repairId)
        {
            var repair = await _context.Repairs.FirstOrDefaultAsync(x => x.RepairId == repairId);
            if (repair == null)
                throw new Exception("Repair not found");

            var personMksResult1 = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.PersonMksUrl,
                      new RestRequest("/GetPersonById", Method.POST)
                          .AddHeader("Content-type", "application/json")
                          .AddJsonBody(new
                          {
                              id = repair.Client.Value
                          })));
            if (!personMksResult1.Success)
                throw new Exception(personMksResult1.Data);
            var person = JsonConvert.DeserializeObject<PersonModel>(personMksResult1.Data);
            var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                    new RestRequest("/SearchEquipmentsByRepair", Method.POST)
                        .AddHeader("Content-type", "application/json")
                        .AddParameter(new Parameter("repairId", repairId, ParameterType.RequestBody))));
            if (!equipmentMksResult.Success)
                throw new Exception(equipmentMksResult.Data);
            var equipmentsInfo = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data);
            var cost = default(decimal);
            foreach (var equip in equipmentsInfo)
                cost += equip.Defects.Sum(x => x.Price.Value);
            if (person.Balance < cost)
                return false;
            var personMksResult2 = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.PersonMksUrl,
                      new RestRequest("/UpdateBalance", Method.POST)
                          .AddHeader("Content-type", "application/json")
                          .AddJsonBody(JsonConvert.SerializeObject(new BalanceModel
                          {
                              ClientId = person.PersonId,
                              RepairCost = cost
                          }))));
            if (!personMksResult1.Success)
                throw new Exception(personMksResult1.Data);
            repair.Status = (byte)RepairStatuses.Paid;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task RemoveRepair(long repairId)
        {
            var repair = await _context.Repairs.FirstOrDefaultAsync(x => x.RepairId == repairId);
            if (repair == null)
                throw new Exception("Repair not found");
            var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                    new RestRequest("/SearchEquipmentsByRepair", Method.POST)
                        .AddHeader("Content-type", "application/json")
                        .AddParameter(new Parameter("repairId", repairId, ParameterType.RequestBody))));
            if (!equipmentMksResult.Success)
                throw new Exception(equipmentMksResult.Data);
            var equipmentsInfo = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data);
            foreach (var equip in equipmentsInfo)
            {
                var equipmentMksResult2 = JsonConvert.DeserializeObject<MksResponseResult>(
                await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                     new RestRequest("/RemoveEquipment", Method.POST)
                         .AddHeader("Content-type", "application/json")
                         .AddJsonBody(JsonConvert.SerializeObject(new RemovingEquipmentModel
                         {
                             EquipmentId = equip.EquipmentId
                         }))));
                if (!equipmentMksResult2.Success)
                    throw new Exception(equipmentMksResult2.Data);
            }
            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
        }
        public async Task<long> RepairApplication(RepairApplicationModel repairModel)
        {
            var personMksResultWithClient = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.PersonMksUrl,
                   new RestRequest("/GetPersonIdByFIO", Method.POST)
                       .AddHeader("Content-type", "application/json")
                       .AddJsonBody(new
                       {
                           fio = repairModel.ClientFIO
                       })));
            if (!personMksResultWithClient.Success)
                throw new Exception(personMksResultWithClient.Data);
            var personMksResultWithMaster = JsonConvert.DeserializeObject<MksResponseResult>(
               await RequestExecutor.ExecuteRequest(Scope.PersonMksUrl,
                      new RestRequest("/GetPersonIdByFIO", Method.POST)
                          .AddHeader("Content-type", "application/json")
                          .AddJsonBody(new
                          {
                              fio = repairModel.MasterFIO
                          })));
            if (!personMksResultWithMaster.Success)
                throw new Exception(personMksResultWithMaster.Data);
            var repair = new Models.Repairs
            {
                StartDate = repairModel.StartDate,
                EndDate = repairModel.EndDate,
                RepairType = repairModel.RepairType,
                Status = (byte)RepairStatuses.Confirm,
                Client = long.Parse(personMksResultWithClient.Data),
                Master = long.Parse(personMksResultWithMaster.Data)
            };
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();
            return repair.RepairId;
        }
    }
}
