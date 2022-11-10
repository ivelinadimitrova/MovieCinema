using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.MovieResponses
{
    public class ReceiveMovieInformationResponse : BaseResponse
    {
        public Movie Movie { get; set; }
    }
}