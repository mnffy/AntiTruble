using System;
using System.Threading.Tasks;
using AntiTruble.Equipment.Core;
using AntiTruble.Equipment.JsonModels;
using AntiTruble.Person.Filters;
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

        [HttpPost("SearchEquipment")]
        public async Task<IActionResult> SearchEquipment([FromBody] SearchingModel model)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _equipmentRepository.SearchEquipments(model.PersonId))
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