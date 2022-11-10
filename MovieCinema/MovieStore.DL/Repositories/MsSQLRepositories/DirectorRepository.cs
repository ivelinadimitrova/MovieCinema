using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MovieCinema.Models.Models;
using MovieStore.DL.Interfaces;

namespace MovieStore.DL.Repositories.MsSQLRepositories
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly ILogger<DirectorRepository> _logger;
        private readonly IConfiguration _configuration;

        public DirectorRepository(ILogger<DirectorRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Director>> GetAllDirectors()
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryAsync<Director>("SELECT * FROM Directors WITH(NOLOCK)");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllDirectors)}: {e.Message}", e);
            }

            return Enumerable.Empty<Director>();
        }

        public async Task<Director?> GetDirectorById(int directorId)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<Director>("SELECT * FROM Directors WITH(NOLOCK) WHERE Id = @Id", new { Id = directorId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetDirectorById)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<Director?> GetDirectorByName(string directorName)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var result =  await connection.QueryFirstOrDefaultAsync<Director>("SELECT * FROM Directors WITH(NOLOCK) WHERE Name = @Name", new { Name = directorName });

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetDirectorByName)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<Director?> AddDirector(Director director)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteScalarAsync<Director>("INSERT INTO Directors (Name, NickName, Age, DateOfBirth) " +
                                                                         "VALUES (@Name, @NickName, @Age, @DateOfBirth)", director);

                    return director;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddDirector)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<Director?> UpdateDirector(Director director)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteScalarAsync("UPDATE Directors SET Name = @Name, NickName = @NickName, Age = @Age, DateOfBirth = @DateOfBirth WHERE Id = @Id", director);

                    return director;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateDirector)}: {e.Message}", e);
            }

            return null;
        }

        public async Task<Director?> DeleteDirector(int directorId)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync("DELETE FROM Directors WHERE Id = @Id", new { Id = directorId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteDirector)}: {e.Message}", e);
            }

            return null;
        }
    }
}