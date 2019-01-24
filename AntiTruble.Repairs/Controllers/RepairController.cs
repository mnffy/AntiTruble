using System;
using System.Threading.Tasks;
using AntiTruble.Repairs.Core;
using AntiTruble.Repairs.Filters;
using AntiTruble.Repairs.JsonModel;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("TryToPayOrder/{repairId}")]
        public async Task<IActionResult> TryToPayOrder(long repairId)
        {
            try
            {
                var result = await _repairsRepository.TryToPayOrder(repairId);
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
                          Data = await _repairsRepository.GetAllRepairs()
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        [HttpGet("GetRepairStatus/{personId}")]
        public async Task<IActionResult> GetRepairStatus(long personId)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = await _repairsRepository.GetRepairStatus(personId)
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        [HttpGet("GetRepairReport/{repairId}")]
        public async Task<IActionResult> GetRepairReport(long repairId)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = await _repairsRepository.GetRepairReport(repairId)
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
       
        [HttpPost("RepairApplication")]
        public async Task<IActionResult> RepairApplication([FromBody]RepairApplicationModel model)
        {
            try
            {
                await _repairsRepository.RepairApplication(model);
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
    }
}