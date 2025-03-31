using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieWebsiteWexo.BusinessLogic;
using MovieWebsiteWexo.Models;
using System.Diagnostics;

namespace MovieWebsiteWexo.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieBusinessLogic _movieBusinessLogic;

        public HomeController(MovieBusinessLogic movieBusinessLogic)
        {
            _movieBusinessLogic = movieBusinessLogic;
        }
        //[HttpGet("Movie/Genre")]
        public async Task<IActionResult> Index(int genreId, int page = 1)
        {
            var moviesByGenres = await _movieBusinessLogic.GetGenresWithMoviesAsync(page);

            var viewModel = new MovieViewModel
            {
                MovieGenres = moviesByGenres
            };

            return View(viewModel.MovieGenres);
        }

        //[HttpGet("Movie/GenreDetails/{genreId}")]
        public async Task<IActionResult> GenreDetails(int genreId, string genreName, int page = 1)
        {
            
            var movieResponse = await _movieBusinessLogic.GetMoviesByGenreAsync(genreId, page);

            if (movieResponse == null || !movieResponse.Results.Any())
            {
                return View("Error"); // Eller en side, der viser en besked om manglende film
            }

            // Hent prædefinerede genrer fra business logic-laget
            var selectedGenres = _movieBusinessLogic.GetPredefinedGenres().FirstOrDefault(g => g.Id == genreId);

            if (selectedGenres == null)
            {
                return View("Error"); // Hvis genren ikke findes, vis en fejlside
            }

            // Opdater Genre-modellen med filmdata
            selectedGenres.Movies = movieResponse.Results;
            selectedGenres.MovieCount = movieResponse.TotalResults;
            selectedGenres.TotalPages = movieResponse.TotalPages;
            selectedGenres.CurrentPage = page;


            return View(selectedGenres); // Returnerer genre med alle dens film
        }

        public async Task<IActionResult> LoadMoreMovies(int genreId, int page = 1)
        {
            Console.WriteLine($"LoadMoreMovies called with genreId: {genreId}, page: {page}");
            var movieResponse = await _movieBusinessLogic.GetMoviesByGenreAsync(genreId, page);

            if (movieResponse == null || movieResponse.Results.Count == 0)
            {
                Console.WriteLine("No movies found!");
                return Json(new { success = false, message = "Ingen flere film." });
            }

            // Log JSON-outputtet for debugging
            Console.WriteLine($"Returning {movieResponse.Results.Count} movies for genre {genreId} on page {page}");
            return Json(new { success = true, movies = movieResponse.Results });
        }

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
