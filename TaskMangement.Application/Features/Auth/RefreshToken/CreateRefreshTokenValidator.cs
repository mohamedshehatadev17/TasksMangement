using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TaskMangement.Application.Features.Auth.RefreshToken
{
    internal class CreateRefreshTokenValidator : FluentValidation.AbstractValidator<CreateRefreshTokenCommand>
    {
        public CreateRefreshTokenValidator()
        {
            RuleFor(x => x.refreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.");
        }
    }
}
