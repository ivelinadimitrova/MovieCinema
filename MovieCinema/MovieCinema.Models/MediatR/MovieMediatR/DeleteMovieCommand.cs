using MediatR;
using MovieCinema.Models.Responses.MovieResponses;

namespace MovieCinema.Models.MediatR.MovieMediatR
{
    public record DeleteMovieCommand(int id) : IRequest<ReceiveMovieInformationResponse> 
    {
    }
}