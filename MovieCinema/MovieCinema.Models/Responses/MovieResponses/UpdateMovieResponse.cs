using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.MovieResponses
{
    public class UpdateMovieResponse : BaseResponse
    {
        public Movie? Movie { get; set; }
    }
}