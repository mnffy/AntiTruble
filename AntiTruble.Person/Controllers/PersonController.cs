using AntiTruble.ClassLibrary.Filters;
using AntiTruble.Person.Core;
using AntiTruble.Person.JsonModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AntiTruble.Person.ControllerModels;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.ClassLibrary.Models;
using PersonModel = AntiTruble.Person.JsonModels.PersonModel;

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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            return View(await _personsRepository.GetPersons());
        }
        [HttpGet]
        public IActionResult AddUserView()
        {
            return View("_AddUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthModel model)
        {
            try
            {
                var authResult = await _personsRepository.Authorize(model.PhoneNumber, model.Password);
                if (authResult)
                {
                    await Authenticate(model.PhoneNumber); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Person");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(PersonModel model)
        {
            try
            {
                await _personsRepository.Registration(model);
                await Authenticate(model.PhoneNumber); // аутентификация
                return RedirectToAction("Index", "Home");
            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(PersonParam model)
        {
            try
            {
                await _personsRepository.UpdateUserData(model);
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
        [HttpPost]
        public async Task<IActionResult> AddUser(PersonParam model)
        {
            try
            {
                Enum.TryParse(model.Role, out PersonTypes role);
                await _personsRepository.Registration(new PersonModel
                {
                     Address = model.Address,
                     Balance = decimal.Parse(model.Balance),
                     Fio = model.Fio,
                     Password = model.Password,
                     PhoneNumber = model.PhoneNumber,
                     Role = (byte)role,
                     PersonId = long.Parse(model.PersonId)
                });
                return RedirectToAction("Users", "Person");
            }
            catch (Exception exception)
            {
                return Json(new { Success = false, exception.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string personId)
        {
            try
            {
                await _personsRepository.RemoveUser(long.Parse(personId));
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

        [HttpPost("UpdateBalance")]
        public async Task<IActionResult> UpdateBalance([FromBody]BalanceModel model)
        {
            try
            {
                await _personsRepository.UpdateBalance(model.ClientId, model.RepairCost);
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
    }
}