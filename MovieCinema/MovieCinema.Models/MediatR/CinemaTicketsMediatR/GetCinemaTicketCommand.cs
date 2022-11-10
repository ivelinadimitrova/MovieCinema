using MediatR;
using MovieCinema.Models.Responses.CinemaTicketsResponses;

namespace MovieCinema.Models.MediatR.CinemaTicketsMediatR
{
    public record GetCinemaTicketCommand(Guid id) : IRequest<GetCinemaTicketResponse>
    {
    }
}