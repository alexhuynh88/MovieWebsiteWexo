namespace MovieWebsiteWexo.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
        public List<Genre> MovieGenres { get; set; }  // Liste af genres med film
        public List<Movie> RandomMovies { get; set; }
        public int TotalCount { get; set; }
    }
}
