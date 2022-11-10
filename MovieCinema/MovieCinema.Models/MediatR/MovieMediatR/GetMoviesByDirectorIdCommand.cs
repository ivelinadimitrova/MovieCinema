using MediatR;
using MovieCinema.Models.Responses.MovieResponses;

namespace MovieCinema.Models.MediatR.MovieMediatR
{
    public record GetMoviesByDirectorIdCommand(int id) : IRequest<GetMoviesByDirectorIdResponse>
    {
    }
}