using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryBrick.Models
{
    public class ConsumerType
    {
        public const int houseId = 1;
        public const int plantId = 2;

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dependence { get; set; }
        public string Description { get; set; }
    }
}
