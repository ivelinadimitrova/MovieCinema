using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Responses.MovieResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.MovieCommandHandlers
{
    public class GetAllMoviesCommandHandler : IRequestHandler<GetAllMoviesCommand, GetAllMoviesResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<GetAllMoviesCommandHandler> _logger;

        public GetAllMoviesCommandHandler(IMovieRepository movieRepository, ILogger<GetAllMoviesCommandHandler> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<GetAllMoviesResponse> Handle(GetAllMoviesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movies = await _movieRepository.GetAllMovies();

                if (movies == Enumerable.Empty<Movie>())
                    return new GetAllMoviesResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "No available movies.",
                        Movies = null
                    };

                return new GetAllMoviesResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received the movies collection.",
                    Movies = movies
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to receive the movies collection. Error message: {e.Message}");
            }

            return new GetAllMoviesResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the movies collection.",
                Movies = null
            };
        }
    }
}
