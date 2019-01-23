using System;

namespace AntiTruble.Person.Core
{
    public class PersonsRepository : IPersonsRepository
    {
        public bool Authorize(string phoneNumber, string password)
        {
            throw new NotImplementedException();
        }

        public bool Registration(string fio, string password, string phoneNumber, string address, byte role = 1, DateTime? dateBirth = null, decimal? balance = 0)
        {
            throw new NotImplementedException();
        }
    }
}
