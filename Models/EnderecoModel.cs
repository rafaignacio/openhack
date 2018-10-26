using Newtonsoft.Json;

namespace openhack.k8s.api.Models
{
    public class EnderecoModel
    {
        public EnderecoModel()
        {

        }

        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("name")]
        public string Nome { get; set; }
        [JsonProperty("online")]
        public string Online { get; set; }
        [JsonProperty("max_cap")]
        public string MaxCapacity { get; set; }

        [JsonProperty("ports")]
        public string Ports { get; set; }
    }
}