namespace ExampleTest2.Models
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public AgeRatingDto AgeRating { get; set; } = null!;
        public List<MovieActorDto> Actors { get; set; } = new List<MovieActorDto>();
    }

    public class AgeRatingDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ActorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }

    public class MovieActorDto
    {
        public ActorDto Actor { get; set; } = null!;
        public string CharacterName { get; set; } = string.Empty;
    }

    public class NewMovieActorDto
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
    }
}