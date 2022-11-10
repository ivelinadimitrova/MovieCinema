using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.CinemaTicketsMediatR;
using MovieCinema.Models.Responses.CinemaTicketsResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.CinemaTicketsCommandHandler
{
    public class GetAllCinemaTicketsCommandHandler : IRequestHandler<GetAllCinemaTicketsCommand, GetAllCinemaTicketsResponse>
    {
        private readonly ICinemaTicketRepository _cinemaTicketRepository;
        private ILogger<GetAllCinemaTicketsCommandHandler> _logger;

        public GetAllCinemaTicketsCommandHandler(ICinemaTicketRepository cinemaTicketRepository, ILogger<GetAllCinemaTicketsCommandHandler> logger)
        {
            _cinemaTicketRepository = cinemaTicketRepository;
            _logger = logger;
        }

        public async Task<GetAllCinemaTicketsResponse> Handle(GetAllCinemaTicketsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tickets = await _cinemaTicketRepository.GetAllCinemaTickets();

                if (tickets == null)
                {
                    return new GetAllCinemaTicketsResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The cinema ticket collection is empty.",
                        CinemaTickets = null
                    };
                }

                return new GetAllCinemaTicketsResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received the cinema collection.",
                    CinemaTickets = tickets
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to receive the cinema collection. Error message: {e.Message}");
            }

            return new GetAllCinemaTicketsResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the cinema collection.",
                CinemaTickets = null
            };
        }
    }
}
