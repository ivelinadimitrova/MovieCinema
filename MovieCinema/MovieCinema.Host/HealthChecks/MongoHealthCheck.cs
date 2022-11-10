using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieCinema.Models.Configurations;

namespace MovieCinema.Host.HealthChecks
{
    public class MongoHealthCheck : IHealthCheck
    {
        private IMongoDatabase _database;
        private MongoClient _mongoClient;
        
        public MongoHealthCheck(IOptions<MongoDbConfiguration> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.ConnectionString);

            _database = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
            }
            catch (MongoException e)
            {
                return HealthCheckResult.Unhealthy(e.Message);
            }
            return HealthCheckResult.Healthy("MongoDB health check is successful.");
        }
    }
}
