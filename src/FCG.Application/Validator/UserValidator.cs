using FCG.Application.DTOs;
using FluentValidation;

namespace FCG.Application.Validator;

public class UserValidator : AbstractValidator<UserCreate>
{
    public UserValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.")
            .Matches("^(?=.*[^A-Za-z0-9])(?=.*[A-Za-z])(?=.*[0-9]).+$")
            .WithMessage("Senha deve conter ao menos uma letra, um número e um caractere especial.");

        RuleFor(u => u.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório.")
            .Must(BeValidCpf).WithMessage("CPF inválido.");
    }

    private static bool BeValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return false;
        }

        var numbersOnly = cpf.Trim().Replace("-", string.Empty).Replace(".", string.Empty);
        if (numbersOnly.Length != 11 || !long.TryParse(numbersOnly, out _))
        {
            return false;
        }

        var allEqual = true;
        for (var i = 1; i < numbersOnly.Length; i++)
        {
            if (numbersOnly[i] != numbersOnly[0])
            {
                allEqual = false;
                break;
            }
        }

        if (allEqual)
        {
            return false;
        }

        int CalcDigit(int length)
        {
            var sum = 0;
            var factor = length + 1;

            for (var i = 0; i < length; i++)
            {
                sum += (numbersOnly[i] - '0') * factor;
                factor--;
            }

            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        var firstDigit = CalcDigit(9);
        var secondDigit = CalcDigit(10);

        return numbersOnly[9] - '0' == firstDigit && numbersOnly[10] - '0' == secondDigit;
    }
}
