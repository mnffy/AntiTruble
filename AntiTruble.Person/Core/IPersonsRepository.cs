using AntiTruble.Person.Enums;
using AntiTruble.Person.JsonModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiTruble.Person.Core
{
    public interface IPersonsRepository
    {
        Task Registration(PersonModel person);
        Task<bool> Authorize(string phoneNumber, string password);
        Task<long> GetPersonIdByFIO(string fio);
        Task<PersonModel> GetPersonById(long id);
        Task<IEnumerable<PersonModel>> GetPersons();
    }
}
