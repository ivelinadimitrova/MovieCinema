namespace MovieCinema.Models.Requests.MoviesRequests
{
    public class UpdateMovieRequest
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string Actors { get; set; }
        public int DirectorId { get; set; }
        public int Quantity { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal Price { get; set; }
    }
}