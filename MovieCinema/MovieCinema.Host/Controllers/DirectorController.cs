using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCinema.Models.MediatR.DirectorMediatR;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Requests.DirectorRequests;

namespace MovieCinema.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly ILogger<DirectorController> _logger;
        private readonly IMediator _mediator;

        public DirectorController(ILogger<DirectorController> logger, IMapper mapper, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(GetAllDirectors))]
        public async Task<IActionResult> GetAllDirectors()
        {
            return Ok( await _mediator.Send(new GetAllDirectorsCommand()));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetDirectorById))]
        public async Task<IActionResult> GetDirectorById(int directorId)
        {
            if (directorId <= 0)
            {
                _logger.LogWarning("Id must be greater than zero.");
                return BadRequest($"Parameter id: {directorId} must be greater than zero.");
            }

            var result = await _mediator.Send(new GetDirectorByIdCommand(directorId));

            if (result == null)
            {
                _logger.LogWarning("Id not found.");
                return NotFound(directorId);
            }

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetDirectorByName))]
        public async Task<IActionResult> GetDirectorByName(string directorName)
        {
            var result = await _mediator.Send(new GetDirectorByNameCommand(directorName));

            if (result == null) return NotFound($"The director with name: {directorName} does not exists.");

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(AddDirector))]
        public async Task<IActionResult> AddDirector([FromBody] AddDirectorRequest addDirectorRequest)
        {
            var result = await _mediator.Send(new AddDirectorCommand(addDirectorRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest) return BadRequest();

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(nameof(UpdateDirector))]
        public async Task<IActionResult> UpdateDirector([FromBody] UpdateDirectorRequest updateDirectorRequest)
        {
            var result = await _mediator.Send(new UpdateDirectorCommand(updateDirectorRequest));

            if (result.HttpStatusCode == HttpStatusCode.NotFound) return NotFound(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete(nameof(DeleteDirector))]
        public async Task<IActionResult> DeleteDirector(int directorId)
        {
            var movies = await _mediator.Send(new GetMoviesByDirectorIdCommand(directorId));

            if (movies.MovieCollection.ToList().Any()) return BadRequest("The Director with this Id has added movies and can't be deleted.");

            var directorExists = await _mediator.Send(new GetDirectorByIdCommand(directorId));

            if (directorExists.Director == null) return NotFound("The Director with this ID doesn't exists.");

            await _mediator.Send(new DeleteDirectorCommand(directorId));

            return Ok(directorExists);
        }
    }
}