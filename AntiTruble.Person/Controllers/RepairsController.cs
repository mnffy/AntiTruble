using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary;
using AntiTruble.ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace AntiTruble.Person.Controllers
{
    public class RepairsController : Controller
    {
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