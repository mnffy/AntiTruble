using AntiTruble.Person.Core;
using AntiTruble.Person.Filters;
using AntiTruble.Person.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AntiTruble.Person.Controllers
{
    [ValidateModel]
    public class PersonController : Controller
    {
        private readonly IPersonsRepository _personsRepository;
        public PersonController(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
        }
       
        [HttpPost("GetPersonIdByFIO")]
        public async Task<IActionResult> GetPersonIdByFIO([FromBody] PersonIdByFioModel model)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = await _personsRepository.GetPersonIdByFIO(model.Fio)
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        [HttpPost("GetPersonById")]
        public async Task<IActionResult> GetPersonById([FromBody]PersonByIdModel model)
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = JsonConvert.SerializeObject(await _personsRepository.GetPersonById(model.Id))
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        [HttpGet("GetPersons")]
        public async Task<IActionResult> GetPersons()
        {
            try
            {
                return Json(
                      new
                      {
                          Success = true,
                          Data = await _personsRepository.GetPersons()
                      });

            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody]PersonModel model)
        {
            try
            {
                await _personsRepository.Registration(model);
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
        [HttpPost("Authorization")]
        public async Task<IActionResult> Authorization([FromBody]AuthModel model)
        {
            try
            {
                var result = await _personsRepository.Authorize(model.PhoneNumber, model.Password);
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

    }
}