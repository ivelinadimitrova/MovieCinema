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
    public class AddDirectorCommandHandler : IRequestHandler<AddDirectorCommand, AddDirectorResponse>
    {
        private readonly IDirectorRepository _directorRepository;
        private ILogger<AddDirectorCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddDirectorCommandHandler(IDirectorRepository directorRepository, ILogger<AddDirectorCommandHandler> logger, IMapper mapper)
        {
            _directorRepository = directorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AddDirectorResponse> Handle(AddDirectorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var director = await _directorRepository.GetDirectorByName(request.director.Name);

                if (director != null)
                {
                    return new AddDirectorResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Director already exists!",
                    };
                }

                var directorMapper = _mapper.Map<Director>(request.director);
                var result = await _directorRepository.AddDirector(directorMapper);

                return new AddDirectorResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Director successfully added.",
                    Director = result
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Unable to add the director. Error message: {e.Message}");
            }

            return new AddDirectorResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to add the director."
            };
        }
    }
}