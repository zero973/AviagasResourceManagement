using ARM.Core.Models.Security;
using FluentValidation;

namespace ARM.Core.Validators.Security;

public class TokensPairValidator : AbstractValidator<TokensPair>
{
    public TokensPairValidator()
    {
        RuleFor(x => x.AccessToken).MinimumLength(1)
            .WithMessage("AccessToken должен быть больше 1 символа");
        
        RuleFor(x => x.RefreshToken).MinimumLength(1)
            .WithMessage("RefreshToken должен быть больше 1 символа");
    }
}