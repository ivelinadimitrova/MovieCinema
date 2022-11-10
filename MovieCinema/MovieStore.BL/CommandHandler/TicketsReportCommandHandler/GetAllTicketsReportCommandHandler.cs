using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.TicketsReportMediatR;
using MovieCinema.Models.Responses.TicketsReportResponse;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.TicketsReportCommandHandler
{
    public class GetAllTicketsReportCommandHandler : IRequestHandler<GetAllTicketsReportCommand, GetAllTickersReportResponse>
    {
        private readonly ITicketsReportRepository _ticketsReportRepository;
        private ILogger<GetAllTicketsReportCommandHandler> _logger;

        public GetAllTicketsReportCommandHandler(ITicketsReportRepository ticketsReportRepository, ILogger<GetAllTicketsReportCommandHandler> logger)
        {
            _ticketsReportRepository = ticketsReportRepository;
            _logger = logger;
        }

        public async Task<GetAllTickersReportResponse> Handle(GetAllTicketsReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tickets = await _ticketsReportRepository.GetAllTickets();

                if (tickets == null)
                {
                    return new GetAllTickersReportResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The tickets collection is empty.",
                        TicketsReport = null
                    };
                }

                return new GetAllTickersReportResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You successfully received the tickets collection.",
                    TicketsReport = tickets
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning("Unable to receive the tickets collection.");
            }

            return new GetAllTickersReportResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to receive the tickets collection.",
                TicketsReport = null
            };
        }
    }
}