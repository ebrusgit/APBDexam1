using ExampleTest2.Models;

namespace ExampleTest2.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetMovies(DateTime? releaseDate);
        Task AssignActorToMovie(NewMovieActorDto newMovieActor);
    }
}