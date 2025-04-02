using Newtonsoft.Json;
using System.IO;

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
        
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        public string Poster_path { get; set; }
        public string Backdrop_path { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Director> Directors { get; set; }

    }
}
