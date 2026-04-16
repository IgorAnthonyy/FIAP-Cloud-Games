using Bogus;
using FCG.Application.DTOs;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordBuilder
{
    public static RequestChangePassword Build()
    {
        return new Faker<RequestChangePassword>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password())
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}