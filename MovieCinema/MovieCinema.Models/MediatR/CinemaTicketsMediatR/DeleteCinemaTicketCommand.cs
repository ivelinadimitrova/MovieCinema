using MediatR;
using MovieCinema.Models.Responses.CinemaTicketsResponses;

namespace MovieCinema.Models.MediatR.CinemaTicketsMediatR
{
    public record DeleteCinemaTicketCommand(Guid id) : IRequest<DeleteCinemaTicketResponse>
    {
    }
}