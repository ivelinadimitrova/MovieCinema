namespace MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration
{
    public class KafkaProducerSettings
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
    }
}