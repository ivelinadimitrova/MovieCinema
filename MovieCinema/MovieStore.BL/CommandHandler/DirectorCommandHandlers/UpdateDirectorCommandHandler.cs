using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.DirectorMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Responses.DirectorResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.DirectorCommandHandlers
{
    public class UpdateDirectorCommandHandler : IRequestHandler<UpdateDirectorCommand, UpdateDirectorResponse>
    {
        private readonly IDirectorRepository _directorRepository;
        private ILogger<UpdateDirectorCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateDirectorCommandHandler(IDirectorRepository directorRepository, ILogger<UpdateDirectorCommandHandler> logger, IMapper mapper)
        {
            _directorRepository = directorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UpdateDirectorResponse> Handle(UpdateDirectorCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var director = await _directorRepository.GetDirectorById(request.director.Id);

                if (director == null)
                    return new UpdateDirectorResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "Director doesn't exists!",
                        Director = null
                    };

                var directorMapper = _mapper.Map<Director>(request.director);
                var result = await _directorRepository.UpdateDirector(directorMapper);

                return new UpdateDirectorResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Director successfully updated.",
                    Director = result
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to update the director! Error message: {e.Message}");
            }

            return new UpdateDirectorResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to update the director.",
                Director = null
            };
        }
    }
}