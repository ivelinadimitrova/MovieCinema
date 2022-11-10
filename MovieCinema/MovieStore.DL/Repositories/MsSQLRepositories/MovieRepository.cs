using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieCinema.Models.Models;
using MovieStore.DL.Interfaces;

namespace MovieStore.DL.Repositories.MsSQLRepositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ILogger<MovieRepository> _logger;
        private readonly IConfiguration _configuration;

        public MovieRepository(ILogger<MovieRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryAsync<Movie>("SELECT * FROM Movies WITH(NOLOCK)");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllMovies)}: {e.Message}", e);
            }

            return Enumerable.Empty<Movie>();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByDirectorId(int directorId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryAsync<Movie>("SELECT * FROM Movies WHERE DirectorId = @DirectorId", new { DirectorId = directorId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetMoviesByDirectorId)}: {e.Message}", e);
            }

            return Enumerable.Empty<Movie>();
        }

        public async Task<Movie?> GetMovieById(int movieId)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<Movie>("SELECT * FROM Movies WITH(NOLOCK) WHERE Id = @Id", new { Id = movieId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {nameof(GetMovieById)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<Movie?> GetMovieByName(string movieName)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryFirstOrDefaultAsync<Movie>("SELECT * FROM Movies WITH(NOLOCK) WHERE MovieName = @MovieName", new { MovieName = movieName });
                    
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {nameof(GetMovieByName)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<Movie?> AddMovie(Movie movie)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteScalarAsync<Movie>("INSERT INTO Movies (MovieName, Actors, DirectorId, Quantity, ReleaseDate, LastUpdated, Price) " +
                                                                      "VALUES (@MovieName, @Actors, @DirectorId, @Quantity, @ReleaseDate, @LastUpdated, @Price)", movie);

                    return movie;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddMovie)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<bool> AddMultipleMovies(IEnumerable<Movie> movieCollection)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("INSERT INTO Movies (MovieName, Actors, DirectorId, Quantity, ReleaseDate, LastUpdated, Price) " +
                                                               "VALUES (@MovieName, @Actors, @DirectorId, @Quantity, @ReleaseDate, @LastUpdated, @Price)", movieCollection);
                    return result > 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddMultipleMovies)}: {e.Message}", e);
            }

            return false;
        }

        public async Task<Movie?> UpdateMovie(Movie movie)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteScalarAsync("UPDATE Movies SET MovieName = @MovieName, Actors = @Actors, DirectorId = @DirectorId, Quantity = @Quantity, " +
                                                        "ReleaseDate = @ReleaseDate, LastUpdated = @LastUpdated, Price = @Price WHERE Id = @Id", movie);

                    return movie;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateMovie)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<Movie?> DeleteMovie(int movieId)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync("DELETE FROM Movies WHERE Id = @Id", new { Id = movieId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteMovie)}: {e.Message}", e);
            }

            return null;
        }
    }
}