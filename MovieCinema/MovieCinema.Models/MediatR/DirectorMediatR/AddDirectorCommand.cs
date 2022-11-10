using MediatR;
using MovieCinema.Models.Requests.DirectorRequests;
using MovieCinema.Models.Responses.DirectorResponses;

namespace MovieCinema.Models.MediatR.DirectorMediatR
{
    public record AddDirectorCommand(AddDirectorRequest director) : IRequest<AddDirectorResponse>
    {
    }
}