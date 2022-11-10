using FluentValidation;
using MovieCinema.Models.Requests.DirectorRequests;

namespace MovieCinema.Host.Validators
{
    public class AddDirectorRequestValidator : AbstractValidator<AddDirectorRequest>
    {
        public AddDirectorRequestValidator()
        {
            RuleFor(d => d.Age)
            .GreaterThan(0)
            .WithMessage("The age should be greater than 0.")
            .LessThanOrEqualTo(180)
            .WithMessage("The age should be less than 180.");

            RuleFor(d => d.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);

            When(d => !string.IsNullOrEmpty(d.NickName), () =>
            {
                RuleFor(d => d.NickName)
                    .MinimumLength(2)
                    .MaximumLength(100);
            });

            RuleFor(d => d.DateOfBirth)
                .GreaterThan(DateTime.MinValue)
                .LessThan(DateTime.MaxValue);
        }
    }
}