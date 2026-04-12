using FCG.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Domain.Services
{
    public class BaseDomainService(IUnitOfWork unitOfWork)
    {
        protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    }
}
