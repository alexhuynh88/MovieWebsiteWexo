using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieWebsiteWexo.BusinessLogic;
using MovieWebsiteWexo.Models;
using MovieWebsiteWexo.ServiceLayer;
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
        [HttpGet("Movie/GenreDetails/{genreId}")]
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
            // Beregn TotalPages baseret på TotalResults og pageSize
            int pageSize = 20; // Vælg en passende pageSize
            selectedGenres.TotalPages = (int)Math.Ceiling((double)movieResponse.TotalResults / pageSize);
            Console.WriteLine($"TotalResults: {movieResponse.TotalResults}");
            Console.WriteLine($"PageSize: {20}");
            Console.WriteLine($"Calculated TotalPages: {(int)Math.Ceiling((double)movieResponse.TotalResults / 20)}");
            selectedGenres.CurrentPage = page;

            // Sæt ViewBag-variabler for pagination og resultater
            ViewBag.GenreName = genreName; // Sæt genrenavn
            ViewBag.GenreId = genreId; // Sæt genreId
            ViewBag.TotalResults = movieResponse.TotalResults; // Sæt totalResults
            ViewBag.TotalPages = (int)Math.Ceiling((double)movieResponse.TotalResults / pageSize);
            ViewBag.Page = page; // Sæt den aktuelle side

            return View(selectedGenres); // Returnerer genre med alle dens film
        }

        public async Task<IActionResult> LoadMoreMovies(int genreId, int page = 1)
        {
            // Hent filmene for den genre og den ønskede side
            var movieApiResponse = await _movieBusinessLogic.GetMoviesByGenreAsync(genreId, page);
            Console.WriteLine($"Total Results: {movieApiResponse.TotalResults}");  // Debugger total results
            Console.WriteLine($"Results: {movieApiResponse.Results?.Count()}");

            // Hvis der ikke er nogen film
            if (movieApiResponse.Results == null || !movieApiResponse.Results.Any() || page > Math.Ceiling((double)movieApiResponse.TotalResults / 20))
            {
                return Json(new { success = false, message = "Ingen flere film" });
            }

            // Hvis der er film, send dem tilbage
            return Json(new
            {
                success = true,
                movies = movieApiResponse.Results,
                currentPage = page
            });
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
