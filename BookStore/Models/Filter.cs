using BookStore.DAL.Models;
using System.Text.Json.Serialization;

namespace BookStore.Models
{
    #nullable enable
    public class Filter
    {
        [JsonPropertyName("searchRequest")]
        public string SearchRequest { get; set; } = "";
        [JsonPropertyName("inclGenres")]
        public int[] IncludingGenres { get; set; } = new int[0];
        [JsonPropertyName("exclGenres")]
        public int[] ExcludingGenres { get; set; } = new int[0];
    }
}
