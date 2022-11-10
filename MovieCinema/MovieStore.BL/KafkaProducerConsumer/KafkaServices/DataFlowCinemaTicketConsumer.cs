using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MovieCinema.Models.Models;
using MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration;
using MovieStore.BL.KafkaProducerConsumer.KafkaGenericServices;
using MovieStore.DL.Interfaces;

namespace MovieStore.BL.KafkaProducerConsumer.KafkaServices
{
    public class DataFlowCinemaTicketConsumer : KafkaConsumerService<int, CinemaTicket>, IHostedService
    {
        private readonly ITicketsReportRepository _ticketsReportRepository;
        private TransformBlock<CinemaTicket, CinemaTicket> _transformBlock;

        public DataFlowCinemaTicketConsumer(IOptions<KafkaConsumerSettings> settings, ITicketsReportRepository ticketsReportRepository) : base(settings)
        {
            _ticketsReportRepository = ticketsReportRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consume(cancellationToken);

            _transformBlock = new TransformBlock<CinemaTicket, CinemaTicket>(async result =>
            {
                var ticketExists = await _ticketsReportRepository.GetCinemaTicketByVisitorId(result.VisitorId);

                if (ticketExists == null)
                {
                    result.Report = await _ticketsReportRepository.MakeCinemaTicket(result.VisitorId);
                }
                else
                {
                    result.Report.TicketsQuantity = await _ticketsReportRepository.AddTicketQuantity(result.VisitorId);
                }

                return result;
            });

            var actionBlock = new ActionBlock<CinemaTicket>(result =>
            {
                if (result.Report.TicketsQuantity >= 5)
                {
                    result.TotalMoney -= 2;
                }
                else if (result.Report.TicketsQuantity >= 10)
                {
                    result.TotalMoney -= 4;
                }
                else if (result.Report.TicketsQuantity >= 20)
                {
                    result.TotalMoney -= 6;
                }
            });

            _transformBlock.LinkTo(actionBlock);

            return Task.CompletedTask;
        }

        public override async Task HandlerMessage(CinemaTicket value)
        {
            await _transformBlock.SendAsync(value);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();

            return Task.CompletedTask;
        }
    }
}