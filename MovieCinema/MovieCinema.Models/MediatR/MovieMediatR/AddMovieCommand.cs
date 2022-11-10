using MediatR;
using MovieCinema.Models.Requests.MoviesRequests;
using MovieCinema.Models.Responses.MovieResponses;

namespace MovieCinema.Models.MediatR.MovieMediatR
{
    public record AddMovieCommand(AddMovieRequest addMovieRequest) : IRequest<AddMovieResponse>
    {
    }
}