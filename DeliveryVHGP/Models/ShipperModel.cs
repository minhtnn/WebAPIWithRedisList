namespace DeliveryVHGP.Models
{
    //Should not use entity directly, you can use model instead to validate, be secure
    public class ShipperModel
    {
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
