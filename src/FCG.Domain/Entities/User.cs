using System;
using System.Collections.Generic;

namespace FCG.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public DateTime BirthDate { get; set; }

    public string CpfNumber { get; set; }

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
        CpfNumber = user.CpfNumber;
        Situation = true;

        AddRole(role);
    }

    public void AddRole(Role role)
    {
        role.CreateBaseEntity();

        role.UserId = Id;
        Roles.Add(role);
    }
}
