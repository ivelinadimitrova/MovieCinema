using MessagePack;

namespace MovieCinema.Models.Models
{
    [MessagePackObject]
    public record Movie
    {
        [Key(0)]
        public int Id { get; init; }
        [Key(1)]
        public string MovieName { get; init; }
        [Key(2)]
        public string Actors { get; init; }
        [Key(3)]
        public int DirectorId { get; init; }
        [Key(4)]
        public int Quantity { get; set; }
        [Key(5)]
        public DateTime ReleaseDate { get; set; }
        [Key(6)]
        public DateTime LastUpdated { get; set; }
        [Key(7)]
        public decimal Price { get; set; }
    }
}