using MediatR;
using MovieCinema.Models.Requests.CinemaTicketsRequests;
using MovieCinema.Models.Responses.CinemaTicketsResponses;

namespace MovieCinema.Models.MediatR.CinemaTicketsMediatR
{
    public record MakeCinemaTicketCommand(MakeCinemaTicketRequest ticket) :IRequest<GetCinemaTicketResponse>
    {
    }
}