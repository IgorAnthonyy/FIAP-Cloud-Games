using Bogus;
using Bogus.Extensions.Brazil;
using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Fixture
{
    public class UserFixture
    {
        private readonly Faker _faker;

        public UserFixture()
        {
            _faker = new Faker();
        }

        public User GenerateUserWithRoleEmpty()
        {
            string name = _faker.Name.FullName();
            string email = _faker.Internet.Email();
            string password = _faker.Internet.Password();
            string phone = _faker.Phone.PhoneNumber();
            string cpf = _faker.Person.Cpf();
            DateTime birthDate = _faker.Date.Past(50, DateTime.Now.AddYears(-20));
            bool situation = _faker.Random.Bool();

            return new User { Name = name, Email = email, Password = password, Phone = phone, BirthDate = birthDate, Situation = situation };
        }
    }
}
