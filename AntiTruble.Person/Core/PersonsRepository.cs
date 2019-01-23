using System;
using System.Threading.Tasks;
using AntiTruble.Person.Extentions;
using AntiTruble.Person.Models;
using Microsoft.EntityFrameworkCore;

namespace AntiTruble.Person.Core
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly AntiTruble_PersonContext _context;
        public PersonsRepository(AntiTruble_PersonContext context)
        {
            _context = context;
        }
        public async Task<bool> Authorize(string phoneNumber, string password)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.PhoneNumber.Equals(phoneNumber));
            if (person == null)
                throw new Exception("Person not found");
            var encryptedPassword = SecurePasswordHasher.Encrypt(password);
            return encryptedPassword.Equals(person.Password);
        }

        public async Task<long> GetPersonIdByFIO(string fio)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Fio.Equals(fio));
            if (person == null)
                throw new Exception("Person not found");
            return person.PersonId;
        }
        
        public async Task<Persons> GetPersonById(long id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.PersonId == id);
            if (person == null)
                throw new Exception("Person not found");
            return person;
        }

        public async Task Registration(string fio, string password, string phoneNumber, string address, byte role = 1, DateTime? dateBirth = null, decimal? balance = 0)
        {
            var person = new Persons
            {
                Fio = fio,
                Password = SecurePasswordHasher.Decrypt(password),
                Address = address,
                Balance = balance,
                PhoneNumber = phoneNumber,
                Role = role
            };
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }
    }
}
