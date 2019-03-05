using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.Person.Core;
using System.Threading.Tasks;

namespace AntiTruble.Person.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonsRepository _personsRepository;
        public HomeController(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
        }
        
        public async Task<IActionResult> Index()
        {
            var role = PersonTypes.None;
            var identity = (ClaimsIdentity)User.Identity;
            if (identity.Claims.Any())
            {
                var userPhoneNumber = identity.Claims.ToList()[0].Value;
                var person = await _personsRepository.GetPersonByPhoneNumber(userPhoneNumber);
                role = (PersonTypes)person.Role;
                ViewBag.UserName = person.Fio;
            }
            if (role == PersonTypes.None)
                return RedirectToAction("Login", "Person");
            ViewBag.Role = role;
            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Страница контактов.";

            return View();
        }
    }
}