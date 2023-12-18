using SmartEnergyHub.DAL.Entities.Enums;

namespace SmartEnergyHub.BLL.DeviceGenerator.Abstract
{
    public interface IDeviceGenerator
    {
        Task CreateRandomDevices(
            int residenceId,
            Dictionary<DeviceType, List<string>> deviceNameData,
            List<string> serialNumbers,
            List<string> accessTokens,
            List<string> macAddresses,
            List<RoomType> rooms);
    }
}
