using ExampleTest2.Models;

namespace ExampleTest2.Repositories
{
    public interface IMoviesRepository
    {
        Task<IEnumerable<MovieDto>> GetMovies(DateTime? releaseDate);
        Task AssignActorToMovie(NewMovieActorDto newMovieActor);
    }
}