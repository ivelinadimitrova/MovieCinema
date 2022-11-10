using MediatR;
using MovieCinema.Models.Requests.DirectorRequests;
using MovieCinema.Models.Responses.DirectorResponses;

namespace MovieCinema.Models.MediatR.DirectorMediatR
{
    public record UpdateDirectorCommand(UpdateDirectorRequest director) : IRequest<UpdateDirectorResponse>
    {
    }
}