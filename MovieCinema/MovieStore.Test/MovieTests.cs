using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using MovieCinema.Host.AutoMapper;
using MovieCinema.Models.MediatR.MovieMediatR;
using MovieCinema.Models.Models;
using MovieCinema.Models.Requests.MoviesRequests;
using MovieStore.BL.CommandHandler.MovieCommandHandlers;
using MovieStore.DL.Interfaces;

namespace MovieStore.Test
{
    public class MovieTests
    {
        private IList<Movie> _movies = new List<Movie>()
        {
            new Movie()
            {
                Id = 1,
                Actors = "some actors",
                DirectorId = 2,
                ReleaseDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                MovieName = "Movie Name First",
                Price = 15,
                Quantity = 3
            },
            new Movie()
            {
                Id = 2,
                Actors = "John, David",
                DirectorId = 1,
                ReleaseDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                MovieName = "Second Movie",
                Price = 30,
                Quantity = 2
            }
        };

        private IEnumerable<Movie> _moviesEnumerable;

        private IList<Director> _directors = new List<Director>()
        {
            new Director()
            {
                Id = 1,
                DateOfBirth = DateTime.Now,
                Age = 23,
                Name = "Patrick",
                NickName = "Pashi"
            },
            new Director()
            {
                Id = 2,
                DateOfBirth = DateTime.Now,
                Age = 67,
                Name = "Jorshua",
                NickName = "Jon"
            }
        };

        private readonly IMapper _mapper;
        private readonly Mock<IMovieRepository> _movieRepository;
        private readonly Mock<IDirectorRepository> _directorRepository;
        private readonly CancellationTokenSource _tokenSource;
        private Mock<ILogger<GetAllMoviesCommandHandler>> _loggerGetAllMoviesCommandHandler;
        private Mock<ILogger<GetMovieByIdCommandHandler>> _loggerGetMovieByIdCommandHandler;
        private Mock<ILogger<AddMovieCommandHandler>> _loggerAddMovieCommandHandler;
        private Mock<ILogger<GetMovieByNameCommandHandler>> _loggerGetMovieByNameCommandHandler;
        private Mock<ILogger<UpdateMovieCommandHandler>> _loggerUpdateMovieCommandHandler;
        private Mock<ILogger<DeleteMovieCommandHandler>> _loggerDeleteMovieCommandHandler;
        private Mock<ILogger<AddMultipleMoviesCommandHandler>> _loggerAddMultipleMoviesCommandHandler;
        private Mock<ILogger<GetMoviesByDirectorIdCommandHandler>> _loggerGetMoviesByDirectorIdCommandHandler;

        public MovieTests()
        {
            _moviesEnumerable = _movies;

            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });

            _mapper = mockMapperConfig.CreateMapper();

            _directorRepository = new Mock<IDirectorRepository>();
            _movieRepository = new Mock<IMovieRepository>();

            _tokenSource = new CancellationTokenSource();

