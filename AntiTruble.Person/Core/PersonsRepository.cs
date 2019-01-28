using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntiTruble.ClassLibrary.Enums;
using AntiTruble.Person.Extentions;
using AntiTruble.Person.JsonModels;
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
            return SecurePasswordHasher.ValidatePassword(password, person.Password);
        }

        public async Task<long> GetPersonIdByFIO(string fio)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Fio.Equals(fio));
            if (person == null)
                throw new Exception("Person not found");
            return person.PersonId;
        }
        public async Task<PersonModel> GetPersonByPhoneNumber(string phoneNumber)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.PhoneNumber.Equals(phoneNumber));
            if (person == null)
                throw new Exception("Person not found");
            return new PersonModel
            {
                Address = person.Address,
                PhoneNumber = person.PhoneNumber,
                Balance = person.Balance,
                Fio = person.Fio,
                Password = person.Password,
                Role = person.Role
            };
        }
        public async Task<long> GetPersonIdByPhoneNumber(string phoneNumber)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.PhoneNumber.Equals(phoneNumber));
            if (person == null)
                throw new Exception("Person not found");
            return person.PersonId;
        }
        public async Task<PersonModel> GetPersonById(long id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.PersonId == id);
            if (person == null)
                throw new Exception("Person not found");
            return new PersonModel
            {
                Address = person.Address,
                Balance = person.Balance,
                Fio = person.Fio,
                Password = person.Password,
                PhoneNumber = person.PhoneNumber,
                Role = person.Role
            };
        }

        public async Task Registration(PersonModel model)
        {
            var person = new Persons
            {
                Fio = model.Fio,
                Password = SecurePasswordHasher.HashPassword(model.Password),
                Address = model.Address,
                Balance = model.Balance ?? default(decimal),
                PhoneNumber = model.PhoneNumber,
                Role = model.Role ?? (byte)PersonTypes.Client
            };
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonModel>> GetPersons() =>
            await _context.Persons.Select(x =>
            new PersonModel
            {
                Role = x.Role,
                Address = x.Address,
                Balance = x.Balance,
                Fio = x.Fio,
                Password = x.Password,
                PhoneNumber = x.PhoneNumber
            }).ToListAsync();
    }
}
