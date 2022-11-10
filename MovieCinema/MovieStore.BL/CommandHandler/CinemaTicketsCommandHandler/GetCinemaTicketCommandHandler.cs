using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.CinemaTicketsMediatR;
using MovieCinema.Models.Responses.CinemaTicketsResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.CinemaTicketsCommandHandler
{
    public class GetCinemaTicketCommandHandler : IRequestHandler<GetCinemaTicketCommand, GetCinemaTicketResponse>
    {
        private readonly ICinemaTicketRepository _cinemaTicketRepository;
        private ILogger<GetCinemaTicketCommandHandler> _logger;

        public GetCinemaTicketCommandHandler(ICinemaTicketRepository cinemaTicketRepository, ILogger<GetCinemaTicketCommandHandler> logger)
        {
            _cinemaTicketRepository = cinemaTicketRepository;
            _logger = logger;
        }

        public async Task<GetCinemaTicketResponse> Handle(GetCinemaTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = await _cinemaTicketRepository.GetCinemaTicket(request.id);

                if (ticket == null)
                {
                    return new GetCinemaTicketResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The cinema ticket with this visitor id doesn't exists.",
                        CinemaTicket = null
                    };
                }

                return new GetCinemaTicketResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received the cinema ticket.",
                    CinemaTicket = ticket
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to receive the cinema ticket. Error message: {e.Message}");
            }

            return new GetCinemaTicketResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the cinema ticket.",
                CinemaTicket = null
            };
        }
    }
}