using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Respositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserRepositoryBuild
{
    private readonly Mock<IUserRepository> _repository;
    
    public UserRepositoryBuild()
    {
        _repository = new Mock<IUserRepository>();
    }
    public UserRepositoryBuild GetById(User user)
    {
        _repository.Setup(u => u.GetById(It.IsAny<Guid>())).ReturnsAsync(user);

        return this;
    }

    public UserRepositoryBuild GetByEmail(User user)
    {
        _repository.Setup(u => u.GetByEmail(It.IsAny<string>())).ReturnsAsync(user);

        return this;
    }

    public UserRepositoryBuild GetAll(User user)
    {
        _repository.Setup(u => u.GetAll()).ReturnsAsync([user]);

        return this;
    }

    public IUserRepository Build() => _repository.Object;
}
