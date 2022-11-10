using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Responses.MovieResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.MovieCommandHandlers
{
    public class AddMultipleMoviesCommandHandler : IRequestHandler<AddMultipleMoviesCommand, AddMultipleMoviesResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddMultipleMoviesCommandHandler> _logger;

        public AddMultipleMoviesCommandHandler(IMovieRepository movieRepository, ILogger<AddMultipleMoviesCommandHandler> logger, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AddMultipleMoviesResponse> Handle(AddMultipleMoviesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movies = await _movieRepository.AddMultipleMovies(request.movies.MovieCollection);

                if (movies == false)
                    return new AddMultipleMoviesResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "The collection is empty.",
                        Result = false
                    };

                var movieMapper = _mapper.Map<IEnumerable<Movie>>(request.movies.MovieCollection);
                var result = await _movieRepository.AddMultipleMovies(movieMapper);

                return new AddMultipleMoviesResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully added the collection.",
                    Result = result
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to add the collection. Error message: {e.Message}");
            }

            return new AddMultipleMoviesResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to add the collection.",
                Result = false
            };
        }
    }
}