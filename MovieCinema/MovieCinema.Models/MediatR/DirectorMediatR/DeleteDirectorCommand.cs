using MediatR;
using MovieCinema.Models.Responses.DirectorResponses;

namespace MovieCinema.Models.MediatR.DirectorMediatR
{
    public record DeleteDirectorCommand(int id) : IRequest<ReceiveDirectorInformationResponse>
    {
    }
}