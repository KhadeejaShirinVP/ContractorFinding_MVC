namespace MVC.Models
{
    public class ContractorGetModule
    {
        public int? ContractorId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string? Gender { get; set; }

        public string License { get; set; } = null!;

        public string? Services { get; set; }

        public double? Lattitude { get; set; }

        public double? Longitude { get; set; }

        public int? Pincode { get; set; }

        public long? PhoneNumber { get; set; }
    }
}
