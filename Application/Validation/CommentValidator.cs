using Domain.EFEntities;
using FluentValidation;

namespace Application.Validation
{
    public class CommentValidator: AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
