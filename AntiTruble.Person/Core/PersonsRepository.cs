using System;
using AntiTruble.Person.Models;

namespace AntiTruble.Person.Core
{
    public class PersonsRepository : IPersonsRepository
    {
        public bool Authorize(string phoneNumber, string password)
        {
            throw new NotImplementedException();
        }

        public Persons GetPersonByFIO(string fio)
        {
            throw new NotImplementedException();
        }

        public Persons GetPersonById(string fio)
        {
            throw new NotImplementedException();
        }

        public bool Registration(string fio, string password, string phoneNumber, string address, byte role = 1, DateTime? dateBirth = null, decimal? balance = 0)
        {
            throw new NotImplementedException();
        }
    }
}
