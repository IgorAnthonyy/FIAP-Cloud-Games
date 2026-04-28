using CommonTestUtilities.Authentication;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Password;
using CommonTestUtilities.Repositories;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Services;

namespace CommonTestUtilities.Services;

public static class UserServiceBuilder
{
    public static (UserService userService, UserDomainService userDomainService) CreateUseCase(User user, bool isAdmin = false, string? password = null, User? targetUser = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var emailService = EmailServiceBuilder.Build(user);
        var passwordEncripter = new PasswordServiceBuilder().VerifyPassword(password).Build();
        var userRepositoryMock = new UserRepositoryBuild().GetByEmail(user).GetById(targetUser ?? user).Build();
        var loggedUser = isAdmin ? UserLoggedBuilder.BuildAdmin(user) : UserLoggedBuilder.Build(user);


        var userDomainService = new UserDomainService(userRepositoryMock, passwordEncripter);
        var userService = new UserService(unitOfWork, mapper, emailService, userDomainService, loggedUser, passwordEncripter);

        return (userService, userDomainService);
    }
}