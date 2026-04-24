using System;

namespace FCG.Application.DTOs;

public class UserUpdate
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public DateTime BirthDate { get; set; }

    public bool? Situation { get; set; }
}
