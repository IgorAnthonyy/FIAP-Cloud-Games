using System;

namespace FCG.Domain.Interfaces;
public interface IBaseEntity
{
    Guid Id { get; set; }
}