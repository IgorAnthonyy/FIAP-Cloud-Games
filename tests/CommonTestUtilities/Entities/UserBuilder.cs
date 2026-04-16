using Bogus;
using Bogus.Extensions.Brazil;
using CommonTestUtilities.Password;
using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build()
    {
        var passwordEncripter = new PasswordServiceBuilder().Build();

        var user = new Faker<User>("pt_BR")
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => new Email(faker.Internet.Email(user.Name)))
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.GenerateHash(user.Password))
            .RuleFor(u => u.Phone, faker => faker.Person.Phone)
            .RuleFor(u => u.BirthDate, faker => faker.Person.DateOfBirth)
            .RuleFor(u => u.Cpf, faker => new CPF(faker.Person.Cpf()));

        return user;
    }
}
