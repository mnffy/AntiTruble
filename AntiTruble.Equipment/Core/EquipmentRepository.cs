using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using AntiTruble.Equipment.JsonModels;
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
        public async Task AddEquipment(string name, byte type, IEnumerable<EquipmentInfoParamModel> defects, string fio)
        {
            var personMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                await RequestExecutor.ExecuteRequest(Scope.PersonMksUrl,
                       new RestRequest("/GetPersonIdByFIO/", Method.POST)
                           .AddHeader("Content-type", "application/json")
                           .AddJsonBody(new
                           {
                               fio
                           })));
            if (!personMksResult.Success)
                throw new Exception(personMksResult.Data);
            var equipment = new Equipments
            {
                EquipmentType = type,
                Name = name,
                OwnerId = long.Parse(personMksResult.Data)
            };
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
            foreach (var defect in defects)
            {
                _context.EquipmentDefects.Add(new EquipmentDefects
                {
                    DefectName = defect.DefectName,
                    EquipmentId = equipment.EquipmentId,
                    Price = defect.Price
                });
            }
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

        public async Task<IEnumerable<EquipmentInfo>> SearchEquipments(long personId)
        {
            var result = new List<EquipmentInfo>();
            var equips = _context.Equipments.Where(x => x.OwnerId == personId);
            foreach(var equip in equips)
            {
                var equipmentInfo = new EquipmentInfo
                {
                    EquipmentId = equip.EquipmentId,
                    EquipmentType = (EquipmentTypes)equip.EquipmentType,
                    Name = equip.Name
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
