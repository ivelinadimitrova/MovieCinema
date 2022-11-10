using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.DirectorResponses
{
    public class AddDirectorResponse : BaseResponse
    {
        public Director? Director { get; set; }
    }
}