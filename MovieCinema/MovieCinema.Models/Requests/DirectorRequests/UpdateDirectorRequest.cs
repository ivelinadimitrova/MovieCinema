namespace MovieCinema.Models.Requests.DirectorRequests
{
    public class UpdateDirectorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}