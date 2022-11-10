using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.MovieResponses
{
    public class GetMoviesByDirectorIdResponse : BaseResponse
    {
        public IEnumerable<Movie> MovieCollection { get; set; }
    }
}