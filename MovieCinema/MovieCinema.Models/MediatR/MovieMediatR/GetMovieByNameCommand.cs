using MediatR;
using MovieCinema.Models.Responses.MovieResponses;

namespace MovieCinema.Models.MediatR.MovieMediatR
{
    public record GetMovieByNameCommand(string name) : IRequest<ReceiveMovieInformationResponse>
    {
    }
}