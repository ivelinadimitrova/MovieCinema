using MediatR;
using MovieCinema.Models.Requests.MoviesRequests;
using MovieCinema.Models.Responses.MovieResponses;

namespace MovieCinema.Models.MediatR.MovieMediatR
{
    public record UpdateMovieCommand(UpdateMovieRequest movie) : IRequest<UpdateMovieResponse>
    {
    }
}