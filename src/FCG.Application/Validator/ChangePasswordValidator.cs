using FCG.Application.DTOs;
using FluentValidation;

namespace FCG.Application.Validator;

public class ChangePasswordValidator : AbstractValidator<RequestChangePassword>
{
    public ChangePasswordValidator()
    {
        RuleFor(u => u.NewPassword)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.")
            .Matches("^(?=.*[^A-Za-z0-9])(?=.*[A-Za-z])(?=.*[0-9]).+$")
            .WithMessage("Senha deve conter ao menos uma letra, um número e um caractere especial.");

    }
}
