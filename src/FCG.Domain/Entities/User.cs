using FCG.Domain.Contants;
using FCG.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCG.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }

    public Email Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public DateTime BirthDate { get; set; }

    public CPF Cpf { get; set; }

    public bool Situation { get; set; }

    public ICollection<Role> Roles { get; set; } = [];

    public void CreateUser(User user, Role role)
    {
        base.CreateBaseEntity();

        Name = user.Name;
        Email = user.Email;
        Password = user.Password;
        Phone = user.Phone;
        BirthDate = user.BirthDate;
        Cpf = user.Cpf;
        Situation = true;

        AddRole(role);
    }

    public void AddRole(Role role)
    {
        role.CreateBaseEntity();

        role.UserId = Id;
        Roles.Add(role);
    }

    public bool IsAdmin()
    {
        return Roles.Any(r => r.Name == FCGConstant.AdminRole);
    }
}
