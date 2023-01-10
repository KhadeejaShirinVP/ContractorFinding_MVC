namespace MVC.Models
{
    public class CustomerGetModule
    {
        public double LandSqft { get; set; }

        public string RegistrationNo { get; set; } = null!;

        public string? BuildingType { get; set; }

        public double? Lattitude { get; set; }

        public double? Longitude { get; set; }

        public int Pincode { get; set; }

        public int? CustomerId { get; set; }

    }
}
