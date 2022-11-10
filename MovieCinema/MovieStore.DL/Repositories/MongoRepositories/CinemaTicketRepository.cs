using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieCinema.Models.Configurations;
using MovieCinema.Models.Models;
using MovieStore.DL.Interfaces;

namespace MovieStore.DL.Repositories.MongoRepositories
{
    public class CinemaTicketRepository : ICinemaTicketRepository
    {
        private MongoClient _dbClient;

        private IMongoDatabase _database;

        private readonly IMongoCollection<CinemaTicket> _collection;

        private readonly IOptions<MongoDbConfiguration> _configuration;
        public CinemaTicketRepository(IOptions<MongoDbConfiguration> configuration)
        {
            _configuration = configuration;
            _dbClient = new MongoClient(_configuration.Value.ConnectionString);
            _database = _dbClient.GetDatabase(_configuration.Value.DatabaseName);
            _collection = _database.GetCollection<CinemaTicket>(_configuration.Value.CinemaTicketsCollection);
        }

        public async Task<IEnumerable<CinemaTicket?>> GetAllCinemaTickets()
        {
            var result = _collection.Find(new BsonDocument()).ToEnumerable<CinemaTicket>();
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<CinemaTicket?>> GetAllFromDate(DateTime lastUpdpated)
        {
            var filter = Builders<CinemaTicket>.Filter.Where(x => x.LastUpdated > lastUpdpated);
            var result = _collection.Find(filter).ToEnumerable();

            return await Task.FromResult(result);
        }

        public async Task<CinemaTicket?> MakeCinemaTicket(CinemaTicket cinemaTicket)
        {
            var ticket = new CinemaTicket()
            {
                Movies = cinemaTicket.Movies,
                TotalMoney = cinemaTicket.TotalMoney,
                VisitorId = cinemaTicket.VisitorId,
                LastUpdated = DateTime.Now,
                Report = cinemaTicket.Report
            };

            await _collection.InsertOneAsync(ticket);

            return ticket;
        }

        public async Task<CinemaTicket?> UpdateCinemaTicket(CinemaTicket cinemaTicket)
        {
            var updateFiletr = Builders<CinemaTicket>.Filter.Eq("_id", cinemaTicket.Id);

            var update = Builders<CinemaTicket>.Update.Set(ct => ct.Movies, cinemaTicket.Movies)
                .Set(ct => ct.TotalMoney, cinemaTicket.TotalMoney)
                .Set(ct => ct.VisitorId, cinemaTicket.VisitorId)
                .Set(ct => ct.Report, cinemaTicket.Report);

            await _collection.UpdateOneAsync(updateFiletr, update);
            
            return cinemaTicket;
        }

        public async Task<CinemaTicket?> GetCinemaTicket(Guid id)
        {
            var filter = Builders<CinemaTicket>.Filter.Eq("Id", id);

            var ticket = await _collection.Find(filter).FirstOrDefaultAsync();

            return ticket;
        }

        public async Task<Guid?> DeleteCinemaTicket(Guid id)
        {
            var deleteFilter = Builders<CinemaTicket>.Filter.Eq("_id", id);

            await _collection.DeleteOneAsync(deleteFilter);

            return id;
        }
    }
}