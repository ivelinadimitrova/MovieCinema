using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.DirectorMediatR;
using MovieCinema.Models.Responses.DirectorResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.DirectorCommandHandlers
{
    public class DeleteDirectorCommandHandler :IRequestHandler<DeleteDirectorCommand, ReceiveDirectorInformationResponse>
    {
        private readonly IDirectorRepository _directorRepository;
        private ILogger<DeleteDirectorCommandHandler> _logger;

        public DeleteDirectorCommandHandler(IDirectorRepository directorRepository, ILogger<DeleteDirectorCommandHandler> logger)
        {
            _directorRepository = directorRepository;
            _logger = logger;
        }

        public async Task<ReceiveDirectorInformationResponse> Handle(DeleteDirectorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var director = await _directorRepository.DeleteDirector(request.id);

                if (director == null)
                    return new ReceiveDirectorInformationResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The director doesn't exists."
                    };

                return new ReceiveDirectorInformationResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully deleted the director.",
                    Director = director
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to delete the director. Error message: {e.Message}");
            }

            return new ReceiveDirectorInformationResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to delete the director.",
            };
        }
    }
}