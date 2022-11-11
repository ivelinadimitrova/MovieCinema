using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.MediatR.CinemaTicketsMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Responses.CinemaTicketsResponses;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.CommandHandler.CinemaTicketsCommandHandler
{
    public class UpdateCinemaTicketCommandHandler : IRequestHandler<UpdateCinemaTicketCommand, GetCinemaTicketResponse>
    {
        private readonly ICinemaTicketRepository _cinemaTicketRepository;
        private readonly ITicketsReportRepository _ticketsReportRepository;
        private readonly IMovieRepository _movieRepository;
        private ILogger<UpdateCinemaTicketCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateCinemaTicketCommandHandler(ICinemaTicketRepository cinemaTicketRepository, ILogger<UpdateCinemaTicketCommandHandler> logger, IMapper mapper, ITicketsReportRepository ticketsReportRepository, IMovieRepository movieRepository)
        {
            _cinemaTicketRepository = cinemaTicketRepository;
            _logger = logger;
            _mapper = mapper;
            _ticketsReportRepository = ticketsReportRepository;
            _movieRepository = movieRepository;
        }

        public async Task<GetCinemaTicketResponse> Handle(UpdateCinemaTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cinemaTicketExists = await _cinemaTicketRepository.GetCinemaTicket(request.ticket.Id);

                if (cinemaTicketExists == null)
                    return new GetCinemaTicketResponse()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = "The cinema ticket with this Id doesn't exists!",
                        CinemaTicket = null
                    };

                var moviesExistCount = 0;

                foreach (var movie in request.ticket.Movies)
                {
                    var exists = _movieRepository.GetMovieById(movie.Id).Result;

                    if (exists != null)
                    {
                        moviesExistCount++;
                    }
                }

                if (moviesExistCount != request.ticket.Movies.Count())
                {
                    return new GetCinemaTicketResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Cinema ticket cannot be updated because this/these movie/s doesn't exist..",
                        CinemaTicket = null
                    };
                }

                var ticketMapper = _mapper.Map<CinemaTicket>(request.ticket);

                var ticketExists = await _ticketsReportRepository.GetCinemaTicketByVisitorId(ticketMapper.VisitorId);

                ticketMapper.Report = ticketExists;

                ticketMapper.TotalMoney = ticketMapper.Movies.Select(x => x.Price).Sum();

                var result = await _cinemaTicketRepository.UpdateCinemaTicket(ticketMapper);

                return new GetCinemaTicketResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Cinema ticket updated successfully.",
                    CinemaTicket = result
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Unable to update the cinema ticket. Error message: {e.Message}");
            }

            return new GetCinemaTicketResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to update the cinema ticket."
            };
        }
    }
}