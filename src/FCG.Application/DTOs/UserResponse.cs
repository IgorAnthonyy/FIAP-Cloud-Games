using System;

namespace FCG.Application.DTOs;

public class UserResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public string Cpf { get; set; }
}
