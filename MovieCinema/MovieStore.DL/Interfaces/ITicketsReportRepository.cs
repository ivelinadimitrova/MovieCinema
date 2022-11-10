using MovieCinema.Models.Models;

namespace MovieStore.DL.Interfaces
{
    public interface ITicketsReportRepository
    {
        Task<IEnumerable<TicketsReport>> GetAllTickets();
        Task<TicketsReport?> GetCinemaTicketByVisitorId(int id);
        Task<TicketsReport?> MakeCinemaTicket(int visitorId);
        Task<int> AddTicketQuantity(int visitorId);
        Task<IEnumerable<TicketsReport?>> GetAllFromDate(DateTime lastUpdpated);
    }
}