using AntiTruble.Person.Enums;
using System;

namespace AntiTruble.Person.Core
{
    public interface IPersonsRepository
    {
        bool Registration(string fio, string password, string phoneNumber, string address, byte role = (byte)PersonTypes.Client, DateTime? dateBirth = null, decimal? balance = default(decimal));
        bool Authorize(string phoneNumber, string password);
    }
}
