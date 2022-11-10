namespace MovieCinema.Models.Models
{
    public record Director
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string NickName { get; init; }
        public int Age { get; init; }
        public DateTime DateOfBirth { get; init; }
    }
}