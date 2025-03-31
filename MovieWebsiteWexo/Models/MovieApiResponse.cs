using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MovieWebsiteWexo.Models
{
    public class MovieApiResponse
    {
        public List<Movie> Results { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }  // Antal resultater
        public int TotalPages { get; set; }    // Antal sider
    }
}
