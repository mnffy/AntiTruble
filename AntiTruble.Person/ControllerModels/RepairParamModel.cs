using System.Collections.Generic;

namespace AntiTruble.Person.ControllerModels
{
    public class RepairParamModel
    {
        public string EquipmentName { get; set; }
        public string EType { get; set; }
        public string Master { get; set; }
        public string Client { get; set; }
        public string RType { get; set; }
        public IEnumerable<DefectStringModel> Defects { get; set; }
        public string Days { get; set; }

        public class DefectStringModel
        {
            public string DefectName { get; set; }
            public string Price { get; set; }
        }
    }
}
