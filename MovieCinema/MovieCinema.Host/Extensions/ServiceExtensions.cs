using MovieStore.BL.KafkaProducerConsumer.KafkaServices;
using MovieStore.DL.Interfaces;
using MovieStore.DL.Repositories.MongoRepositories;
using MovieStore.DL.Repositories.MsSQLRepositories;

namespace MovieCinema.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<IDirectorRepository, DirectorRepository>();
            services.AddSingleton<ICinemaTicketRepository, CinemaTicketRepository>();
            services.AddSingleton<ITicketsReportRepository, TicketsReportRepository>();

            return services;
        }

        public static IServiceCollection RegisterHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<CinemaTicketProducerService>();
            services.AddHostedService<TicketsReportProducerService>();
            services.AddHostedService<DataFlowCinemaTicketConsumer>();
            services.AddHostedService<TicketsReportConsumerService>();

            return services;
        }
    }
}