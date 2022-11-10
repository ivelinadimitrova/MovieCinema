using FluentValidation;
using MovieCinema.Models.Requests.MoviesRequests;

namespace MovieCinema.Host.Validators
{
    public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
    {
        public AddMovieRequestValidator()
        {
            RuleFor(m => m.MovieName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);

            When(m => !string.IsNullOrEmpty(m.Actors), () =>
            {
                RuleFor(m => m.Actors)
                    .MinimumLength(2)
                    .MaximumLength(200);
            });

            RuleFor(m => m.ReleaseDate)
                .GreaterThan(DateTime.MinValue)
                .LessThan(DateTime.MaxValue);

            RuleFor(m => m.Price)
                .GreaterThan(0);

            RuleFor(m => m.DirectorId)
                .GreaterThan(0);
        }
    }
}