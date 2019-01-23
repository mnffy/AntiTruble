using System.ComponentModel.DataAnnotations;

namespace AntiTruble.Person.Models
{
    public partial class Persons
    {
        [Key]
        public long PersonId { get; set; }
        public string Fio { get; set; }
        public string Password { get; set; }
        public byte? Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal? Balance { get; set; }
    }
}
