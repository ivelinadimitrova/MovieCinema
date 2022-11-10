using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.DirectorResponses
{
    public class UpdateDirectorResponse : BaseResponse
    {
        public Director? Director { get; set; }
    }
}