
using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Enums;
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
   // [Authorize]
    public class ReportController : Controller
    {
        private readonly IPersonsRepository _personsRepository;
        public ReportController(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var repairs = new List<RepairInfo>();
                var identity = (ClaimsIdentity)User.Identity;
                var userPhoneNumber = identity.Claims.ToList()[0].Value;
                var person = await _personsRepository.GetPersonByPhoneNumber(userPhoneNumber);
                if (person.Role == (byte)PersonTypes.Administator)
                {
                    var repairMksResult = JsonConvert.DeserializeObject<MksResponseResult>(
                        await RequestExecutor.ExecuteRequest(Scope.RepairsMksUrl,
                            new RestRequest("/GetAllRepairs/", Method.GET)
                                .AddHeader("Content-type", "application/json")));
                    repairs = JsonConvert.DeserializeObject<IEnumerable<RepairInfo>>(repairMksResult.Data).ToList();
                }
                else
                    return RedirectToAction("Index", "Home");
                ViewBag.Role = (PersonTypes)person.Role;
                ViewBag.TotalCost = repairs.Sum(x => x.Cost);
                return View(repairs);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}