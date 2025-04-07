using Newtonsoft.Json;
using System.IO;

namespace MovieWebsiteWexo.Models
{
    public class Movie
    {
        public int Id { get; set; } 
        public string Title { get; set; }

        public string Overview { get; set; }
        
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        public string Poster_path { get; set; }
        public string Backdrop_path { get; set; }
        public List<Genre> Genres { get; set; }
        /// Used to display a random selection of movie suggestions.
        public List<Movie> RandomMovies { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Director> Directors { get; set; }

    }
}
