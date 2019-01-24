using System;
using System.Threading.Tasks;
using AntiTruble.Equipment.Core;
using AntiTruble.Equipment.JsonModels;
using AntiTruble.Person.Filters;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("SearchEquipment/{personId}")]
        public async Task<IActionResult> SearchEquipment(long personId)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = await _equipmentRepository.SearchEquipments(personId)
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
                await _equipmentRepository.AddEquipment(model.Name, model.EquipmentType, model.Defects, model.Fio);
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

        [HttpPost("RemoveEquipment/{equipmentId}")]
        public async Task<IActionResult> RemoveEquipment(long equipmentId)
        {
            try
            {
                await _equipmentRepository.RemoveEquipment(equipmentId);
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