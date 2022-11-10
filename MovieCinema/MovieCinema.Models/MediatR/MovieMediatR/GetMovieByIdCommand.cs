using MediatR;
using MovieCinema.Models.Responses.MovieResponses;

namespace MovieCinema.Models.MediatR.MovieMediatR
{
    public record GetMovieByIdCommand(int id) : IRequest<ReceiveMovieInformationResponse>
    {
    }
}