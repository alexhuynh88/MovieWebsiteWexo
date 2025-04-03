using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MovieWebsiteWexo.Models;
using Newtonsoft.Json;

namespace MovieWebsiteWexo.ServiceLayer
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for API requests.</param>
        /// <param name="configuration">Configuration to retrieve API settings.</param>
        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiSettings:apiUrl"];
            _apiKey = configuration["ApiSettings:apiKey"];
        }

        /// <summary>
        /// Retrieves a list of movies from the API.
        /// </summary>
        /// <param name="page">The page number for paginated results (default is 1).</param>
        /// <returns>A list of movies or an empty list if an error occurs.</returns>
        public async Task<List<Movie>> GetMoviesAsync(int page = 1)
        {
            var url = $"{_apiUrl}discover/movie?api_key={_apiKey}&language=en-US&page={page}";
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                
                var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(response);
                return movieApiResponse?.Results ?? new List<Movie>();

            }
            catch (Exception ex)
            {
                return new List<Movie>();
            }


        }

        /// <summary>
        /// Retrieves a list of movies filtered by genre.
        /// </summary>
        /// <param name="genreId">The genre ID to filter movies.</param>
        /// <param name="page">The page number for paginated results (default is 1).</param>
        /// <returns>A <see cref="MovieApiResponse"/> containing movies of the specified genre.</returns>
        public async Task<MovieApiResponse> GetMoviesByGenreAsync(int genreId, int page = 1)
        {
            var url = $"{_apiUrl}discover/movie?api_key={_apiKey}&with_genres={genreId}&page={page}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(response);

                return movieApiResponse ?? new MovieApiResponse { Results = new List<Movie>()};
            }
            catch (Exception ex)
            {
                return new MovieApiResponse { Results = new List<Movie>(), TotalResults = 0, TotalPages = 0 };
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>A <see cref="Movie"/> object containing movie details, or null if not found.</returns>
        public async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            var url = $"{_apiUrl}movie/{movieId}?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var movieDetails = JsonConvert.DeserializeObject<Movie>(content);

            var castCrewUrl = $"{_apiUrl}movie/{movieId}/credits?api_key={_apiKey}";

            var actorsResponse = await _httpClient.GetAsync(castCrewUrl);
            if (actorsResponse.IsSuccessStatusCode)
            {
                var actorsContent = await actorsResponse.Content.ReadAsStringAsync();
                var actorsData = JsonConvert.DeserializeObject<dynamic>(actorsContent);
                movieDetails.Actors = actorsData.cast.ToObject<List<Actor>>(); 
            }

            var directorsResponse = await _httpClient.GetAsync(castCrewUrl);
            if (directorsResponse.IsSuccessStatusCode)
            {
                var directorsContent = await directorsResponse.Content.ReadAsStringAsync();
                var directorsData = JsonConvert.DeserializeObject<dynamic>(directorsContent);
                movieDetails.Directors = directorsData.crew.ToObject<List<Director>>();
            }

            return movieDetails;
        }

        /// <summary>
        /// Retrieves a random selection of movies.
        /// </summary>
        /// <param name="numberOfMovies">The number of random movies to retrieve (default is 5).</param>
        /// <returns>A list of randomly selected movies.</returns>
        public async Task<List<Movie>> GetRandomMoviesAsync(int numberOfMovies = 5)
        {
            var allMovies = new List<Movie>();
            int page = 1;

            while (allMovies.Count < numberOfMovies)
            {
                var movies = await GetMoviesAsync(page);
                allMovies.AddRange(movies);
                page++;
            }
            var randomMovies = allMovies.OrderBy(x => Guid.NewGuid()).Take(numberOfMovies).ToList();
            return randomMovies;
        }
    }
}
