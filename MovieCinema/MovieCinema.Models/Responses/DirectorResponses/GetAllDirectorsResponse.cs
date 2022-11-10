using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.DirectorResponses
{
    public class GetAllDirectorsResponse : BaseResponse
    {
        public IEnumerable<Director?> DirectorCollection { get; set; }
    }
}