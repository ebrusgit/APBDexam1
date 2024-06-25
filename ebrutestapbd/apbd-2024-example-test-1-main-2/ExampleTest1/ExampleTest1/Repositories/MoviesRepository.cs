using System.Data.SqlClient;
using ExampleTest2.Models;
using Microsoft.Extensions.Configuration;

namespace ExampleTest2.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly IConfiguration _configuration;

        public MoviesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<MovieDto>> GetMovies(DateTime? releaseDate)
        {
            var movies = new List<MovieDto>();

            var query = @"
                SELECT 
                    m.IdMovie, m.Name, m.ReleaseDate, 
                    ar.IdRating, ar.Name AS RatingName,
                    a.IdActor, a.Name AS ActorName, a.Surname, 
                    am.CharacterName
                FROM Movie m
                JOIN AgeRating ar ON m.IdAgeRating = ar.IdRating
                LEFT JOIN Actor_Movie am ON m.IdMovie = am.IdMovie
                LEFT JOIN Actor a ON am.IdActor = a.IdActor
                WHERE (@ReleaseDate IS NULL OR m.ReleaseDate = @ReleaseDate)
                ORDER BY m.ReleaseDate DESC";

            await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ReleaseDate", releaseDate.HasValue ? (object)releaseDate.Value : DBNull.Value);

            await connection.OpenAsync();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var movieId = reader.GetInt32(reader.GetOrdinal("IdMovie"));
                var movie = movies.FirstOrDefault(m => m.Id == movieId);
                if (movie == null)
                {
                    movie = new MovieDto
                    {
                        Id = movieId,
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        ReleaseDate = reader.GetDateTime(reader.GetOrdinal("ReleaseDate")),
                        AgeRating = new AgeRatingDto
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("IdRating")),
                            Name = reader.GetString(reader.GetOrdinal("RatingName"))
                        },
                        Actors = new List<MovieActorDto>()
                    };
                    movies.Add(movie);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("IdActor")))
                {
                    movie.Actors.Add(new MovieActorDto
                    {
                        Actor = new ActorDto
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("IdActor")),
                            Name = reader.GetString(reader.GetOrdinal("ActorName")),
                            Surname = reader.GetString(reader.GetOrdinal("Surname"))
                        },
                        CharacterName = reader.GetString(reader.GetOrdinal("CharacterName"))
                    });
                }
            }

            return movies;
        }

        public async Task AssignActorToMovie(NewMovieActorDto newMovieActor)
        {
            var query = @"
                INSERT INTO Actor_Movie (IdMovie, IdActor, CharacterName) 
                VALUES (@IdMovie, @IdActor, @CharacterName)";

            await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@IdMovie", newMovieActor.MovieId);
            command.Parameters.AddWithValue("@IdActor", newMovieActor.ActorId);
            command.Parameters.AddWithValue("@CharacterName", newMovieActor.CharacterName);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
