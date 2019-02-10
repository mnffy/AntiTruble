using System;
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
        private async Task<PersonTypes> InitRole()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (identity.Claims.Any())
            {
                var userPhoneNumber = identity.Claims.ToList()[0].Value;
                var person = await _personsRepository.GetPersonByPhoneNumber(userPhoneNumber);
                return (PersonTypes)person.Role;
            }
            return PersonTypes.None;
        }

        [HttpGet]
        public async Task<IActionResult> AddRepair()
        {
            var persons = await _personsRepository.GetPersons();
            ViewBag.Clients = persons.Where(x => x.Role == (byte)PersonTypes.Client).Select(x => x.Fio);
            ViewBag.Masters = persons.Where(x => x.Role == (byte)PersonTypes.Master).Select(x => x.Fio);
            ViewBag.Role = await InitRole();
            return View("_AddRepair");
        }

        [HttpGet]
        public async Task<IActionResult> RepairDetails(string repairId)
        {
            ViewBag.Role = await InitRole();
            var result = new RepairInfo();
            var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                   await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                       new RestRequest("/GetRepairReport", Method.GET)
                            .AddHeader("Content-type", "application/json")
                            .AddParameter(new Parameter("repairId", long.Parse(repairId), ParameterType.GetOrPost))));
            if (!repairMksResult.Success)
                throw new Exception(repairMksResult.Data);
            else
                result = JsonConvert.DeserializeObject<RepairInfo>(repairMksResult.Data);
            return View("_RepairDetails", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRepair(RepairParamModel model)
        {
            try
            {
                ViewBag.Role = await InitRole();
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
                ViewBag.Role = await InitRole();
                if (!Enum.TryParse(model.Status, out RepairStatuses repairStatus))
                    throw new Exception("Parsing error");
                if (repairStatus == RepairStatuses.Cancel)
                {
                    var repairMksResult2 = JsonConvert.DeserializeObject<MksResponseResult>(
                       await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                           new RestRequest("/RemoveRepair", Method.GET)
                                .AddHeader("Content-type", "application/json")
                                .AddParameter(new Parameter("repairId", long.Parse(model.RepairId), ParameterType.GetOrPost))));
                    if (!repairMksResult2.Success)
                        throw new Exception(repairMksResult2.Data);
                }

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

        [HttpPost]
        public async Task<IActionResult> PayRepair(string repairId)
        {
            try
            {
                ViewBag.Role = await InitRole();
                var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                        new RestRequest("/TryToPayOrder", Method.POST)
                             .AddHeader("Content-type", "application/json")
                             .AddJsonBody(JsonConvert.SerializeObject(new PayOrderModel
                             {
                                 RepairId = long.Parse(repairId),
                             }))));
                if (!repairMksResult.Success)
                    throw new Exception(repairMksResult.Data);
                return Json(
                      new
                      {
                          Success = true,
                          repairMksResult.Data
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
                           new RestRequest("/GetRepairsById", Method.POST)
                               .AddHeader("Content-type", "application/json")
                               .AddParameter(new Parameter("personId", person.PersonId, ParameterType.RequestBody))));
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