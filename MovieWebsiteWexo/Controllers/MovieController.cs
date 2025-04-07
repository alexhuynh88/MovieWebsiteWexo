using Microsoft.AspNetCore.Mvc;
using MovieWebsiteWexo.BusinessLogic;
using MovieWebsiteWexo.Models;

namespace MovieWebsiteWexo.Controllers
{
    /// <summary>
    /// Controller responsible for handling movie-related requests such as displaying movie details,
    /// managing a wishlist, and loading more movies.
    /// </summary>
    public class MovieController : Controller
    {
        private readonly IMovieBusinessLogic _movieBusinessLogic;

        public MovieController(IMovieBusinessLogic movieBusinessLogic)
        {
            _movieBusinessLogic = movieBusinessLogic;
        }

        /// <summary>
        /// Retrieves a list of movies and passes them to the view.
        /// </summary>
        /// <param name="page">The page number to fetch movies for. Default is 1.</param>
        /// <returns>An IActionResult containing the view with the list of movies.</returns>
        public async Task<IActionResult> Index(int page = 1)
        {
            var movies = await _movieBusinessLogic.GetMovies(1);
            return View(movies);
        }

        /// <summary>
        /// Retrieves more movies (via AJAX) and returns them as JSON.
        /// </summary>
        /// <param name="page">The page number to fetch additional movies for. Default is 1.</param>
        /// <returns>A JSON result containing the list of movies.</returns>
        public async Task<IActionResult> LoadMoreMovies(int page = 1)
        {
            var movies = await _movieBusinessLogic.GetMovies(page);
            return Json(movies); // Returns only JSON for AJAX calls
        }

        /// <summary>
        /// Retrieves details of a specific movie and passes them to the view.
        /// </summary>
        /// <param name="movieId">The ID of the movie to retrieve details for.</param>
        /// <returns>An IActionResult containing the view with movie details, or a 404 if the movie is not found.</returns>
        public async Task<IActionResult> MovieDetails(int movieId)
        {
            var movie = await _movieBusinessLogic.GetMovieDetailsAsync(movieId);

            if (movie == null)
            {
                return NotFound(); 
            }
            ViewBag.IsInWishlist = IsMovieInWishlist(movieId);
            return View(movie); 
        }

        /// <summary>
        /// Adds a movie to the user's wishlist and redirects to the movie details page.
        /// </summary>
        /// <param name="movieId">The ID of the movie to add to the wishlist.</param>
        /// <returns>An IActionResult that redirects to the MovieDetails view for the added movie.</returns>
        [HttpPost]
        public IActionResult AddToWishlist(int movieId)
        {
            var wishlist = HttpContext.Session.GetWishlist();

            if (!wishlist.Contains(movieId))
            {
                wishlist.Add(movieId);
                HttpContext.Session.SetWishlist(wishlist);
            }
            return RedirectToAction("MovieDetails", new { movieId = movieId });
        }

        /// <summary>
        /// Checks if a movie is in the user's wishlist.
        /// </summary>
        /// <param name="movieId">The ID of the movie to check for.</param>
        /// <returns>A boolean indicating whether the movie is in the wishlist.</returns>
        public bool IsMovieInWishlist(int movieId)
        {
            var wishlist = HttpContext.Session.GetWishlist();
            return wishlist.Contains(movieId);
        }

        /// <summary>
        /// Removes a movie from the user's wishlist and redirects to the movie details page.
        /// </summary>
        /// <param name="movieId">The ID of the movie to remove from the wishlist.</param>
        /// <returns>An IActionResult that redirects to the MovieDetails view for the removed movie.</returns>
        public IActionResult RemoveFromWishlist(int movieId)
        {
            HttpContext.Session.RemoveFromWishlist(movieId);
            return RedirectToAction("Wishlist");
        }

        /// <summary>
        /// Retrieves the user's wishlist and passes the list of movies to the view.
        /// </summary>
        /// <returns>An IActionResult containing the view with the user's wishlist movies.</returns>
        public async Task<IActionResult> Wishlist()
        {

            var wishlistIds = HttpContext.Session.GetWishlist();

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
