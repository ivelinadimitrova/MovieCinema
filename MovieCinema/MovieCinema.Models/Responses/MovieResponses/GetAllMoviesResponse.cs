using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.MovieResponses
{
    public class GetAllMoviesResponse : BaseResponse
    {
        public IEnumerable<Movie?> Movies { get; set; }
    }
}