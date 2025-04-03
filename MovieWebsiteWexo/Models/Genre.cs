namespace MovieWebsiteWexo.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MovieCount { get; set; }
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; } 
        public List<Movie> Movies { get; set; }
        public int TotalCount { get; set; }
    }
}
