using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using AntiTruble.Equipment.JsonModels;
using AntiTruble.Person.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AntiTruble.Person.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly IPersonsRepository _personsRepository;
        public EquipmentController(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var _userPhoneNumber = identity.Claims.ToList()[0].Value;
                var person = await _personsRepository.GetPersonByPhoneNumber(_userPhoneNumber);
                var equipments = new List<EquipmentInfo>();
                if (person.Role == (byte)PersonTypes.Client)
                {
                    var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                        await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                           new RestRequest("/SearchEquipmentsByPerson", Method.POST)
                                .AddHeader("Content-type", "application/json")
                                .AddParameter(new Parameter("personId", person.PersonId, ParameterType.RequestBody))));
                    equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data).ToList();
                }
                else
                {
                    var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                           await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                              new RestRequest("/GetAllEquipments", Method.GET)
                                   .AddHeader("Content-type", "application/json")
                                   ));
                    equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data).ToList();
                }
                return View(equipments);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAddPartial()
        {
            var persons = await _personsRepository.GetPersons();
            ViewBag.Clients = persons.Where(x => x.Role == (byte)PersonTypes.Client).Select(x => x.Fio);
            return View("_AddEquipment", new EquipmentParamModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddEquipment(EquipmentInfoParam model)
        {
            try
            {
                if (!Enum.TryParse(model.EquipmentType, out EquipmentTypes type))
                    type = EquipmentTypes.OtherDevice;
                var defects = new List<EquipmentInfoParamModel>();
                foreach (var defect in model.Defects)
                {
                    defects.Add(new EquipmentInfoParamModel
                    {
                        DefectName = defect.DefectName,
                        Price = decimal.Parse(defect.Price)
                    });
                }
                var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                        new RestRequest("/CreateEquipment", Method.POST)
                             .AddHeader("Content-type", "application/json")
                             .AddJsonBody(JsonConvert.SerializeObject(new EquipmentParamModel
                             {
                                 Name = model.Name,
                                 RepairId = long.Parse(model.RepairId),
                                 EquipmentType = (byte)type,
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
        public async Task<IActionResult> RemoveEquipment(int equipmentId)
        {
            try
            {
                var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                        new RestRequest("/RemoveEquipment", Method.POST)
                             .AddHeader("Content-type", "application/json")
                             .AddJsonBody(new
                             {
                                 equipmentId
                             })));
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
    }
}