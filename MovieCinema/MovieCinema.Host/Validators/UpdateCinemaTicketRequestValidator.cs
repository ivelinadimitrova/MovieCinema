using FluentValidation;
using MovieCinema.Models.Requests.CinemaTicketsRequests;
using MovieCinema.Models.Requests.MoviesRequests;

namespace MovieCinema.Host.Validators
{
    public class UpdateCinemaTicketRequestValidator : AbstractValidator<UpdateCinemaTicketRequest>
    {
        public UpdateCinemaTicketRequestValidator()
        {
            RuleFor(ct => ct.Movies)
                .NotEmpty();

            RuleFor(ct => ct.VisitorId)
                .GreaterThan(0);
        }
    }
}