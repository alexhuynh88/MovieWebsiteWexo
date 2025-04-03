using MovieWebsiteWexo.Models;

namespace MovieWebsiteWexo.ServiceLayer
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync(int page = 1);
        Task<MovieApiResponse> GetMoviesByGenreAsync(int genreId, int page = 1);
        Task<Movie> GetMovieDetailsAsync(int movieId);
        Task<List<Movie>> GetRandomMoviesAsync(int numberOfMovies = 5);

    }
}
