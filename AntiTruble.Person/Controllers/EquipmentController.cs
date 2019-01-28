using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Models;
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
                var phoneNumber = identity.Claims.ToList()[0].Value;
                var personId = await _personsRepository.GetPersonIdByPhoneNumber(phoneNumber);
                var equipmentMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                    await RequestExecutor.ExecuteRequest(Scope.EquipmentMksUrl,
                        new RestRequest("/SearchEquipment/", Method.POST)
                             .AddHeader("Content-type", "application/json")
                             .AddJsonBody(new
                             {
                                 personId
                             })));
                if (!equipmentMksResult.Success)
                    throw new Exception(equipmentMksResult.Data);
                var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentInfo>>(equipmentMksResult.Data);
                return View(equipments);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(nameof(exception), exception.ToString());
                throw;
            }
        }
    }
}