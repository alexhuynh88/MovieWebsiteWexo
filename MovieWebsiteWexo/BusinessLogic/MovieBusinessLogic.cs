using MovieWebsiteWexo.Models;
using MovieWebsiteWexo.ServiceLayer;
using Newtonsoft.Json;

namespace MovieWebsiteWexo.BusinessLogic
{
    public class MovieBusinessLogic
    {
        private readonly MovieService _movieService;

        public MovieBusinessLogic(MovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<List<Movie>> GetMovies(int page)
        {
            var movies = await _movieService.GetMoviesAsync(page); // Passer page parameter videre til servicelaget
            return movies;
        }


        public async Task<List<Genre>> GetGenresWithMoviesAsync(int page = 1, int moviesPerGenre = 6)
        {
            var selectedGenres = GetPredefinedGenres();

            // Hent filmene for hver genre med pagination
            foreach (var genre in selectedGenres)
            {
                var moviePage = await _movieService.GetMoviesByGenreAsync(genre.Id, page);
                if (genre.Movies == null)
                {
                    genre.Movies = new List<Movie>();
                }
                // Sæt total film-antal i genren
                genre.MovieCount = moviePage.TotalResults;

                if (page == 1)
                {
                    genre.Movies = moviePage.Results.Take(moviesPerGenre).ToList();  // Begræns til 6 film på første side
                }
                else
                {
                    genre.Movies.AddRange(moviePage.Results); // Tilføj flere film til listen
                }
            }

            return selectedGenres;
        }

        public async Task<MovieApiResponse> GetMoviesByGenreAsync(int genreId, int page)
        {
            var movieResponse = await _movieService.GetMoviesByGenreAsync(genreId, page);

            //var selectedGenres = GetPredefinedGenres();

            //var genre = selectedGenres.FirstOrDefault(g => g.Id == genreId);

            if (!movieResponse.Results.Any())
            {
                return new MovieApiResponse
                {
                    Results = new List<Movie>(),
                    TotalResults = 0,
                    TotalPages = 0
                };
            }


            return movieResponse; // Returnér API-responsen direkte
        }

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

        public async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            // Hent filmens detaljer fra MovieService
            var movieDetails = await _movieService.GetMovieDetailsAsync(movieId);

            return movieDetails;
        }

        public async Task<List<Movie>> GetRandomMovies()
        {
            return await _movieService.GetRandomMoviesAsync(5);  // 5 tilfældige film
        }

    }


}
