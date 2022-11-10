using MediatR;
using MovieCinema.Models.Requests.MoviesRequests;
using MovieCinema.Models.Responses.MovieResponses;

namespace MovieCinema.Models.MediatR.MovieMediatR
{
    public record AddMultipleMoviesCommand(AddMultipleMoviesRequest movies) : IRequest<AddMultipleMoviesResponse>
    {
    }
}