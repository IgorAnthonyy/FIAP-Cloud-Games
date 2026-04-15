using FCG.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IAcessService
    {
        Task<string> Login(AcessLogin login);
    }
}
