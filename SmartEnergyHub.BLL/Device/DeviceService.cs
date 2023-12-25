using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Device_.Abstract;
using SmartEnergyHub.BLL.Device_.Models;
using SmartEnergyHub.DAL.EF;
using SmartEnergyHub.DAL.Entities;
using SmartEnergyHub.DAL.Entities.Enums;
using System.Globalization;

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

        public async Task UpdateDeviceStatus(int deviceId, bool isActive)
        {
            if (deviceId <= 0)
            {
                throw new ArgumentNullException(nameof(deviceId));
            }

            Device device = await this._context.Devices
                .Include(x=>x.DeviceInfo)
                .FirstOrDefaultAsync(x=>x.Id == deviceId);

            if (device != null)
            {
                device.DeviceInfo.IsActive = isActive;

                await this._context.SaveChangesAsync();
            }
        }

        public async Task<DeviceResponseModel> GetDevice(int deviceId)
        {
            if (deviceId <= 0)
            {
                throw new ArgumentNullException(nameof(deviceId));
            }

            DeviceResponseModel device = await this._context.Devices
                .Include(x => x.DeviceInfo)
                .Where(x => x.Id == deviceId)
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
                .FirstOrDefaultAsync();

            if (device != null)
            {
                return device;
            }

            return null;              
        }

        //public async Task<List<ActivitySession>> GetActivitySessions(int deviceId)
        //{
        //    if (deviceId <= 0)
        //    {
        //        throw new ArgumentNullException(nameof(deviceId));
        //    }

        //    List<ActivitySession> sessions = this._context.ActivitySessions
        //        .Where(x => x.DeviceId == deviceId)
        //        .ToList();

        //    var groupedByDay = sessions
        //        .GroupBy(x => x.TurnOnTime.Date)
        //        .Select(group =>
        //        {
        //            DateTime startOfDay = group.Key;
        //            DateTime endOfDay = group.Key.AddDays(1).AddSeconds(-1);
        //            double totalKWh = group.Sum(x => x.KWh);

        //            return new ActivitySession
        //            {
        //                TurnOnTime = startOfDay,
        //                TurnOffTime = endOfDay,
        //                KWh = totalKWh,
        //                DeviceId = group.First().DeviceId,
        //                Device = group.First().Device
        //            };
        //        })
        //        .ToList();

        //    return groupedByDay;
        //}

        public async Task<List<ActivitySession>> GetActivitySessions(int deviceId, Period groupingType)
        {
            if (deviceId <= 0)
            {
                throw new ArgumentNullException(nameof(deviceId));
            }

            List<ActivitySession> sessions = this._context.ActivitySessions
                .Where(x => x.DeviceId == deviceId)
                .ToList();

            List<ActivitySession> groupedSessions = new List<ActivitySession>();

            switch (groupingType)
            {
                case Period.Day:
                    groupedSessions = GroupByDay(sessions);
                    break;
                case Period.Week:
                    groupedSessions = GroupByWeek(sessions);
                    break;
                case Period.Month:
                    groupedSessions = GroupByMonth(sessions);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(groupingType), "Invalid grouping type.");
            }

            return groupedSessions;
        }

        private List<ActivitySession> GroupByDay(List<ActivitySession> sessions)
        {
            var groupedByDay = sessions
                .GroupBy(x => x.TurnOnTime.Date)
                .Select(group =>
                {
                    DateTime startOfDay = group.Key;
                    DateTime endOfDay = group.Key.AddDays(1).AddSeconds(-1);
                    double totalKWh = group.Sum(x => x.KWh);

                    return new ActivitySession
                    {
                        TurnOnTime = startOfDay,
                        TurnOffTime = endOfDay,
                        KWh = totalKWh,
                        DeviceId = group.First().DeviceId,
                        Device = group.First().Device
                    };
                })
                .ToList();

            return groupedByDay;
        }

        private List<ActivitySession> GroupByWeek(List<ActivitySession> sessions)
        {
            var groupedByWeek = sessions
                .GroupBy(x => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.TurnOnTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                .Select(group =>
                {
                    DateTime startDateOfWeek = GetStartOfWeek(group.First().TurnOnTime);
                    DateTime endDateOfWeek = startDateOfWeek.AddDays(7).AddSeconds(-1);
                    double totalKWh = group.Sum(x => x.KWh);

                    return new ActivitySession
                    {
                        TurnOnTime = startDateOfWeek,
                        TurnOffTime = endDateOfWeek,
                        KWh = totalKWh,
                        DeviceId = group.First().DeviceId,
                        Device = group.First().Device
                    };
                })
                .ToList();

            return groupedByWeek;
        }

        private List<ActivitySession> GroupByMonth(List<ActivitySession> sessions)
        {
            var groupedByMonth = sessions
                .GroupBy(x => new { x.TurnOnTime.Year, x.TurnOnTime.Month })
                .Select(group =>
                {
                    DateTime startDateOfMonth = new DateTime(group.Key.Year, group.Key.Month, 1);
                    DateTime endDateOfMonth = startDateOfMonth.AddMonths(1).AddSeconds(-1);
                    double totalKWh = group.Sum(x => x.KWh);

                    return new ActivitySession
                    {
                        TurnOnTime = startDateOfMonth,
                        TurnOffTime = endDateOfMonth,
                        KWh = totalKWh,
                        DeviceId = group.First().DeviceId,
                        Device = group.First().Device
                    };
                })
                .ToList();

            return groupedByMonth;
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

    }
}
