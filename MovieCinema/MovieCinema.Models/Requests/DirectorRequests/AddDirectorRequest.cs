namespace MovieCinema.Models.Requests.DirectorRequests
{
    public class AddDirectorRequest
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}