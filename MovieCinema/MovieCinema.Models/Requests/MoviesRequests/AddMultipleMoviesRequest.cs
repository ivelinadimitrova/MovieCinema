using MovieCinema.Models.Models;

namespace MovieCinema.Models.Requests.MoviesRequests
{
    public class AddMultipleMoviesRequest
    {
        public IEnumerable<Movie> MovieCollection { get; set; }
    }
}