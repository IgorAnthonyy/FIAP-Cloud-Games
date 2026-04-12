using FCG.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FCG.Domain.ValueObjects
{
    public class Password
    {
        public string Value { get; }
        public Password(string value)
        {
            AssertionConcern.AssertNotEmpty(value, "Senha obrigatória");
            AssertionConcern.AssertTrue(value.Length >= 8, "Senha deve ter no mínimo 8 caracteres");
            AssertionConcern.AssertTrue(IsValid(value), "Senha precisa conter um caractere especial, uma letra e um numero");
            Value = value;
        }
        private bool IsValid(string password)
        {
            bool valid = Regex.IsMatch(password, @"^(?=.*[^A-Za-z0-9])(?=.*[A-Za-z])(?=.*[0-9]).+$");
            return valid;
        }
    }
}
