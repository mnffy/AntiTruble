﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using AntiTruble.Equipment.JsonModels;
using AntiTruble.Person.ControllerModels;
using AntiTruble.Person.Core;
using AntiTruble.Repairs.JsonModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace AntiTruble.Person.Controllers
{
    [Authorize]
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
            //var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
            //       await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
            //           new RestRequest("/GetAllRepairs", Method.GET)
            //                .AddHeader("Content-type", "application/json")));
            //var repairs = JsonConvert.DeserializeObject<IEnumerable<RepairInfo>>(repairMksResult.Data);
            //if (repairs == null)
            //    repairs = new List<RepairInfo>();
            //var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
            //        await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
            //            new RestRequest("/GetAllEquipments", Method.GET)
            //                 .AddHeader("Content-type", "application/json")
            //                 ));
            ////if (!equipmentMksResult.Success)
            ////    throw new Exception(equipmentMksResult.Data);
            //var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data);
            //if (equipments == null)
            //    equipments = new List<EquipmentInfo>();
            //var equipsInRepairsIds = new List<long>();
            //foreach (var repair in repairs)
            //{
            //    foreach(var equip in repair.Equipments)
            //    {
            //        equipsInRepairsIds.Add(equip.EquipmentId);
            //    }
            //}
            //var equipmentsNotInRepair = equipments.Where(x => !equipsInRepairsIds.Any(y => y == x.EquipmentId));
            //ViewBag.Equipments = equipmentsNotInRepair.Select(x => x.Name);
            return View("_AddRepair");
        }
        [HttpPost]
        public async Task<IActionResult> AddRepair(RepairParamModel model)
        {
            try
            {
                if (!Enum.TryParse(model.EType, out EquipmentTypes equipmentType))
                    equipmentType = EquipmentTypes.OtherDevice;
                if (!Enum.TryParse(model.RType, out RepairTypes repairType))
                    repairType = RepairTypes.FirstOfAll;
                if (!int.TryParse(model.Days, out var repairDays))
                    repairDays = 30;
                var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                  await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                      new RestRequest("/RepairApplication", Method.POST)
                           .AddHeader("Content-type", "application/json")
                           .AddJsonBody(JsonConvert.SerializeObject(new RepairApplicationModel
                           {
                               ClientFIO = model.Client,
                               MasterFIO = model.Master,
                               RepairType = (byte)repairType,
                               StartDate = DateTime.UtcNow,
                               EndDate = DateTime.UtcNow.AddDays(repairDays)
                           }))));
                if (!repairMksResult.Success)
                    throw new Exception(repairMksResult.Data);
                var defects = new List<EquipmentInfoParamModel>();
                foreach (var defect in model.Defects)
                {
                    defects.Add(new EquipmentInfoParamModel
                    {
                        DefectName = defect.DefectName,
                        Price = decimal.Parse(defect.Price),
                    });
                }
                var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                        new RestRequest("/CreateEquipment", Method.POST)
                             .AddHeader("Content-type", "application/json")
                             .AddJsonBody(JsonConvert.SerializeObject(new EquipmentParamModel
                             {
                                 Name = model.EquipmentName,
                                 RepairId = long.Parse(repairMksResult.Data),
                                 EquipmentType = (byte)equipmentType,
                                 Defects = defects
                             }))));
                if (!equipmentMksResult.Success)
                    throw new Exception(equipmentMksResult.Data);
               
                return RedirectToAction("Index", "Equipment");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(nameof(exception), exception.ToString());
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRepairStatus(RepairStatusParamModel model)
        {
            try
            {
                if (!Enum.TryParse(model.Status, out RepairStatuses repairStatus))
                    throw new Exception("Parsing error");
                var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                        new RestRequest("/ChangeRepairStatus", Method.POST)
                             .AddHeader("Content-type", "application/json")
                             .AddJsonBody(JsonConvert.SerializeObject(new RepairStatusModel
                             {
                                 RepairId = long.Parse(model.RepairId),
                                 Status = (byte)repairStatus
                             }))));
                if (!repairMksResult.Success)
                    throw new Exception(repairMksResult.Data);
                return Json(
                      new
                      {
                          Success = true,
                          Data = "ok"
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        public async Task<IActionResult> RepairList()
        {
            try
            {
                var repairs = new List<RepairInfo>();
                var identity = (ClaimsIdentity)User.Identity;
                var userPhoneNumber = identity.Claims.ToList()[0].Value;
                var person = await _personsRepository.GetPersonByPhoneNumber(userPhoneNumber);
                if (person.Role == (byte)PersonTypes.Administator || person.Role == (byte)PersonTypes.Master)
                {
                    var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                        await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                            new RestRequest("/GetAllRepairs/", Method.GET)
                                .AddHeader("Content-type", "application/json")));
                    repairs = JsonConvert.DeserializeObject<IEnumerable<RepairInfo>>(repairMksResult.Data).ToList();
                }
                else
                {
                    var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                       await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                           new RestRequest("/GetRepairsById", Method.GET)
                               .AddHeader("Content-type", "application/json")
                               .AddParameter(new Parameter("clientId", person.PersonId, ParameterType.GetOrPost))));
                    repairs = JsonConvert.DeserializeObject<IEnumerable<RepairInfo>>(repairMksResult.Data).ToList();
                }
                ViewBag.Role = (PersonTypes)person.Role;
                return View(repairs);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}