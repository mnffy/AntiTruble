using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using AntiTruble.Equipment.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace AntiTruble.Equipment.Core
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AntiTruble_EquipmentContext _context;
        public EquipmentRepository(AntiTruble_EquipmentContext context)
        {
            _context = context;
        }
        public async Task AddEquipment(string name, byte type, long repairId)
        {
            var equipment = new Equipments
            {
                EquipmentType = type,
                Name = name,
                RepairId = repairId
            };
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
           
        }

        public async Task AddDefects(long equipmentId, long repairId, IEnumerable<EquipmentDefectsParam> defects)
        {
            var equipment = await _context.Equipments.FirstOrDefaultAsync(x => x.EquipmentId == equipmentId);
            if (equipment == null)
                throw new Exception("Equipment not found");
            var repairDaysSum = default(int);
            foreach (var defect in defects)
            {
                _context.EquipmentDefects.Add(new EquipmentDefects
                {
                    DefectName = defect.DefectName,
                    EquipmentId = equipment.EquipmentId,
                    Price = decimal.Parse(defect.Price.Replace('.',','))
                });
                repairDaysSum += int.Parse(defect.RepairDays);
            }
            var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                        new RestRequest("/UpdateRepairDays", Method.POST)
                            .AddHeader("Content-type", "application/json")
                            .AddJsonBody(JsonConvert.SerializeObject(new RepairWithDaysModel
                            {
                                RepairId = repairId,
                                RepairDays = repairDaysSum
                            }))));
            if (!repairMksResult.Success)
                throw new Exception(repairMksResult.Data);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEquipment(long equipmentId)
        {
            var defects = _context.EquipmentDefects.Where(x => x.EquipmentId == equipmentId);
            if (defects != null)
                _context.EquipmentDefects.RemoveRange(defects);
            var equipment = await _context.Equipments.FirstOrDefaultAsync(x => x.EquipmentId == equipmentId);
            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<EquipmentInfo>> SearchEquipmentsByRepair(long repairId)
        {
            var result = new List<EquipmentInfo>();
            var equips = _context.Equipments.Where(x => x.RepairId == repairId);
            foreach (var equip in equips)
            {
                var equipmentInfo = new EquipmentInfo
                {
                    EquipmentId = equip.EquipmentId,
                    EquipmentType = (EquipmentTypes)equip.EquipmentType,
                    Name = equip.Name,
                    Repair = repairId
                };
                var defects = _context.EquipmentDefects.Where(x => x.EquipmentId == equip.EquipmentId);
                if (defects != null)
                    equipmentInfo.Defects = await defects.Select(x => new EquipmentDefectsModel
                    {
                        DefectId = x.DefectId,
                        DefectName = x.DefectName,
                        EquipmentId = x.EquipmentId,
                        Price = x.Price
                    }).ToListAsync();
                result.Add(equipmentInfo);
            }
            if (!result.Any())
                throw new Exception("Equipments not found");
            return result;

        }
        public async Task<IEnumerable<EquipmentInfo>> SearchEquipmentsByPerson(long personId)
        {
            var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                      await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                          new RestRequest("/GetRepairsById", Method.POST)
                              .AddHeader("Content-type", "application/json")
                              .AddParameter(new Parameter("personId", personId, ParameterType.RequestBody))));
            if (!repairMksResult.Success)
                throw new Exception(repairMksResult.Data);

            var repairs = JsonConvert.DeserializeObject<IEnumerable<RepairInfo>>(repairMksResult.Data).ToList();
            var result = new List<EquipmentInfo>();
            foreach(var repair in repairs)
            {
                var equips = _context.Equipments.Where(x => x.RepairId == repair.RepairId);
                foreach (var equip in equips)
                {
                    var equipmentInfo = new EquipmentInfo
                    {
                        EquipmentId = equip.EquipmentId,
                        EquipmentType = (EquipmentTypes)equip.EquipmentType,
                        Name = equip.Name,
                        Repair = equip.RepairId
                    };
                    var defects = _context.EquipmentDefects.Where(x => x.EquipmentId == equip.EquipmentId);
                    if (defects != null)
                        equipmentInfo.Defects = await defects.Select(x => new EquipmentDefectsModel
                        {
                            DefectId = x.DefectId,
                            DefectName = x.DefectName,
                            EquipmentId = x.EquipmentId,
                            Price = x.Price
                        }).ToListAsync();
                    result.Add(equipmentInfo);
                }
                if (!result.Any())
                    throw new Exception("Equipments not found");
            }
            return result;
        }

        public async Task<IEnumerable<EquipmentInfo>> GetAllEquipments()
        {
            var result = new List<EquipmentInfo>();
            var equips = _context.Equipments;
            foreach (var equip in equips)
            {
                var equipmentInfo = new EquipmentInfo
                {
                    EquipmentId = equip.EquipmentId,
                    EquipmentType = (EquipmentTypes)equip.EquipmentType,
                    Name = equip.Name,
                    Repair = equip.RepairId
                };
                var defects = _context.EquipmentDefects.Where(x => x.EquipmentId == equip.EquipmentId);
                if (defects != null)
                    equipmentInfo.Defects = await defects.Select(x => new EquipmentDefectsModel
                    {
                        DefectId = x.DefectId,
                        DefectName = x.DefectName,
                        EquipmentId = x.EquipmentId,
                        Price = x.Price
                    }).ToListAsync();
                result.Add(equipmentInfo);
            }
            if (!result.Any())
                throw new Exception("Equipments not found");
            return result;
        }
    }
}
