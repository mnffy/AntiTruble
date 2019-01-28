using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntiTruble.Equipment.Core;
using Microsoft.AspNetCore.Mvc;

namespace AntiTruble.Person.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentRepository _equipmentRepository;
        public EquipmentController(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }
        public IActionResult Index()
        {
            var a = _equipmentRepository.SearchEquipments(1);
            return View();
        }
    }
}