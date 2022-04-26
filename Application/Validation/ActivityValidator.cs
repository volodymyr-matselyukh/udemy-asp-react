using Domain;
using FluentValidation;

namespace Application.Validation
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.Description).NotEmpty();
            RuleFor(c => c.Date).NotEmpty();
            RuleFor(c => c.Venue).NotEmpty();
            RuleFor(c => c.Category).NotEmpty();
            RuleFor(c => c.City).NotEmpty();
        }
    }
}