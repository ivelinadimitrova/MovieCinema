using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Responses.MovieResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.MovieCommandHandlers
{
    public class GetMovieByNameCommandHandler : IRequestHandler<GetMovieByNameCommand, ReceiveMovieInformationResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<GetMovieByNameCommandHandler> _logger;

        public GetMovieByNameCommandHandler(IMovieRepository movieRepository, ILogger<GetMovieByNameCommandHandler> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<ReceiveMovieInformationResponse> Handle(GetMovieByNameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _movieRepository.GetMovieByName(request.name);

                if (movie == null)
                    return new ReceiveMovieInformationResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The movie doesn't exists",
                        Movie = null
                    };

                return new ReceiveMovieInformationResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received the movie.",
                    Movie = movie
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to receive the movie. Error message: {e.Message}");
            }

            return new ReceiveMovieInformationResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the movie.",
            };
        }
    }
}
