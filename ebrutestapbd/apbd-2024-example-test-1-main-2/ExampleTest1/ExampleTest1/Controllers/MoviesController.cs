using Microsoft.AspNetCore.Mvc;
using ExampleTest2.Models;
using ExampleTest2.Services;

namespace ExampleTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] DateTime? releaseDate = null)
        {
            var movies = await _movieService.GetMovies(releaseDate);
            return Ok(movies);
        }

        [HttpPost("assign-actor")]
        public async Task<IActionResult> AssignActorToMovie([FromBody] NewMovieActorDto newMovieActor)
        {
            try
            {
                await _movieService.AssignActorToMovie(newMovieActor);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}