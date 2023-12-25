using SmartEnergyHub.BLL.Device_.Models;
using SmartEnergyHub.DAL.Entities;

namespace SmartEnergyHub.BLL.Device_.Abstract
{
    public interface IDeviceService
    {
        Task<List<DeviceResponseModel>> GetDevicesAsync(int residenceId, FilterModel filter, PaginationModel pagination);
        Task UpdateDeviceStatus(int deviceId, bool isActive);
        Task<DeviceResponseModel> GetDevice(int deviceId);
        Task<List<ActivitySession>> GetActivitySessions(int deviceId, Period period);
    }
}
