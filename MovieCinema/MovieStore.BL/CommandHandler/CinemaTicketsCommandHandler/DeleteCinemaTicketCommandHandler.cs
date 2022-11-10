using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.CinemaTicketsMediatR;
using MovieCinema.Models.Responses.CinemaTicketsResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.CinemaTicketsCommandHandler
{
    public class DeleteCinemaTicketCommandHandler : IRequestHandler<DeleteCinemaTicketCommand, DeleteCinemaTicketResponse>
    {
        private readonly ICinemaTicketRepository _cinemaTicketRepository;
        private ILogger<DeleteCinemaTicketCommandHandler> _logger;

        public DeleteCinemaTicketCommandHandler(ICinemaTicketRepository cinemaTicketRepository, ILogger<DeleteCinemaTicketCommandHandler> logger)
        {
            _cinemaTicketRepository = cinemaTicketRepository;
            _logger = logger;
        }

        public async Task<DeleteCinemaTicketResponse> Handle(DeleteCinemaTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _cinemaTicketRepository.DeleteCinemaTicket(request.id);

                if (result == null)
                {
                    return new DeleteCinemaTicketResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The cinema ticket with this Id doesn't exists.",
                        Id = request.id
                    };
                }

                return new DeleteCinemaTicketResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Cinema ticket successfully deleted.",
                    Id = result
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Unable to add the cinema ticket. Error message: {e.Message}");
            }

            return new DeleteCinemaTicketResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to add the cinema ticket.",
                Id = null
            };
        }
    }
}