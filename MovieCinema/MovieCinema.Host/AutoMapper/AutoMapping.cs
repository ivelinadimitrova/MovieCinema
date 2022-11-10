using AutoMapper;
using MovieCinema.Models.Models;
using MovieCinema.Models.Requests.CinemaTicketsRequests;
using MovieCinema.Models.Requests.DirectorRequests;
using MovieCinema.Models.Requests.MoviesRequests;

namespace MovieCinema.Host.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AddMovieRequest, Movie>();
            CreateMap<UpdateMovieRequest, Movie>();
            CreateMap<AddMultipleMoviesRequest, Movie>();
            
            CreateMap<AddDirectorRequest, Director>();
            CreateMap<UpdateDirectorRequest, Director>();

            CreateMap<MakeCinemaTicketRequest, CinemaTicket>();
            CreateMap<UpdateCinemaTicketRequest, CinemaTicket>();
        }
    }
}