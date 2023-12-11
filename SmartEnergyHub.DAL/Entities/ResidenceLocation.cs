using System.ComponentModel.DataAnnotations;

namespace SmartEnergyHub.DAL.Entities
{
    public class ResidenceLocation
    {
        [Key]
        public int Id { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? FlatNumber { get; set; }

        public Residence Residence { get; set; }
    }
}
