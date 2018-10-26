using System.Collections.Generic;
using Newtonsoft.Json;

namespace openhack.k8s.api.Models
{
    public class OutputModel
    {
        public OutputModel()
        {

        }

        [JsonProperty("name")]
        public string Nome { get; set; }

        [JsonProperty("endpoints")]
        public List<EnderecoModel> Enderecos { get; set; }
    }
}