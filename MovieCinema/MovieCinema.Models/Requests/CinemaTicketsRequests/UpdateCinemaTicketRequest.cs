using MovieCinema.Models.Models;

namespace MovieCinema.Models.Requests.CinemaTicketsRequests
{
    public class UpdateCinemaTicketRequest
    {
        public Guid Id { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public decimal TotalMoney { get; set; }
        public int VisitorId { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}