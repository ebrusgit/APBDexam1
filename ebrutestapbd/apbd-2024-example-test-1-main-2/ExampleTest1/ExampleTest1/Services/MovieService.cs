using ExampleTest2.Models;
using ExampleTest2.Repositories;

namespace ExampleTest2.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMoviesRepository _moviesRepository;

        public MovieService(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        public Task<IEnumerable<MovieDto>> GetMovies(DateTime? releaseDate)
        {
            return _moviesRepository.GetMovies(releaseDate);
        }

        public Task AssignActorToMovie(NewMovieActorDto newMovieActor)
        {
            return _moviesRepository.AssignActorToMovie(newMovieActor);
        }
    }
}