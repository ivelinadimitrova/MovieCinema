using MediatR;
using MovieCinema.Models.Responses.DirectorResponses;

namespace MovieCinema.Models.MediatR.DirectorMediatR
{
    public record GetAllDirectorsCommand : IRequest<GetAllDirectorsResponse>
    {
    }
}