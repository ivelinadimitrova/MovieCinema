using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCinema.Models.MediatR.CinemaTicketsMediatR;
using MovieCinema.Models.MediatR.TicketsReportMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Requests.CinemaTicketsRequests;
using MovieStore.BL.KafkaProducerConsumer.KafkaGenericServices;

namespace MovieCinema.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CinemaTicketController : ControllerBase
    {
        private readonly ILogger<CinemaTicketController> _logger;
        private readonly IMediator _mediator;

        public CinemaTicketController(ILogger<CinemaTicketController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetAllTicketsReport))]
        public async Task<IActionResult> GetAllTicketsReport()
        {
            var result = await _mediator.Send(new GetAllTicketsReportCommand());

            if (result.TicketsReport == null) return NotFound(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetAllCinemaTickets))]
        public async Task<IActionResult> GetAllCinemaTickets()
        {
            var result = await _mediator.Send(new GetAllCinemaTicketsCommand());

            if (result.CinemaTickets == null) return NotFound(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetCinemaTicket))]
        public async Task<IActionResult> GetCinemaTicket(Guid id)
        {
            var result = await _mediator.Send(new GetCinemaTicketCommand(id));

            if (result.CinemaTicket == null) return NotFound(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost(nameof(MakeCinemaTicket))]
        public async Task<IActionResult> MakeCinemaTicket([FromBody] MakeCinemaTicketRequest cinemaTicket)
        {
            return Ok(await _mediator.Send(new MakeCinemaTicketCommand(cinemaTicket)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut(nameof(UpdateCinemaTicket))]
        public async Task<IActionResult> UpdateCinemaTicket([FromBody] UpdateCinemaTicketRequest cinemaTicket)
        {
            return Ok(await _mediator.Send(new UpdateCinemaTicketCommand(cinemaTicket)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete(nameof(DeleteCinemaTicket))]
        public async Task<IActionResult> DeleteCinemaTicket(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteCinemaTicketCommand(id)));
        }
    }
}