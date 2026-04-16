using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface IAccessDomainService
{
    Task<User> Login(Email email, Password password);
}
