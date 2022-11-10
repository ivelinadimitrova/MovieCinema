using MovieCinema.Models.Models;

namespace MovieStore.DL.Interfaces
{
    public interface ICinemaTicketRepository
    {
        Task<IEnumerable<CinemaTicket?>> GetAllCinemaTickets();
        Task<IEnumerable<CinemaTicket?>> GetAllFromDate(DateTime lastUpdpated);
        Task<CinemaTicket?> MakeCinemaTicket(CinemaTicket cinemaTicket);
        Task<CinemaTicket?> UpdateCinemaTicket(CinemaTicket cinemaTicket);
        Task<CinemaTicket?> GetCinemaTicket(Guid id);
        Task<Guid?> DeleteCinemaTicket(Guid id);
    }
}