using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.DirectorMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Responses.DirectorResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.DirectorCommandHandlers
{
    public class GetAllDirectorsCommandHandler : IRequestHandler<GetAllDirectorsCommand, GetAllDirectorsResponse>
    {
        private readonly IDirectorRepository _directorRepository;
        private ILogger<GetAllDirectorsCommandHandler> _logger;

        public GetAllDirectorsCommandHandler(IDirectorRepository directorRepository, ILogger<GetAllDirectorsCommandHandler> logger)
        {
            _directorRepository = directorRepository;
            _logger = logger;
        }

        public async Task<GetAllDirectorsResponse> Handle(GetAllDirectorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var directors = await _directorRepository.GetAllDirectors();

                if (directors.Equals(Enumerable.Empty<Director>()))
                    return new GetAllDirectorsResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "The directors collection is empty.",
                        DirectorCollection = null
                    };

                return new GetAllDirectorsResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received all the directors.",
                    DirectorCollection = directors
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Collection doesn't exists. Error message: {e.Message}");
            }

            return new GetAllDirectorsResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the directors.",
                DirectorCollection = null
            };
        }
    }
}
