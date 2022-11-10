using FluentValidation;
using MovieCinema.Models.Requests.CinemaTicketsRequests;

namespace MovieCinema.Host.Validators
{
    public class AddCinemaTicketRequestValidator : AbstractValidator<MakeCinemaTicketRequest>
    {
        public AddCinemaTicketRequestValidator()
        {
            RuleFor(ct => ct.Movies)
                .NotEmpty();

            RuleFor(ct => ct.VisitorId)
                .GreaterThan(0);
        }
    }
}