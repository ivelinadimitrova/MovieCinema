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
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, UpdateMovieResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMovieCommandHandler> _logger;

        public UpdateMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper, ILogger<UpdateMovieCommandHandler> logger)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpdateMovieResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _movieRepository.GetMovieById(request.movie.Id);

                if (movie == null)
                    return new UpdateMovieResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "Movie doesn't exists!",
                        Movie = null
                    };

                var movieMapper = _mapper.Map<Movie>(request.movie);
                var result = await _movieRepository.UpdateMovie(movieMapper);

                return new UpdateMovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Movie successfully updated.",
                    Movie = result
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to update the movie. Error message: {e.Message}");
            }

            return new UpdateMovieResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to update the movie.",
                Movie = null
            };
        }
    }
}
