using MovieCinema.Models.Models;

namespace MovieCinema.Models.Responses.TicketsReportResponse
{
    public class GetAllTickersReportResponse : BaseResponse
    {
        public IEnumerable<TicketsReport> TicketsReport { get; set; }
    }
}