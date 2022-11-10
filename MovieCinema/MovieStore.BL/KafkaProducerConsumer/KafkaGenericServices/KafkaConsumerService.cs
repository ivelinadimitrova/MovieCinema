using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration;
using MovieStore.BL.KafkaProducerConsumer.MsgPack;

namespace MovieStore.BL.KafkaProducerConsumer.KafkaGenericServices
{
    public abstract class KafkaConsumerService<TKey, TValue>
    {
        private IOptions<KafkaConsumerSettings> _consumerSettings;
        private IConsumer<TKey, TValue> _consumer;

        public KafkaConsumerService(IOptions<KafkaConsumerSettings> consumerSettings)
        {
            _consumerSettings = consumerSettings;

            var config = new ConsumerConfig()
            {
                BootstrapServers = _consumerSettings.Value.BootstrapServers,
                AutoOffsetReset = (AutoOffsetReset?)_consumerSettings.Value.AutoOffsetReset,
                GroupId = _consumerSettings.Value.GroupId
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetKeyDeserializer(new MsgPackDeserializer<TKey>())
                .SetValueDeserializer(new MsgPackDeserializer<TValue>())
                .Build();

            _consumer.Subscribe($"{_consumerSettings.Value.Topic}{typeof(TValue).Name}");
        }

        public abstract Task HandlerMessage(TValue value);

        public Task Consume(CancellationToken cancellationToken)
        {
            
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume();

                    await HandlerMessage(cr.Message.Value);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }
        public Task Dispose()
        {
            _consumer.Dispose();

            return Task.CompletedTask;
        }
    }
}