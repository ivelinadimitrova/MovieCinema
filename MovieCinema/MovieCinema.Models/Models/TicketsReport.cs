using MessagePack;

namespace MovieCinema.Models.Models
{
    [MessagePackObject]
    public class TicketsReport : ICacheItem<int>
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public int VisitorId { get; set; }
        [Key(2)]
        public int TicketsQuantity{ get; set; }
        [Key(3)]
        public DateTime LastUpdated { get; set; }

        public int GetKey() => VisitorId;
    }
}