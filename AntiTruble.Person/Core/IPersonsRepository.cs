using AntiTruble.Person.Enums;
using AntiTruble.Repairs.Models;
using System;

namespace AntiTruble.Person.Core
{
    public interface IPersonsRepository
    {
        bool Registration(string fio, string password, string phoneNumber, string address, byte role = (byte)PersonTypes.Client, DateTime? dateBirth = null, decimal? balance = default(decimal));
        bool Authorize(string phoneNumber, string password);
        Persons GetPersonByFIO(string fio);
        Persons GetPersonById(string fio);
    }
}
