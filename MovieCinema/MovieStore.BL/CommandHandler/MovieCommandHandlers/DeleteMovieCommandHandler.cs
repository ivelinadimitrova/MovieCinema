using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Responses.MovieResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.MovieCommandHandlers
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, ReceiveMovieInformationResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<DeleteMovieCommandHandler> _logger;

        public DeleteMovieCommandHandler(IMovieRepository movieRepository, ILogger<DeleteMovieCommandHandler> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<ReceiveMovieInformationResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _movieRepository.GetMovieById(request.id);

                await _movieRepository.DeleteMovie(request.id);

                if (movie == null)
                    return new ReceiveMovieInformationResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The movie with this Id doesn't exists.",
                        Movie = null
                    };

                return new ReceiveMovieInformationResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully deleted the movie.",
                    Movie = movie
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to delete the movie. Error message: {e.Message}");
            }

            return new ReceiveMovieInformationResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to delete the movie.",
                Movie = null
            };
        }
    }
}