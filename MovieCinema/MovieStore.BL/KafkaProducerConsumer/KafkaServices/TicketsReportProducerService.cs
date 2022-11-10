using Microsoft.Extensions.Options;
using MovieCinema.Models.Models;
using MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration;
using MovieStore.BL.KafkaProducerConsumer.KafkaGenericServices;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.KafkaProducerConsumer.KafkaServices
{
    public class TicketsReportProducerService : KafkaProducerService<int, TicketsReport>//, IHostedService
    {
        private ITicketsReportRepository _ticketsReportRepository;
        private DateTime _lastUpdate = DateTime.Now;

        public TicketsReportProducerService(IOptions<KafkaProducerSettings> producerSettings, ITicketsReportRepository ticketsReportRepository) : base(producerSettings)
        {
            _ticketsReportRepository = ticketsReportRepository;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var data = await _ticketsReportRepository.GetAllFromDate(_lastUpdate);

                    if (data != null)
                    {
                        foreach (var ticket in data)
                        {
                            await Publish(ticket, cancellationToken);
                            
                            _lastUpdate = DateTime.Now;
                        }
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }
    }
}