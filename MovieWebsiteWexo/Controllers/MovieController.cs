using Microsoft.AspNetCore.Mvc;
using MovieWebsiteWexo.BusinessLogic;
using MovieWebsiteWexo.Models;
using MovieWebsiteWexo.ServiceLayer;

namespace MovieWebsiteWexo.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieBusinessLogic _movieBusinessLogic;

        public MovieController(MovieBusinessLogic movieBusinessLogic)
        {
            _movieBusinessLogic = movieBusinessLogic;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var movies = await _movieBusinessLogic.GetMovies(1);
            return View(movies);
        }

        public async Task<IActionResult> LoadMoreMovies(int page = 1)
        {
            var movies = await _movieBusinessLogic.GetMovies(page);
            return Json(movies); // Returnerer kun JSON til AJAX-kald
        }

        public async Task<IActionResult> MovieDetails(int movieId)
        {
            /// Hent filmens detaljer fra API'et
            var movie = await _movieBusinessLogic.GetMovieDetailsAsync(movieId);

            if (movie == null)
            {
                return NotFound();  // Hvis filmen ikke findes, returner NotFound.
            }

            ViewBag.IsInWishlist = IsMovieInWishlist(movieId);  // Tjek om filmen er i ønskelisten
            return View(movie);  // Returner filmen til viewet.
        }

        [HttpPost]
        public IActionResult AddToWishlist(int movieId)
        {
            // Hent ønskelisten fra sessionen
            var wishlist = HttpContext.Session.GetWishlist();

            // Hvis filmen ikke allerede er i ønskelisten, tilføj den
            if (!wishlist.Contains(movieId))
            {
                wishlist.Add(movieId);
                HttpContext.Session.SetWishlist(wishlist);
            }

            // Gå tilbage til filmens detaljeside
            return RedirectToAction("MovieDetails", new { movieId = movieId });
        }

        // Tjek om filmen er i ønskelisten
        public bool IsMovieInWishlist(int movieId)
        {
            var wishlist = HttpContext.Session.GetWishlist();
            return wishlist.Contains(movieId);
        }

        // Fjern film fra ønskelisten
        public IActionResult RemoveFromWishlist(int movieId)
        {
            // Brug sessionens metoder til at fjerne film
            HttpContext.Session.RemoveFromWishlist(movieId);
            return RedirectToAction("MovieDetails", new { movieId = movieId });
        }

        // Vis ønskelisten
        public async Task<IActionResult> Wishlist()
        {
            // Hent ønskelisten (film-IDs) fra sessionen
            var wishlistIds = HttpContext.Session.GetWishlist();

            // Hent filmene for de ønskede IDs
            var wishlistMovies = new List<Movie>();
            foreach (var movieId in wishlistIds)
            {
                var movie = await _movieBusinessLogic.GetMovieDetailsAsync(movieId);
                if (movie != null)
                {
                    wishlistMovies.Add(movie);
                }
            }
            return View(wishlistMovies);
        }
    }
}
