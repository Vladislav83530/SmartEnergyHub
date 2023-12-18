using SmartEnergyHub.DAL.Entities.Enums;

namespace SmartEnergyHub.BLL.Residence_.Models
{
    public class ResidenceResponseModel
    {
        public int Id { get; set; }
        public int DeviceCount { get; set; }
        public int RoomCount { get; set; }
        public double Area { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? FlatNumber { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; }
    }
}
