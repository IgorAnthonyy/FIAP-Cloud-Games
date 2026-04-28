using FCG.Domain.Interfaces.Respositories;

namespace FCG.Application.Services;

public class BaseApplicationService(IUnitOfWork unitOfWork)
{
    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
}
