using MovieWebsiteWexo.Models;
using System.Threading.Tasks;

namespace MovieWebsiteWexo.BusinessLogic
{
    public interface IMovieBusinessLogic
    {
        Task<List<Movie>> GetMovies(int page);
        Task<List<Genre>> GetGenresWithMoviesAsync(int page = 1);
        Task<MovieApiResponse> GetMoviesByGenreAsync(int genreId, int page);
        List<Genre> GetPredefinedGenres();
        Task<Movie> GetMovieDetailsAsync(int movieId);
        Task<List<Movie>> GetRandomMovies();
    }
}
