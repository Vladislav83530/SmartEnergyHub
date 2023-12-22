using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Device_.Abstract;
using SmartEnergyHub.BLL.Device_.Models;
using SmartEnergyHub.DAL.EF;
using SmartEnergyHub.DAL.Entities.Enums;

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

            var parsedTypes = new List<DeviceType>();
            var parsedRoomTypes = new List<RoomType>();

            if (!string.IsNullOrEmpty(filter.DeviceType))
            {
                string[] types = filter.DeviceType.Split('-', StringSplitOptions.RemoveEmptyEntries);

                foreach (string type in types)
                {
                    if (Enum.TryParse(type, out DeviceType parsedType))
                    {
                        parsedTypes.Add(parsedType);
                    }
                }

                query = query.Where(d => parsedTypes.Contains(d.DeviceType));
            }

            if (!string.IsNullOrEmpty(filter.RoomType))
            {
                string[] types = filter.RoomType.Split('-', StringSplitOptions.RemoveEmptyEntries);

                foreach (string type in types)
                {
                    if (Enum.TryParse(type, out RoomType parsedRoomType))
                    {
                        parsedRoomTypes.Add(parsedRoomType);
                    }
                }

                query = query.Where(d => parsedRoomTypes.Contains(d.DeviceInfo.RoomType));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(d => d.DeviceInfo.IsActive == filter.IsActive);
            }

            if (filter.IsAutonomous.HasValue)
            {
                query = query.Where(d => d.DeviceInfo.IsAutonomous == filter.IsAutonomous);
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
                    RoomType = device.DeviceInfo.RoomType,
                })
                .ToListAsync();

            return result;
        }
    }
}
