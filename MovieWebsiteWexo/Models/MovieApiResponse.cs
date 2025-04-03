using Newtonsoft.Json;

namespace MovieWebsiteWexo.Models
{
    /// <summary>
    /// Represents the response from the movie API, including the movie results, total results, and total pages.
    /// </summary>
    public class MovieApiResponse
    {
        public List<Movie> Results { get; set; }

        // This attribute specifies that when deserializing JSON data, the value of "total_results" from the JSON
        // should be mapped to the "TotalResults" property in the C# class.
        // This means that when the JSON response from the API contains a field named "total_results", it will be
        // assigned to the "TotalResults" property in the object. This is useful when the field name in the JSON
        // doesn't match the property name in the C# class.
        [JsonProperty("total_results")]
        public int TotalResults { get; set; } 
        public int TotalPages { get; set; }
    }
}
