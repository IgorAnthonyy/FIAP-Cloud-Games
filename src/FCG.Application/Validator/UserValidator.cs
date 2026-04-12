using FCG.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Application.Validator
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).MinimumLength(8).Matches("^(?=.*[^A-Za-z0-9])(?=.*[A-Za-z])(?=.*[0-9]).+$");
        }
    }
}
