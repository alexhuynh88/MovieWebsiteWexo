using MovieWebsiteWexo.Models;
using MovieWebsiteWexo.ServiceLayer;

namespace MovieWebsiteWexo.BusinessLogic
{
    public class MovieBusinessLogic : IMovieBusinessLogic
    {
        /// <summary>
        /// Handles business logic for movie-related operations. Communicates with IMovieService to fetch data from the API.
        /// </summary>
        private readonly IMovieService _movieService;

        public MovieBusinessLogic(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Fetches a list of movies based on the desired page.
        /// </summary>
        /// <param name="page">The page of movies to fetch.</param>
        /// <returns>A list of movies on the specified page.</returns>
        public async Task<List<Movie>> GetMovies(int page)
        {
            var movies = await _movieService.GetMoviesAsync(page);
            return movies;
        }

        /// <summary>
        /// Fetches movies grouped by genres, with a maximum number of movies per genre.
        /// </summary>
        /// <param name="page">The page of movies to fetch. Default is 1.</param>
        /// <param name="moviesPerGenre">The number of movies to fetch per genre. Default is 6.</param>
        /// <returns>A list of genres, each containing a list of movies.</returns>
        public async Task<List<Genre>> GetGenresWithMoviesAsync(int page = 1)
        {
            var selectedGenres = GetPredefinedGenres();

            foreach (var genre in selectedGenres)
            {
                var moviePage = await _movieService.GetMoviesByGenreAsync(genre.Id, page);
                if (genre.Movies == null)
                {
                    genre.Movies = new List<Movie>();
                }
                genre.MovieCount = moviePage.TotalResults;

                if (page == 1)
                {
                    genre.Movies = moviePage.Results.Take(5).ToList();
                }
                else
                {
                    genre.Movies.AddRange(moviePage.Results);
                }
            }

            return selectedGenres;
        }

        /// <summary>
        /// Fetches movies for a specific genre based on the genre ID and desired page.
        /// </summary>
        /// <param name="genreId">The ID of the genre for which movies should be fetched.</param>
        /// <param name="page">The page of movies to fetch.</param>
        /// <returns>A MovieApiResponse containing the movies and additional information such as TotalResults and TotalPages.</returns>
        public async Task<MovieApiResponse> GetMoviesByGenreAsync(int genreId, int page)
        {
            var movieResponse = await _movieService.GetMoviesByGenreAsync(genreId, page);

            if (!movieResponse.Results.Any())
            {
                return new MovieApiResponse
                {
                    Results = new List<Movie>(),
                    TotalResults = 0,
                    TotalPages = 0
                };
            }
            return movieResponse;
        }

        /// <summary>
        /// Returns a list of predefined genres.
        /// </summary>
        /// <returns>A list of genres with ID and name.</returns>
        public List<Genre> GetPredefinedGenres()
        {
            return new List<Genre>
    {
        new Genre { Id = 28, Name = "Action" },
        new Genre { Id = 35, Name = "Comedy" },
        new Genre { Id = 53, Name = "Thriller" },
        new Genre { Id = 10752, Name = "War" },
        new Genre { Id = 10749, Name = "Romance" },
        new Genre { Id = 18, Name = "Drama" },
        new Genre { Id = 80, Name = "Crime" },
        new Genre { Id = 99, Name = "Documentary" },
        new Genre { Id = 27, Name = "Horror" }
    };
        }

        /// <summary>
        /// Fetches details for a specific movie based on its ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie for which details should be fetched.</param>
        /// <returns>A Movie object with the movie's details.</returns>
        public async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            var movieDetails = await _movieService.GetMovieDetailsAsync(movieId);

            return movieDetails;
        }

        /// <summary>
        /// Fetches a list of random movies.
        /// </summary>
        /// <returns>A list of random movies.</returns>
        public async Task<List<Movie>> GetRandomMovies()
        {
            return await _movieService.GetRandomMoviesAsync(5);
        }

    }


}
