using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Device_.Abstract;
using SmartEnergyHub.BLL.Device_.Models;
using SmartEnergyHub.DAL.EF;

namespace SmartEnergyHub.BLL.Device_
{
    public class DeviceService : IDeviceService
    {
        private readonly ApplicationDbContext _context;

        public DeviceService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<DeviceResponseModel>> GetDevicesAsync(int residenceId, FilterModel filter, PaginationModel pagination)
        {
            var query = _context.Devices
                .Include(x=>x.DeviceInfo)
                .Where(x =>x.ResidenceId == residenceId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(d => d.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.SerialNumber))
            {
                query = query.Where(d => d.SerialNumber.Contains(filter.SerialNumber));
            }

            if (filter.DeviceType.HasValue)
            {
                query = query.Where(d => d.DeviceType == filter.DeviceType);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(d => d.DeviceInfo.IsActive == filter.IsActive);
            }

            if (filter.IsConnected.HasValue)
            {
                query = query.Where(d => d.DeviceInfo.IsConnected == filter.IsConnected);
            }

            if (filter.LastAccessTime.HasValue)
            {
                query = query.Where(d => d.DeviceInfo.LastAccessTime >= filter.LastAccessTime);
            }

            if (filter.RoomType.HasValue)
            {
                query = query.Where(d => d.DeviceInfo.RoomType == filter.RoomType);
            }

            var result = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(device => new DeviceResponseModel
                {
                    Id = device.Id,
                    Name = device.Name,
                    SerialNumber = device.SerialNumber,
                    AccessToken = device.AccessToken,
                    MACAddress = device.MACAddress,
                    DeviceType = device.DeviceType,
                    IsActive = device.DeviceInfo.IsActive,
                    IsConnected = device.DeviceInfo.IsConnected,
                    IsAutonomous = device.DeviceInfo.IsAutonomous,
                    LastAccessTime = device.DeviceInfo.LastAccessTime,
                    RoomType = device.DeviceInfo.RoomType
                })
                .ToListAsync();

            return result;
        }
    }
}
