using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.DirectorResponses
{
    public class ReceiveDirectorInformationResponse : BaseResponse
    {
        public Director Director { get; set; }
    }
}