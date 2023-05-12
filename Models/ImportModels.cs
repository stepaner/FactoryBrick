using FactoryBrick.Data;
using FactoryBrick.Models;
using Newtonsoft.Json;

namespace FactoryBrick
{
    public class JsonRoot
    {
        [JsonProperty("houses")]
        public List<Consumer> Houses { get; set; }
        [JsonProperty("plants")]
        public List<Consumer> Plants { get; set; }
    }
}
