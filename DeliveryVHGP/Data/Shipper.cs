using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryVHGP.Data
{
    //   [Table("")]
    public class Shipper
    {
        [Key]
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
