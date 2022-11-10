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
    public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, AddMovieResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<AddMovieCommandHandler> _logger;
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;

        public AddMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper, ILogger<AddMovieCommandHandler> logger, IDirectorRepository directorRepository)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _logger = logger;
            _directorRepository = directorRepository;
        }

        public async Task<AddMovieResponse> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var directorExists = await _directorRepository.GetDirectorById(request.addMovieRequest.DirectorId);

                if (directorExists == null)
                {
                    return new AddMovieResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "You can't add the movie because the director with this Id doesn't!",
                        Movie = null
                    }; ;
                }

                var movie = await _movieRepository.GetMovieByName(request.addMovieRequest.MovieName);

                if (movie != null)
                    return new AddMovieResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Movie Already Exists!",
                        Movie = movie
                    };

                var movieMapper = _mapper.Map<Movie>(request.addMovieRequest);
                var result = await _movieRepository.AddMovie(movieMapper);

                return new AddMovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Movie successfully added.",
                    Movie = result
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to add movie. Error message: {e.Message}");
            }

            return new AddMovieResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to add movie.",
                Movie = null
            };
        }
    }
}