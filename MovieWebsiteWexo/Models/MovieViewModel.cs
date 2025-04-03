namespace MovieWebsiteWexo.Models
{
    /// <summary>
    /// Represents the view model for movies, containing the list of genres and random movie suggestions.
    /// </summary>
    public class MovieViewModel
    {
        /// Used to display movies organized by genre.
        public List<Genre> MovieGenres { get; set; }

        /// Used to display a random selection of movie suggestions.
        public List<Movie> RandomMovies { get; set; }
    }
}
