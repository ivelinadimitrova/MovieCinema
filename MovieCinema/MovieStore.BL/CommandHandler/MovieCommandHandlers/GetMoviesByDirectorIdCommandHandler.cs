using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Responses.MovieResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.MovieCommandHandlers
{
    public class GetMoviesByDirectorIdCommandHandler : IRequestHandler<GetMoviesByDirectorIdCommand, GetMoviesByDirectorIdResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<GetMoviesByDirectorIdCommandHandler> _logger;

        public GetMoviesByDirectorIdCommandHandler(IMovieRepository movieRepository, ILogger<GetMoviesByDirectorIdCommandHandler> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<GetMoviesByDirectorIdResponse> Handle(GetMoviesByDirectorIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.id <= 0)
                {
                    return new GetMoviesByDirectorIdResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Id must be greater than 0.",
                        MovieCollection = null
                    };
                };

                var movies = await _movieRepository.GetMoviesByDirectorId(request.id);

                if (movies == Enumerable.Empty<Movie>())
                    return new GetMoviesByDirectorIdResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "Director with this Id doesn't exists.",
                        MovieCollection = null
                    };

                return new GetMoviesByDirectorIdResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received the collection.",
                    MovieCollection = movies
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to receive the movie. Error message: {e.Message}");
            }

            return new GetMoviesByDirectorIdResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the movie.",
                MovieCollection = null
            };
        }
    }
}
