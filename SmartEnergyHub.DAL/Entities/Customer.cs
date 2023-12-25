using Microsoft.AspNetCore.Identity;

namespace SmartEnergyHub.DAL.Entities
{
    public class Customer : IdentityUser
    {
        public string FistName { get; set; }
        public string LastName { get; set; }

        public Residence Residence { get; set; }
    }
}
