using MovieCinema.Models.Models;

namespace MovieStore.DL.Interfaces
{
    public interface IDirectorRepository
    {
        Task<IEnumerable<Director>> GetAllDirectors();
        Task<Director?> GetDirectorById(int directorId);
        Task<Director?> GetDirectorByName(string directorName);
        Task<Director?> AddDirector(Director director);
        Task<Director?> UpdateDirector(Director director);
        Task<Director?> DeleteDirector(int directorId);
    }
}