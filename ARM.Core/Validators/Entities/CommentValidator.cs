using ARM.Core.Models.Entities;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class CommentValidator : AbstractValidator<Comment>
{
    public CommentValidator()
    {
        RuleFor(x => x.Text).MinimumLength(1)
            .WithMessage("Комментарий не может быть пустым");
    }
}