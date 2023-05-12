using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryBrick.Models
{
    public class Consumer
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [JsonProperty("ConsumerId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ConsumerTypeId { get; set; }
        public ConsumerType ConsumerType { get; set; }
        [JsonProperty("consumptions")]
        public List<ConsumptionData> Consumptions { get; set; } = new();       
    }
}
