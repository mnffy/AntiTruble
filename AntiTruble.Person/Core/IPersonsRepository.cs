using AntiTruble.Person.Enums;
using AntiTruble.Person.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Person.Core
{
    public interface IPersonsRepository
    {
        Task Registration(string fio, string password, string phoneNumber, string address, byte role = (byte)PersonTypes.Client, DateTime? dateBirth = null, decimal? balance = default(decimal));
        Task<bool> Authorize(string phoneNumber, string password);
        Task<long> GetPersonIdByFIO(string fio);
        Task<Persons> GetPersonById(long id);
        Task<IEnumerable<Persons>> GetPersons();
    }
}
