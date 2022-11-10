using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MovieCinema.Models.Models;
using MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration;
using MovieStore.BL.KafkaProducerConsumer.MsgPack;

namespace MovieStore.BL.KafkaProducerConsumer.KafkaGenericServices
{
    public abstract class KafkaProducerService<TKey, TValue> : IHostedService where TValue : ICacheItem<TKey>
    {
        private IOptions<KafkaProducerSettings> _producerSettings;
        private IProducer<TKey, TValue> _producer;

        public KafkaProducerService(IOptions<KafkaProducerSettings> producerSettings)
        {
            _producerSettings = producerSettings;

            var config = new ProducerConfig()
            {
                BootstrapServers = _producerSettings.Value.BootstrapServers,
            };

            _producer = new ProducerBuilder<TKey, TValue>(config)
                .SetKeySerializer(new MsgPackSerializer<TKey>())
                .SetValueSerializer(new MsgPackSerializer<TValue>())
                .Build();
        }

        public async Task Publish(TValue data, CancellationToken token)
        {
            var message = new Message<TKey, TValue>
            {
                Key = data.GetKey(),
                Value = data
            };

            var result = await _producer.ProduceAsync($"{_producerSettings.Value.Topic}{typeof(TValue).Name}", message, token);
        }

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}