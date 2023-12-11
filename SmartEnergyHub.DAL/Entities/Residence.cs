using SmartEnergyHub.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartEnergyHub.DAL.Entities
{
    public class Residence
    {
        [Key]
        public int Id { get; set; }
        public int DeviceCount { get; set; }
        public int RoomCount { get; set; }
        public double Area { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; }

        public int ResidenceLocationId { get; set; }
        public ResidenceLocation ResidenceLocation { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Device> Devices { get; set; }
    }
}
