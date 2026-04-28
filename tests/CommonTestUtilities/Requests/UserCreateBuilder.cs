using Bogus;
using Bogus.Extensions.Brazil;
using FCG.Application.DTOs;

namespace CommonTestUtilities.Requests;

public class UserCreateBuilder
{
    public static UserCreate Build()
    {
        return new Faker<UserCreate>("pt_BR")
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(u => u.Password, faker => faker.Internet.Password(12, false, "[A-Z]", "@1Aa"))
            .RuleFor(u => u.Phone, faker => faker.Person.Phone)
            .RuleFor(u => u.BirthDate, faker => faker.Person.DateOfBirth)
            .RuleFor(u => u.Cpf, faker => faker.Person.Cpf());
    }
}
