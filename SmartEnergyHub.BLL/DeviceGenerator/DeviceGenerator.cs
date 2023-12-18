using SmartEnergyHub.BLL.DeviceGenerator.Abstract;
using SmartEnergyHub.DAL.EF;
using SmartEnergyHub.DAL.Entities;
using SmartEnergyHub.DAL.Entities.Enums;

namespace SmartEnergyHub.BLL.DeviceGenerator
{
    public class DeviceGenerator : IDeviceGenerator
    {
        private readonly ApplicationDbContext _context;

        public DeviceGenerator(ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateRandomDevices(
            int residenceId,
            Dictionary<DeviceType, List<string>> deviceNameData,
            List<string> serialNumbers,
            List<string> accessTokens,
            List<string> macAddresses,
            List<RoomType> rooms)
        {
            List<Device> devices = new List<Device>();

            for (var i = 0; i < new Random().Next(5, 25); i++)
            {
                devices.Add(GenerateDevice(residenceId, deviceNameData, serialNumbers, accessTokens, macAddresses, rooms));
            }

            await _context.Devices.AddRangeAsync(devices);
            await _context.SaveChangesAsync();
        }

        private Device GenerateDevice(
            int residenceId,
            Dictionary<DeviceType, List<string>> deviceNameData, 
            List<string> serialNumbers, 
            List<string> accessTokens,
            List<string> macAddresses,
            List<RoomType> rooms)
        {
            Device device = new Device();
            DeviceInfo deviceInfo = new DeviceInfo();

            device.DeviceInfo = deviceInfo;
            device.ResidenceId = residenceId;
            device.DeviceType = GetRandomKey<DeviceType>(deviceNameData);
            device.Name = GetRandomElement(deviceNameData[device.DeviceType]);
            device.SerialNumber = GetRandomElement(serialNumbers);
            device.AccessToken = GetRandomElement(accessTokens);
            device.MACAddress = GetRandomElement(macAddresses);
            device.DeviceInfo.IsActive = false;
            device.DeviceInfo.IsConnected = true;
            device.DeviceInfo.IsAutonomous = GetRandomBoolean();
            device.DeviceInfo.LastAccessTime = DateTime.UtcNow;
            device.DeviceInfo.RoomType = GetRandomElement(rooms);

            return device;
        }
          
        private T GetRandomKey<T>(Dictionary<T, List<string>> dictionary)
        {
            List<T> keys = new List<T>(dictionary.Keys);
            int randomIndex = new Random().Next(keys.Count);
            return keys[randomIndex];
        }

        private T GetRandomElement<T>(List<T> list)
        {
            int randomIndex = new Random().Next(list.Count);
            return list[randomIndex];
        }

        private bool GetRandomBoolean()
        {
            return new Random().Next(2) == 0;
        }
    }
}
