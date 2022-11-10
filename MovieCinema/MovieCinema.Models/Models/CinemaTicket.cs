using MessagePack;

namespace MovieCinema.Models.Models
{
    [MessagePackObject]
    public record CinemaTicket : ICacheItem<int>
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public IEnumerable<Movie> Movies { get; set; }
        [Key(2)]
        public decimal TotalMoney { get; set; }
        [Key(3)]
        public int VisitorId { get; set; }
        [Key(4)]
        public DateTime LastUpdated { get; set; }
        [Key(5)]
        public TicketsReport Report { get; set; }
        public int GetKey() => VisitorId;
    }
}