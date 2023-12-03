using Microsoft.AspNetCore.Identity;

namespace SmartEnergyHub.DAL.Entities
{
    public class Customer : IdentityUser
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string? Region {  get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? FlatNumber { get; set; }
    }
}
