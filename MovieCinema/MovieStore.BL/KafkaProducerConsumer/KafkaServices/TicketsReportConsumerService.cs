using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MovieCinema.Models.Models;
using MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration;
using MovieStore.BL.KafkaProducerConsumer.KafkaGenericServices;

namespace MovieStore.BL.KafkaProducerConsumer.KafkaServices
{
    public class TicketsReportConsumerService : KafkaConsumerService<int, TicketsReport>, IHostedService
    {

        public TicketsReportConsumerService(IOptions<KafkaConsumerSettings> consumerSettings) : base(consumerSettings)
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consume(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();

            return Task.CompletedTask;
        }

        public override Task HandlerMessage(TicketsReport value)
        {
            return Task.FromResult(value);
        }
    }
}