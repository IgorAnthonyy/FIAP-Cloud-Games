using System;
using FCG.Domain.Interfaces;

namespace FCG.Domain.Entities;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
}
