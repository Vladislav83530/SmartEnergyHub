using SmartEnergyHub.BLL.Device_.Models;

namespace SmartEnergyHub.BLL.Device_.Abstract
{
    public interface IDeviceService
    {
        Task<List<DeviceResponseModel>> GetDevicesAsync(int residenceId, FilterModel filter, PaginationModel pagination);
        Task UpdateDeviceStatus(int deviceId, bool isActive);
    }
}
