using Bogus;
using Bogus.Extensions.Brazil;
using CommonTestUtilities.Password;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public User GenerateUserWithRoleEmpty()
    {
        var user = Build();
        return user;
    }

    public User GenerateUserWithRoleAdmin()
    {
        var user = Build();
        user.Roles = [new Role { Name = FCGConstant.AdminRole }];
        return user;
    }

    public User GenerateUserWithRoles()
    {
        var user = Build();
        user.Roles = [new Role { Name = FCGConstant.AdminRole }, new Role { Name = FCGConstant.UserDefault }];
        return user;
    }

    public User GenerateUserWithRolesDefault()
    {
        var user = Build();
        user.Roles = [new Role { Name = FCGConstant.UserDefault }];
        return user;
    }

    public User GenerateUserLogin(string hashPassword)
    {
        var user = Build();
        user.Password = hashPassword;
        user.Roles = [new Role { Name = FCGConstant.UserDefault }];

        return user;
    }

    private User Build()
    {
        var passwordEncripter = new PasswordServiceBuilder().Build();

        var user = new Faker<User>("pt_BR")
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => new Email(faker.Internet.Email(user.Name)))
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.GenerateHash(user.Password))
            .RuleFor(u => u.Phone, faker => faker.Person.Phone)
            .RuleFor(u => u.BirthDate, faker => faker.Person.DateOfBirth)
            .RuleFor(u => u.Cpf, faker => new CPF(faker.Person.Cpf()))
            .RuleFor(u => u.Situation, faker => faker.Random.Bool());

        return user;
    }
}
