using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.CinemaTicketsResponses
{
    public class GetCinemaTicketResponse : BaseResponse
    {
        public CinemaTicket CinemaTicket { get; set; }
    }
}