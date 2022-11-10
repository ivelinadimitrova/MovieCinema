using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Requests.MoviesRequests;

namespace MovieCinema.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMediator _mediator;
        public MovieController(ILogger<MovieController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(GetAllMovies))]
        public async Task<IActionResult> GetAllMovies()
        {
            return Ok(await _mediator.Send(new GetAllMoviesCommand()));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetMovieById))]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            if (movieId <= 0)
            {
                _logger.LogWarning("Id must be greater than zero.");
                return BadRequest($"Parameter id: {movieId} must be greater than zero.");
            }

            var result = await _mediator.Send(new GetMovieByIdCommand(movieId));

            if (result == null)
            {
                _logger.LogWarning("Id not found.");
                return NotFound(movieId);
            }

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetMovieByName))]
        public async Task<IActionResult> GetMovieByName(string movieName)
        {
            var result = await _mediator.Send((new GetMovieByNameCommand(movieName)));

            if (result == null) return NotFound($"Movie with this name: \"{movieName}\" wasn't found");

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(AddMovie))]
        public async Task<IActionResult> AddMovie([FromBody] AddMovieRequest addMovieRequest)
        {
            var result = await _mediator.Send(new AddMovieCommand(addMovieRequest));

            if (result == null) return BadRequest("Can't add this movie, a director with this Id doesn't exists.");

            if (result.HttpStatusCode == HttpStatusCode.BadRequest) return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(AddMultipleMovies))]
        public async Task<IActionResult> AddMultipleMovies([FromBody] AddMultipleMoviesRequest addMultipleMoviesRequest)
        {
            if (addMultipleMoviesRequest != null && !addMultipleMoviesRequest.MovieCollection.Any())
                return BadRequest(addMultipleMoviesRequest);

            var result = await _mediator.Send(new AddMultipleMoviesCommand(addMultipleMoviesRequest));

            if (!result.Result) return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(nameof(UpdateMovie))]
        public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieRequest updateMovieRequest)
        {
            var result = await _mediator.Send(new UpdateMovieCommand(updateMovieRequest));

            if (result.HttpStatusCode == HttpStatusCode.NotFound) return NotFound(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete(nameof(DeleteMovie))]
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            if (await _mediator.Send(new GetMovieByIdCommand(movieId)) == null) return NotFound("Movie with this ID doesn't exists.");

            var result = await _mediator.Send(new DeleteMovieCommand(movieId));

            return Ok(result);
        }
    }
}