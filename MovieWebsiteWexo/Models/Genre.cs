namespace MovieWebsiteWexo.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MovieCount { get; set; }
        public int CurrentPage { get; set; }  // Den aktuelle side
        public int TotalPages { get; set; }  // Totalt antal sider for denne genre
        public List<Movie> Movies { get; set; }
        public int TotalCount { get; set; }
    }
}
