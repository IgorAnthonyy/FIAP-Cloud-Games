using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Respositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests
{
    public class AdminContext
    {
        public User UserAdmin { get; set; } = null!;
        public Mock<IUserLogged> UserLogged = new();
        public Mock<IUserRepository> UserRepo = new();
    }
}
