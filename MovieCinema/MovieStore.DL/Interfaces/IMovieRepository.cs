using MovieCinema.Models.Models;

namespace MovieStore.DL.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMovies();
        Task<Movie?> GetMovieById(int movieId);
        Task<Movie?> GetMovieByName(string movieName);
        Task<Movie?> AddMovie(Movie movie);
        Task<Movie?> UpdateMovie(Movie movie);
        Task<Movie?> DeleteMovie(int movieId);
        Task<bool> AddMultipleMovies(IEnumerable<Movie> movieCollection);
        Task<IEnumerable<Movie>> GetMoviesByDirectorId(int directorId);
    }
}