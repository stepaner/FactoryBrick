using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryBrick.Models
{
    public class ConsumptionData
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ConsumerId { get; set; }
       
        public Consumer? Consumer { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(26,16)")]
        public decimal Consumption { get; set; }
        [Column(TypeName = "decimal(26,16)")]
        public decimal Dependence { get; set; }
        [NotMapped]
        public decimal Price
        {           
            set => Dependence = value;
        }
        [NotMapped]
        public decimal Weather
        {           
            set => Dependence = value;
        }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ImportDateTime { get; set; }        
    }
}
 