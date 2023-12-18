using SmartEnergyHub.DAL.Entities.Enums;

namespace SmartEnergyHub.API.Settings
{
    public class DeviceGeneratorSettings
    {
        public Dictionary<DeviceType, List<string>> DeviceNameData { get; set; }
        public List<string> SerialNumbers { get; set; }
        public List<string> AccessTokens { get; set; }
        public List<string> MACAddresses { get; set; }
        public List<RoomType> Rooms { get; set; }
    }
}
