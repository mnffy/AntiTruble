using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using AntiTruble.Person.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace AntiTruble.Person.Controllers
{
    public class RepairsController : Controller
    {
        private readonly IPersonsRepository _personsRepository;
        public RepairsController(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> AddRepair()
        {
            var persons = await _personsRepository.GetPersons();
            ViewBag.Clients = persons.Where(x => x.Role == (byte)PersonTypes.Client).Select(x => x.Fio);
            ViewBag.Masters = persons.Where(x => x.Role == (byte)PersonTypes.Master).Select(x => x.Fio);
            var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                   await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                       new RestRequest("/GetAllRepairs", Method.GET)
                            .AddHeader("Content-type", "application/json")));
            var repairs = JsonConvert.DeserializeObject<IEnumerable<RepairInfo>>(repairMksResult.Data);
            if (repairs == null)
                repairs = new List<RepairInfo>();
            var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                        new RestRequest("/GetAllEquipments", Method.GET)
                             .AddHeader("Content-type", "application/json")
                             ));
            //if (!equipmentMksResult.Success)
            //    throw new Exception(equipmentMksResult.Data);
            var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data);
            if (equipments == null)
                equipments = new List<EquipmentInfo>();
            var equipsInRepairsIds = new List<long>();
            foreach (var repair in repairs)
            {
                foreach(var equip in repair.Equipments)
                {
                    equipsInRepairsIds.Add(equip.EquipmentId);
                }
            }
            var equipmentsNotInRepair = equipments.Where(x => !equipsInRepairsIds.Any(y => y == x.EquipmentId));
            ViewBag.Equipments = equipmentsNotInRepair.Select(x => x.Name);
            return View("_AddRepair");
        }
        public async Task<IActionResult> RepairList()
        {
            try
            {
                var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                        new RestRequest("/GetAllRepairs", Method.GET)
                             .AddHeader("Content-type", "application/json")));
                var repairs = JsonConvert.DeserializeObject<IEnumerable<RepairInfo>>(repairMksResult.Data);
                if (repairs == null)
                    repairs = new List<RepairInfo>();
                return View(repairs);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}