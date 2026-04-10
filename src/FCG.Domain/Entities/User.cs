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

    // Relacionamento
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
