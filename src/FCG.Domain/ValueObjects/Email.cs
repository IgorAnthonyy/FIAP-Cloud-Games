using FCG.Domain.Shared;
using System.Text.RegularExpressions;

namespace FCG.Domain.ValueObjects;

public class Email
{
    public string Value { get; set; }
    public Email(string value)
    {
        AssertionConcern.AssertNotEmpty(value, "Email obrigatório");
        AssertionConcern.AssertTrue(IsValid(value), "Email inválido");
        Value = value;
    }
    private bool IsValid(string email)
    {
        bool valid = Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        return valid;
    }

}