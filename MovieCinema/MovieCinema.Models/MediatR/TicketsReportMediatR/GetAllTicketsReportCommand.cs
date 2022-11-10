using MediatR;
using MovieCinema.Models.Responses.TicketsReportResponse;

namespace MovieCinema.Models.MediatR.TicketsReportMediatR
{
    public record GetAllTicketsReportCommand : IRequest<GetAllTickersReportResponse>
    {
    }
}