            _loggerGetAllMoviesCommandHandler = new Mock<ILogger<GetAllMoviesCommandHandler>>();
            _loggerGetMovieByIdCommandHandler = new Mock<ILogger<GetMovieByIdCommandHandler>>();
            _loggerAddMovieCommandHandler = new Mock<ILogger<AddMovieCommandHandler>>();
            _loggerGetMovieByNameCommandHandler = new Mock<ILogger<GetMovieByNameCommandHandler>>();
            _loggerUpdateMovieCommandHandler = new Mock<ILogger<UpdateMovieCommandHandler>>();
            _loggerDeleteMovieCommandHandler = new Mock<ILogger<DeleteMovieCommandHandler>>();
            _loggerAddMultipleMoviesCommandHandler = new Mock<ILogger<AddMultipleMoviesCommandHandler>>();
            _loggerGetMoviesByDirectorIdCommandHandler = new Mock<ILogger<GetMoviesByDirectorIdCommandHandler>>();
        }

        [Fact]
        public async Task Movie_GetAll_Count_Check()
        {
            //Setup
            var expectedCount = 2;

            _movieRepository.Setup(m => m.GetAllMovies()).ReturnsAsync(_movies);

            //Inject
            var command = new GetAllMoviesCommand();
            var handlerGetAll =
                new GetAllMoviesCommandHandler(_movieRepository.Object, _loggerGetAllMoviesCommandHandler.Object);

            //Act
            var result = await handlerGetAll.Handle(command, _tokenSource.Token);

            //Assert
            Assert.NotNull(result.Movies);
            Assert.NotEmpty(result.Movies);
            Assert.Equal(expectedCount, result.Movies.Count());
            Assert.Equal(_movies, result.Movies);
        }

        [Fact]
        public async Task Movie_GetMovieById_Ok()
        {
            //Setup
            var movieId = 1;

            _movieRepository.Setup(m => m.GetMovieById(movieId))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieId));

            //Inject
            var command = new GetMovieByIdCommand(movieId);
            var handlerGetById =
                new GetMovieByIdCommandHandler(_movieRepository.Object, _loggerGetMovieByIdCommandHandler.Object);

            //Act
            var result = await handlerGetById.Handle(command, _tokenSource.Token);

            //Assert
            Assert.NotNull(result.Movie);
            Assert.Equal(movieId, result.Movie.Id);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_GetMovieById_NotFound()
        {
            //Setup
            var movieId = 3;

            _movieRepository.Setup(m => m.GetMovieById(movieId))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieId));

            //Inject
            var command = new GetMovieByIdCommand(movieId);
            var handlerGetById =
                new GetMovieByIdCommandHandler(_movieRepository.Object, _loggerGetMovieByIdCommandHandler.Object);

            //Act
            var result = await handlerGetById.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_GetMovieById_BadRequest()
        {
            //Setup
            var movieId = -2;

            _movieRepository.Setup(m => m.GetMovieById(movieId))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieId));

            //Inject
            var command = new GetMovieByIdCommand(movieId);
            var handlerGetById =
                new GetMovieByIdCommandHandler(_movieRepository.Object, _loggerGetMovieByIdCommandHandler.Object);

            //Act
            var result = await handlerGetById.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_GetMovieByName_Ok()
        {
            //Setup
            var movieName = "Second Movie";

            _movieRepository.Setup(m => m.GetMovieByName(movieName))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.MovieName == movieName));

            //Inject
            var command = new GetMovieByNameCommand(movieName);
            var handlerGetById =
                new GetMovieByNameCommandHandler(_movieRepository.Object, _loggerGetMovieByNameCommandHandler.Object);

            //Act
            var result = await handlerGetById.Handle(command, _tokenSource.Token);

            //Assert
            Assert.NotNull(result.Movie);
            Assert.Equal(movieName, result.Movie.MovieName);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_GetMovieByName_NotFound()
        {
            //Setup
            var movieName = "Random Movie";

            _movieRepository.Setup(m => m.GetMovieByName(movieName))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.MovieName == movieName));

            //Inject
            var command = new GetMovieByNameCommand(movieName);
            var handlerGetById =
                new GetMovieByNameCommandHandler(_movieRepository.Object, _loggerGetMovieByNameCommandHandler.Object);

            //Act
            var result = await handlerGetById.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            Assert.Equal("The movie doesn't exists", result.Message);
        }

        [Fact]
        public async Task Movie_AddMovie_Ok()
        {
            //Setup
            var movieRequest = new AddMovieRequest()
            {
                Actors = "Actors",
                DirectorId = 1,
                ReleaseDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                MovieName = "SomeName",
                Price = 10,
                Quantity = 1
            };

            var expectedId = 3;

            _movieRepository.Setup(m => m.AddMovie(It.IsAny<Movie>()))
                .Callback(() =>
                {
                    _movies.Add(new Movie()
                    {
                        Id = expectedId,
                        Actors = movieRequest.Actors,
                        DirectorId = movieRequest.DirectorId,
                        ReleaseDate = movieRequest.ReleaseDate,
                        LastUpdated = movieRequest.LastUpdated,
                        MovieName = movieRequest.MovieName,
                        Price = movieRequest.Price,
                        Quantity = movieRequest.Quantity
                    });
                }).ReturnsAsync(() => _movies.FirstOrDefault(x => x.Id == expectedId));

            _directorRepository.Setup(x => x.GetDirectorById(movieRequest.DirectorId))
                .ReturnsAsync(_directors.FirstOrDefault(x => x.Id == movieRequest.DirectorId));

            //Inject
            var command = new AddMovieCommand(movieRequest);
            var handlerAddMovie =
                new AddMovieCommandHandler(_movieRepository.Object, _mapper, _loggerAddMovieCommandHandler.Object, _directorRepository.Object);

            //Act
            var result = await handlerAddMovie.Handle(command, _tokenSource.Token);

            //Assert
            Assert.NotNull(result.Movie);
            Assert.Equal(expectedId, result.Movie.Id);
            Assert.Equal("Movie successfully added.", result.Message);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_AddMovie_NotFound()
        {
            //Setup
            var movieRequest = new AddMovieRequest()
            {
                Actors = "Actors",
                DirectorId = 2,
                ReleaseDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                MovieName = "Some movie name",
                Price = 10,
                Quantity = 1
            };

            var expectedId = 3;

            _movieRepository.Setup(m => m.AddMovie(It.IsAny<Movie>()))
                .Callback(() =>
                {
                    _movies.Add(new Movie()
                    {
                        Actors = movieRequest.Actors,
                        DirectorId = movieRequest.DirectorId,
                        ReleaseDate = movieRequest.ReleaseDate,
                        LastUpdated = movieRequest.LastUpdated,
                        MovieName = movieRequest.MovieName,
                        Price = movieRequest.Price,
                        Quantity = movieRequest.Quantity
                    });
                }).ReturnsAsync(() => _movies.FirstOrDefault(x => x.Id == expectedId));

            _directorRepository.Setup(x => x.GetDirectorById(movieRequest.DirectorId))
                .ReturnsAsync(_directors.FirstOrDefault(x => x.Id == expectedId));

            //Inject
            var command = new AddMovieCommand(movieRequest);
            var handlerAddMovie =
                new AddMovieCommandHandler(_movieRepository.Object, _mapper, _loggerAddMovieCommandHandler.Object, _directorRepository.Object);

            //Act
            var result = await handlerAddMovie.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            Assert.Equal("You can't add the movie because the director with this Id doesn't!", result.Message);
        }

        [Fact]
        public async Task Movie_AddMovie_BadRequest()
        {
            //Setup
            var movieRequest = new AddMovieRequest()
            {
                Actors = "Actors",
                DirectorId = 2,
                ReleaseDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                MovieName = "Second Movie",
                Price = 10,
                Quantity = 1
            };

            var expectedId = 2;

            _movieRepository.Setup(m => m.AddMovie(It.IsAny<Movie>()))
                .Callback(() =>
                {
                    _movies.Add(new Movie()
                    {
                        Actors = movieRequest.Actors,
                        DirectorId = movieRequest.DirectorId,
                        ReleaseDate = movieRequest.ReleaseDate,
                        LastUpdated = movieRequest.LastUpdated,
                        MovieName = movieRequest.MovieName,
                        Price = movieRequest.Price,
                        Quantity = movieRequest.Quantity
                    });
                }).ReturnsAsync(() => _movies.FirstOrDefault(x => x.Id == expectedId));

            _movieRepository.Setup(m => m.GetMovieByName(movieRequest.MovieName))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.MovieName == movieRequest.MovieName));

            _directorRepository.Setup(x => x.GetDirectorById(movieRequest.DirectorId))
                .ReturnsAsync(_directors.FirstOrDefault(x => x.Id == expectedId));

            //Inject
            var command = new AddMovieCommand(movieRequest);
            var handlerAddMovie =
                new AddMovieCommandHandler(_movieRepository.Object, _mapper, _loggerAddMovieCommandHandler.Object, _directorRepository.Object);

            //Act
            var result = await handlerAddMovie.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(expectedId, result.Movie.Id);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            Assert.Equal("Movie Already Exists!", result.Message);
        }

        [Fact]
        public async Task Movie_UpdateMovie_Ok()
        {
            //Set
            var movieRequest = new UpdateMovieRequest()
            {
                Id = 1,
                Actors = "some actors",
                DirectorId = 2,
                ReleaseDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                MovieName = "Movie Name Some Otherr",
                Price = 23,
                Quantity = 2
            };

            _movieRepository.Setup(x => x.GetMovieById(movieRequest.Id)).ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieRequest.Id));

            //Inject
            var command = new UpdateMovieCommand(movieRequest);
            var handlerAddMovie =
                new UpdateMovieCommandHandler(_movieRepository.Object, _mapper, _loggerUpdateMovieCommandHandler.Object);

            //Act
            var result = await handlerAddMovie.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal("Movie successfully updated.", result.Message);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_UpdateMovie_NotFound()
        {
            //Setup
            var movieRequest = new UpdateMovieRequest()
            {
                Id = 3,
                Actors = "some actors",
                DirectorId = 2,
                ReleaseDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                MovieName = "Movie Name Some Otherr",
                Price = 23,
                Quantity = 2
            };

            _movieRepository.Setup(x => x.GetMovieById(movieRequest.Id)).ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieRequest.Id));

            //Inject
            var command = new UpdateMovieCommand(movieRequest);
            var handlerAddMovie =
                new UpdateMovieCommandHandler(_movieRepository.Object, _mapper, _loggerUpdateMovieCommandHandler.Object);

            //Act
            var result = await handlerAddMovie.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal("Movie doesn't exists!", result.Message);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_DeleteMovie_Ok()
        {
            //Setup
            var movieId = 1;

            _movieRepository.Setup(m => m.GetMovieById(movieId))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieId));

            _movieRepository.Setup(m => m.DeleteMovie(movieId)).ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieId));

            //Inject
            var command = new DeleteMovieCommand(movieId);
            var handlerAddMovie =
                new DeleteMovieCommandHandler(_movieRepository.Object, _loggerDeleteMovieCommandHandler.Object);

            //Act
            var result = await handlerAddMovie.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(movieId, result.Movie.Id);
            Assert.Equal("You successfully deleted the movie.", result.Message);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_DeleteMovie_NotFound()
        {
            //Setup
            var movieId = 3;

            _movieRepository.Setup(m => m.GetMovieById(movieId))
                .ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieId));

            _movieRepository.Setup(m => m.DeleteMovie(movieId)).ReturnsAsync(_movies.FirstOrDefault(x => x.Id == movieId));

            //Inject
            var command = new DeleteMovieCommand(movieId);
            var handlerAddMovie =
                new DeleteMovieCommandHandler(_movieRepository.Object, _loggerDeleteMovieCommandHandler.Object);

            //Act
            var result = await handlerAddMovie.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal("The movie with this Id doesn't exists.", result.Message);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_AddMultipleMovies_Ok()
        {
            //Setup
            var movieCollection = new List<Movie>()
            {
                new Movie()
                {
                    Id = 3,
                    Actors = "some actors",
                    DirectorId = 2,
                    ReleaseDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    MovieName = "Movie Name Some 22",
                    Price = 23,
                    Quantity = 2
                },
                new Movie()
                {
                    Id = 4,
                    Actors = "some actors",
                    DirectorId = 2,
                    ReleaseDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    MovieName = "Movie 34",
                    Price = 23,
                    Quantity = 2
                }
            };

            var movieRequest = new AddMultipleMoviesRequest()
            {
                MovieCollection = movieCollection
            };

            var expectedResult = 1 > 0;

            _movieRepository.Setup(m => m.AddMultipleMovies(It.IsAny<IEnumerable<Movie>>()))
                .Callback(() =>
                {
                    _movies.Add(new Movie()
                    {
                        Id = 3,
                        Actors = movieCollection[0].Actors,
                        DirectorId = movieCollection[0].DirectorId,
                        ReleaseDate = movieCollection[0].ReleaseDate,
                        LastUpdated = movieCollection[0].LastUpdated,
                        MovieName = movieCollection[0].MovieName,
                        Price = movieCollection[0].Price,
                        Quantity = movieCollection[0].Quantity
                    });
                    _movies.Add(new Movie()
                    {
                        Id = 4,
                        Actors = movieCollection[1].Actors,
                        DirectorId = movieCollection[1].DirectorId,
                        ReleaseDate = movieCollection[1].ReleaseDate,
                        LastUpdated = movieCollection[1].LastUpdated,
                        MovieName = movieCollection[1].MovieName,
                        Price = movieCollection[1].Price,
                        Quantity = movieCollection[1].Quantity
                    });
                }).ReturnsAsync(true);

            //Inject
            var command = new AddMultipleMoviesCommand(movieRequest);
            var handlerAddMultipleMovies =
                new AddMultipleMoviesCommandHandler(_movieRepository.Object, _loggerAddMultipleMoviesCommandHandler.Object, _mapper);

            //Act
            var result = await handlerAddMultipleMovies.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(expectedResult, result.Result);
            Assert.Equal("You successfully added the collection.", result.Message);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_AddMultipleMovies_BadRequest()
        {
            //Setup
            var movieRequest = new AddMultipleMoviesRequest()
            {
                MovieCollection = null
            };

            var expectedResult = false;

            _movieRepository.Setup(m => m.AddMultipleMovies(It.IsAny<IEnumerable<Movie>>()))
                .ReturnsAsync(false);

            //Inject
            var command = new AddMultipleMoviesCommand(movieRequest);
            var handlerAddMultipleMovies =
                new AddMultipleMoviesCommandHandler(_movieRepository.Object, _loggerAddMultipleMoviesCommandHandler.Object, _mapper);

            //Act
            var result = await handlerAddMultipleMovies.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal(expectedResult, result.Result);
            Assert.Equal("The collection is empty.", result.Message);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_GetMoviesByDirectorId_Ok()
        {
            //Setup
            var directorId = 1;

            _movieRepository.Setup(m => m.GetMoviesByDirectorId(directorId))
                .ReturnsAsync(_movies.Where(x => x.DirectorId == directorId));

            //Inject
            var command = new GetMoviesByDirectorIdCommand(directorId);
            var handlerAddMultipleMovies =
                new GetMoviesByDirectorIdCommandHandler(_movieRepository.Object, _loggerGetMoviesByDirectorIdCommandHandler.Object);

            //Act
            var result = await handlerAddMultipleMovies.Handle(command, _tokenSource.Token);

            //Assert
            Assert.NotNull(result.MovieCollection);
            Assert.Equal("You successfully received the collection.", result.Message);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_GetMoviesByDirectorId_NotFound()
        {
            //Setup
            var directorId = 10;

            _movieRepository.Setup(m => m.GetMoviesByDirectorId(directorId))
                .ReturnsAsync(Enumerable.Empty<Movie>());

            //Inject
            var command = new GetMoviesByDirectorIdCommand(directorId);
            var handlerAddMultipleMovies =
                new GetMoviesByDirectorIdCommandHandler(_movieRepository.Object, _loggerGetMoviesByDirectorIdCommandHandler.Object);

            //Act
            var result = await handlerAddMultipleMovies.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal("Director with this Id doesn't exists.", result.Message);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact]
        public async Task Movie_GetMoviesByDirectorId_BadRequest()
        {
            //Setup
            var directorId = -1;

            _movieRepository.Setup(m => m.GetMoviesByDirectorId(directorId))
                .ReturnsAsync(_movies.Where(x => x.DirectorId == directorId));

            _directorRepository.Setup(d => d.GetDirectorById(directorId))
                .ReturnsAsync(_directors.FirstOrDefault(x => x.Id == directorId));

            //Inject
            var command = new GetMoviesByDirectorIdCommand(directorId);
            var handlerAddMultipleMovies =
                new GetMoviesByDirectorIdCommandHandler(_movieRepository.Object, _loggerGetMoviesByDirectorIdCommandHandler.Object);

            //Act
            var result = await handlerAddMultipleMovies.Handle(command, _tokenSource.Token);

            //Assert
            Assert.Equal("Id must be greater than 0.", result.Message);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
        }
    }
}