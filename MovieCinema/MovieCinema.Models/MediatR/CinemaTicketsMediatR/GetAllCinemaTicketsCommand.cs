
using MediatR;
using MovieCinema.Models.Responses.CinemaTicketsResponses;

namespace MovieCinema.Models.MediatR.CinemaTicketsMediatR
{
    public record GetAllCinemaTicketsCommand : IRequest<GetAllCinemaTicketsResponse>
    {
    }
}