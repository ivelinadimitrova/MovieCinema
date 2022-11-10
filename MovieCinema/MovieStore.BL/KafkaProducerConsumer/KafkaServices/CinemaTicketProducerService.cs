using Microsoft.Extensions.Options;
using MovieCinema.Models.Models;
using MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration;
using MovieStore.BL.KafkaProducerConsumer.KafkaGenericServices;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.KafkaProducerConsumer.KafkaServices
{
    public class CinemaTicketProducerService : KafkaProducerService<int, CinemaTicket>//, IHostedService
    {
        private ICinemaTicketRepository _cinemaTicketRepository;
        private DateTime _lastUpdate = DateTime.Now;

        public CinemaTicketProducerService(IOptions<KafkaProducerSettings> producerSettings, ICinemaTicketRepository cinemaTicketRepository) : base(producerSettings)
        {
            _cinemaTicketRepository = cinemaTicketRepository;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var data = await _cinemaTicketRepository.GetAllFromDate(_lastUpdate);

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