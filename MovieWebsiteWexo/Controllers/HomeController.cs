using Microsoft.AspNetCore.Mvc;
using MovieWebsiteWexo.BusinessLogic;
using MovieWebsiteWexo.Models;
using System.Diagnostics;

namespace MovieWebsiteWexo.Controllers
{
    /// <summary>
    /// Controller responsible for handling requests related to movie genres, random movies, and displaying them in the views.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IMovieBusinessLogic _movieBusinessLogic;

        public HomeController(IMovieBusinessLogic movieBusinessLogic)
        {
            _movieBusinessLogic = movieBusinessLogic;
        }

        /// <summary>
        /// Retrieves movies grouped by genres and random movies, and passes them to the view.
        /// </summary>
        /// <param name="genreId">The ID of the genre to filter movies by.</param>
        /// <param name="page">The page number of movies to display. Default is 1.</param>
        /// <returns>An IActionResult containing the view with movies grouped by genre and random movies.</returns>
        public async Task<IActionResult> Index(int genreId, int page = 1)
        {
            var moviesByGenres = await _movieBusinessLogic.GetGenresWithMoviesAsync(page);
            var randomMovies = await _movieBusinessLogic.GetRandomMovies();

            var viewModel = new Movie
            {
                Genres = moviesByGenres,
                RandomMovies = randomMovies
            };

            return View(viewModel);
        }

        /// <summary>
        /// Retrieves movies for a specific genre and passes them to the view.
        /// </summary>
        /// <param name="genreId">The ID of the genre for which movies are fetched.</param>
        /// <param name="genreName">The name of the genre.</param>
        /// <param name="page">The page number to fetch movies for. Default is 1.</param>
        /// <returns>An IActionResult containing the view with the movies for the selected genre.</returns>
        [HttpGet("Movie/GenreDetails/{genreId}")]
        public async Task<IActionResult> GenreDetails(int genreId, string genreName, int page = 1)
        {
            
            var movieResponse = await _movieBusinessLogic.GetMoviesByGenreAsync(genreId, page);

            if (movieResponse == null || !movieResponse.Results.Any())
            {
                return View("Error"); 
            }

            var selectedGenres = _movieBusinessLogic.GetPredefinedGenres().FirstOrDefault(g => g.Id == genreId);

            if (selectedGenres == null)
            {
                return View("Error");
            }

            // Update the Genre model with movie data
            selectedGenres.Movies = movieResponse.Results;
            selectedGenres.MovieCount = movieResponse.TotalResults;
            
            int pageSize = 20;
            selectedGenres.TotalPages = (int)Math.Ceiling((double)movieResponse.TotalResults / pageSize);
            selectedGenres.CurrentPage = page;

            // Set ViewBag variables for pagination and results
            ViewBag.GenreName = genreName;
            ViewBag.GenreId = genreId; 
            ViewBag.TotalResults = movieResponse.TotalResults;
            ViewBag.TotalPages = (int)Math.Ceiling((double)movieResponse.TotalResults / pageSize);
            ViewBag.Page = page;

            return View(selectedGenres);
        }

        //TODO
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
