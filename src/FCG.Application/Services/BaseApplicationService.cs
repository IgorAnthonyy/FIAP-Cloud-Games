using FCG.Application.Interfaces;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services;

public class BaseApplicationService(IUnitOfWork unitOfWork)
{
    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
}
