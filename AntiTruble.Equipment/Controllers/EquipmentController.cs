using System;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary.Filters;
using AntiTruble.Equipment.Core;
using AntiTruble.Equipment.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AntiTruble.Equipment.Controllers
{
    [ValidateModel]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentRepository _equipmentRepository;
        public EquipmentController(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        [HttpPost("SearchEquipmentsByPerson")]
        public async Task<IActionResult> SearchEquipmentsByPerson([FromBody]long personId)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _equipmentRepository.SearchEquipmentsByPerson(personId))
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        [HttpPost("SearchEquipmentsByRepair")]
        public async Task<IActionResult> SearchEquipmentsByRepair([FromBody]long repairId)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _equipmentRepository.SearchEquipmentsByRepair(repairId))
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        [HttpGet("GetAllEquipments")]
        public async Task<IActionResult> GetAllEquipments()
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _equipmentRepository.GetAllEquipments())
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        [HttpPost("CreateEquipment")]
        public async Task<IActionResult> CreateEquipment([FromBody]EquipmentParamModel model)
        {
            try
            {
                await _equipmentRepository.AddEquipment(model.Name, model.EquipmentType, model.Defects, model.RepairId);
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

        [HttpPost("RemoveEquipment")]
        public async Task<IActionResult> RemoveEquipment([FromBody]RemovingEquipmentModel model)
        {
            try
            {
                await _equipmentRepository.RemoveEquipment(model.EquipmentId);
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
    }
}