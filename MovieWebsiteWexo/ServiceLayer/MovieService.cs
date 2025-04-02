using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MovieWebsiteWexo.Models;
using Newtonsoft.Json;
//using System.Text.Json;

namespace MovieWebsiteWexo.ServiceLayer
{
    public class MovieService 
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiSettings:apiUrl"];
            _apiKey = configuration["ApiSettings:apiKey"];
        }

        public async Task<List<Movie>> GetMoviesAsync(int page = 1)
        {
            var url = $"{_apiUrl}discover/movie?api_key={_apiKey}&language=en-US&page={page}";
            Console.WriteLine($"Fetching movies from: {url}");
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                
                var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(response);
                Console.WriteLine($"Movies fetched: {movieApiResponse?.Results?.Count ?? 0}");
                return movieApiResponse?.Results ?? new List<Movie>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Movie>();
            }


        }


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
                Console.WriteLine($"Error fetching movies by genre: {ex.Message}");
                return new MovieApiResponse { Results = new List<Movie>(), TotalResults = 0, TotalPages = 0 };
            }
        }

        public async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            var url = $"{_apiUrl}movie/{movieId}?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var movieDetails = JsonConvert.DeserializeObject<Movie>(content);
            Console.WriteLine($"Parsed Language: {movieDetails.OriginalLanguage}");

            var actorsUrl = $"{_apiUrl}movie/{movieId}/credits?api_key={_apiKey}";
            var actorsResponse = await _httpClient.GetAsync(actorsUrl);
            if (actorsResponse.IsSuccessStatusCode)
            {
                var actorsContent = await actorsResponse.Content.ReadAsStringAsync();
                var actorsData = JsonConvert.DeserializeObject<dynamic>(actorsContent);
                movieDetails.Actors = actorsData.cast.ToObject<List<Actor>>();  // Skuespillere er ofte i 'cast' feltet
            }

            // Hent instruktører
            var directorsUrl = $"{_apiUrl}movie/{movieId}/credits?api_key={_apiKey}";
            var directorsResponse = await _httpClient.GetAsync(directorsUrl);
            if (directorsResponse.IsSuccessStatusCode)
            {
                var directorsContent = await directorsResponse.Content.ReadAsStringAsync();
                var directorsData = JsonConvert.DeserializeObject<dynamic>(directorsContent);
                movieDetails.Directors = directorsData.crew.ToObject<List<Director>>();
            }

            return movieDetails;
        }

        public async Task<List<Movie>> GetRandomMoviesAsync(int numberOfMovies = 5)
        {
            var allMovies = new List<Movie>();
            int page = 1;

            // Hent film i batches af 20
            while (allMovies.Count < numberOfMovies)
            {
                var movies = await GetMoviesAsync(page);
                allMovies.AddRange(movies);
                page++;
            }

            // Vælg tilfældige film fra listen
            var randomMovies = allMovies.OrderBy(x => Guid.NewGuid()).Take(numberOfMovies).ToList();
            return randomMovies;
        }
    }
}
