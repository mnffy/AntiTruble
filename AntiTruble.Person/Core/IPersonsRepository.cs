using AntiTruble.Person.JsonModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntiTruble.Person.ControllerModels;

namespace AntiTruble.Person.Core
{
    public interface IPersonsRepository
    {
        Task Registration(PersonModel person);
        Task UpdateUserData(PersonParam person);
        Task RemoveUser(long personId);
        Task<bool> Authorize(string phoneNumber, string password);
        Task<long> GetPersonIdByFIO(string fio);
        Task<long> GetPersonIdByPhoneNumber(string phoneNumber);

        Task<PersonModel> GetPersonByPhoneNumber(string phoneNumber);
        Task<PersonModel> GetPersonById(long id);
        Task<IEnumerable<PersonModel>> GetPersons();
    }
}
