using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;

namespace MovieCinema.Host.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public SqlHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                }
                catch (SqlException e)
                {
                    return HealthCheckResult.Unhealthy(e.Message);
                }
            }

            return HealthCheckResult.Healthy("SQL connection is OK.");
        }
    }
}