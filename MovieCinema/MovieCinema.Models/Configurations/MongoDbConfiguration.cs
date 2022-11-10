namespace MovieCinema.Models.Configurations
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CinemaTicketsCollection { get; set; }
        public string TicketsReportsCollection { get; set; }

    }
}