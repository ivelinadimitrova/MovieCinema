using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.CinemaTicketsResponses
{
    public class GetAllCinemaTicketsResponse : BaseResponse
    {
        public IEnumerable<CinemaTicket> CinemaTickets { get; set; }
    }
}