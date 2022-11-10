namespace MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration
{
    public class KafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }
        public int AutoOffsetReset { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }
    }
}