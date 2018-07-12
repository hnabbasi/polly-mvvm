using Newtonsoft.Json;
namespace PollyMVVM.Models
{
    public class CountriesResponse
    {
        [JsonProperty("result")]
        public Country[] Countries { get; set; }
    }
}