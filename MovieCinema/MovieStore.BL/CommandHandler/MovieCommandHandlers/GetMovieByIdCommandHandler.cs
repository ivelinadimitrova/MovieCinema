using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Responses.MovieResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.MovieCommandHandlers
{
    public class GetMovieByIdCommandHandler : IRequestHandler<GetMovieByIdCommand, ReceiveMovieInformationResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<GetMovieByIdCommandHandler> _logger;

        public GetMovieByIdCommandHandler(IMovieRepository movieRepository, ILogger<GetMovieByIdCommandHandler> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<ReceiveMovieInformationResponse> Handle(GetMovieByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.id <= 0)
                {
                    return new ReceiveMovieInformationResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Id must be greater than 0.",
                        Movie = null
                    };
                };

                var movie = await _movieRepository.GetMovieById(request.id);

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
