using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.DirectorMediatR;
using MovieCinema.Models.Responses.DirectorResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.DirectorCommandHandlers
{
    public class GetDirectorByIdCommandHandler : IRequestHandler<GetDirectorByIdCommand, ReceiveDirectorInformationResponse>
    {
        private readonly IDirectorRepository _directorRepository;
        private ILogger<GetDirectorByIdCommandHandler> _logger;

        public GetDirectorByIdCommandHandler(IDirectorRepository directorRepository, ILogger<GetDirectorByIdCommandHandler> logger)
        {
            _directorRepository = directorRepository;
            _logger = logger;
        }

        public async Task<ReceiveDirectorInformationResponse> Handle(GetDirectorByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var director = await _directorRepository.GetDirectorById(request.id);

                if (director == null)
                    return new ReceiveDirectorInformationResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "The director doesn't exists.",
                        Director = null
                    };

                return new ReceiveDirectorInformationResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received the director.",
                    Director = director
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to receive the director. Error message: {e.Message}");
            }

            return new ReceiveDirectorInformationResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the director.",
                Director = null
            };
        }
    }
}