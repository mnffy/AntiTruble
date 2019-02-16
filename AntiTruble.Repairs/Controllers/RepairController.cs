using System;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary.Filters;
using AntiTruble.ClassLibrary.Models;
using AntiTruble.Repairs.Core;
using AntiTruble.Repairs.JsonModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AntiTruble.Repairs.Controllers
{
    [ValidateModel]
    public class RepairController : Controller
    {
        private readonly IRepairsRepository _repairsRepository;
        public RepairController(IRepairsRepository repairsRepository)
        {
            _repairsRepository = repairsRepository;
        }
        [HttpPost("TryToPayOrder")]
        public async Task<IActionResult> TryToPayOrder([FromBody]PayOrderModel model)
        {
            try
            {
                var result = await _repairsRepository.TryToPayOrder(model.RepairId);
                return Json(
                      new
                      {
                          Success = true,
                          Data = result.ToString().ToLower()
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        [HttpGet("GetAllRepairs")]
        public async Task<IActionResult> GetAllRepairs()
        {
            try
            { 
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _repairsRepository.GetAllRepairs())
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        [HttpPost("UpdateRepairDays")]
        public async Task<IActionResult> UpdateRepairDays([FromBody]RepairWithDaysModel model)
        {
            try
            {
                await _repairsRepository.UpdateRepairDays(model.RepairId, model.RepairDays);
                return Json(
                      new
                      {
                          Success = true,
                          Data = "Ok"
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        [HttpPost("GetRepairsById")]
        public async Task<IActionResult> GetRepairsById([FromBody]long personId)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _repairsRepository.GetRepairsById(personId))
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        
        [HttpPost("GetRepairReport")]
        public async Task<IActionResult> GetRepairReport([FromBody]long repairId)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _repairsRepository.GetRepairReport(repairId))
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        [HttpGet("RemoveRepair")]
        public async Task<IActionResult> RemoveRepair(long repairId)
        {
            try
            {
                await _repairsRepository.RemoveRepair(repairId);
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
        [HttpPost("ChangeRepairStatus")]
        public async Task<IActionResult> ChangeRepairStatus([FromBody]RepairStatusModel model)
        {
            try
            {
                await _repairsRepository.ChangeRepairStatus(model);
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
        [HttpPost("UpdateRepairMaster")]
        public async Task<IActionResult> UpdateRepairMaster([FromBody]RepairWithMasterModel model)
        {
            try
            {
                await _repairsRepository.UpdateRepairMaster(model);
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

        [HttpPost("RepairApplication")]
        public async Task<IActionResult> RepairApplication([FromBody]RepairApplicationModel model)
        {
            try
            {
                var repairId = await _repairsRepository.RepairApplication(model);
                return Json(
                      new
                      {
                          Success = true,
                          Data = repairId
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
    }
}