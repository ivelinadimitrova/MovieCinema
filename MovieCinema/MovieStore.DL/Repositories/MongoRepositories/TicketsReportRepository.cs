using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieCinema.Models.Configurations;
using MovieCinema.Models.Models;
using MovieStore.DL.Interfaces;

namespace MovieStore.DL.Repositories.MongoRepositories
{
    public class TicketsReportRepository : ITicketsReportRepository
    {

        private MongoClient _dbClient;

        private IMongoDatabase _database;

        private IMongoCollection<TicketsReport> _collection;

        private IOptions<MongoDbConfiguration> _configuration;

        public TicketsReportRepository(IOptions<MongoDbConfiguration> configuration)
        {
            _configuration = configuration;
            _dbClient = new MongoClient(_configuration.Value.ConnectionString);
            _database = _dbClient.GetDatabase(_configuration.Value.DatabaseName);
            _collection = _database.GetCollection<TicketsReport>(_configuration.Value.TicketsReportsCollection);
        }

        public async Task<IEnumerable<TicketsReport>> GetAllTickets()
        {
            var result = _collection.Find(new BsonDocument()).ToEnumerable<TicketsReport>();

            return await Task.FromResult(result);
        }

        public async Task<TicketsReport?> GetCinemaTicketByVisitorId(int id)
        {
            var filter = Builders<TicketsReport>.Filter.Eq("VisitorId", id);

            var ticket = await _collection.Find(filter).FirstOrDefaultAsync();

            return ticket;
        }

        public async Task<TicketsReport?> MakeCinemaTicket(int visitorId)
        {
            var ticketReport = new TicketsReport()
            {
                VisitorId = visitorId,
                TicketsQuantity = 1,
                LastUpdated = DateTime.Now
            };

            await _collection.InsertOneAsync(ticketReport);

            return ticketReport;
        }

        public async Task<IEnumerable<TicketsReport?>> GetAllFromDate(DateTime lastUpdpated)
        {
            var filter = Builders<TicketsReport>.Filter.Where(x => x.LastUpdated > lastUpdpated);
            var result = _collection.Find(filter).ToEnumerable();

            return await Task.FromResult(result);
        }

        public async Task<int> AddTicketQuantity(int visitorId)
        {
            var filter = Builders<TicketsReport>.Filter.Eq("VisitorId", visitorId);

            var ticketQuantityIncreaser = await _collection.Find(filter).FirstOrDefaultAsync();

            var ticketQuantity = ticketQuantityIncreaser.TicketsQuantity + 1;

            var update = Builders<TicketsReport>.Update.Set(tr => tr.TicketsQuantity, ticketQuantity);

            await _collection.UpdateOneAsync(filter, update);

            return ticketQuantity;
        }
    }
}