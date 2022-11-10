using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.MovieResponses
{
    public class AddMovieResponse : BaseResponse
    {
        public Movie? Movie { get; set; }
    }
}