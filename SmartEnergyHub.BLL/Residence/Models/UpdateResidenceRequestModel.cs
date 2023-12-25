namespace SmartEnergyHub.BLL.Residence_.Models
{
    public class UpdateResidenceRequestModel
    {
        public int Id { get; set; }
        public int RoomCount { get; set; }
        public double Area { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? FlatNumber { get; set; }
    }
}
