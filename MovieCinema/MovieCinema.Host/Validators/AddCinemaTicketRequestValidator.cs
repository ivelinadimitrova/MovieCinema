using FluentValidation;
using MovieCinema.Models.Requests.CinemaTicketsRequests;

namespace MovieCinema.Host.Validators
{
    public class AddCinemaTicketRequestValidator : AbstractValidator<MakeCinemaTicketRequest>
    {
        public AddCinemaTicketRequestValidator()
        {
            RuleForEach(ct => ct.Movies).ChildRules(movies =>
            {
                movies.RuleFor(m => m.MovieName)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(100);

                movies.RuleFor(m => m.ReleaseDate)
                    .GreaterThan(DateTime.MinValue)
                    .LessThan(DateTime.MaxValue);

                movies.RuleFor(m => m.Price)
                    .GreaterThan(0);

                movies.RuleFor(m => m.DirectorId)
                    .GreaterThan(0);
            });

            RuleFor(ct => ct.VisitorId)
                .GreaterThan(0);
        }
    }
}