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
    public class MakeCinemaTicketCommandHandler : IRequestHandler<MakeCinemaTicketCommand, GetCinemaTicketResponse>
    {
        private readonly ICinemaTicketRepository _cinemaTicketRepository;
        private readonly ITicketsReportRepository _ticketsReportRepository;
        private readonly IMovieRepository _movieRepository;
        private ILogger<MakeCinemaTicketCommandHandler> _logger;
        private readonly IMapper _mapper;

        public MakeCinemaTicketCommandHandler(ICinemaTicketRepository cinemaTicketRepository, ILogger<MakeCinemaTicketCommandHandler> logger, IMapper mapper, ITicketsReportRepository ticketsReportRepository, IMovieRepository movieRepository)
        {
            _cinemaTicketRepository = cinemaTicketRepository;
            _logger = logger;
            _mapper = mapper;
            _ticketsReportRepository = ticketsReportRepository;
            _movieRepository = movieRepository;
        }

        public async Task<GetCinemaTicketResponse> Handle(MakeCinemaTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var moviesExistCount = 0;

                foreach (var movie in request.ticket.Movies)
                {
                    var exists = _movieRepository.GetMovieById(movie.Id).Result;

                    if(exists != null)
                    {
                        moviesExistCount++;
                    }
                }

                if(moviesExistCount != request.ticket.Movies.Count())
                {
                    return new GetCinemaTicketResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Cinema ticket cannot be updated because this/these movie/s doesn't exist..",
                        CinemaTicket = null
                    };
                }

                var ticketMapper = _mapper.Map<CinemaTicket>(request.ticket);

                ticketMapper.LastUpdated = DateTime.Now;

                var ticketExists = await _ticketsReportRepository.GetCinemaTicketByVisitorId(ticketMapper.VisitorId);

                if (ticketExists == null)
                {
                    ticketMapper.Report = null;
                }
                else
                {
                    ticketMapper.Report = ticketExists;
                }

                ticketMapper.TotalMoney = ticketMapper.Movies.Select(x => x.Price).Sum();

                var result = await _cinemaTicketRepository.MakeCinemaTicket(ticketMapper);
                
                return new GetCinemaTicketResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Cinema ticket successfully created.",
                    CinemaTicket = result
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Unable to add the cinema ticket. Error message: {e.Message}");
            }

            return new GetCinemaTicketResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to add the cinema ticket.",
                CinemaTicket = null
            };
        }
    }
}
