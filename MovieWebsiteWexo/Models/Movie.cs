using Newtonsoft.Json;

namespace MovieWebsiteWexo.Models
{
    public class Movie
    {
        public int Id { get; set; } 
        public string Title { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Poster_path { get; set; }
        public string Backdrop_path { get; set; }
        //public List<int> GenreId { get; set; }
    }
}
