using SmartEnergyHub.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartEnergyHub.DAL.Entities
{
    public class DeviceInfo
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsConnected { get; set; }
        public bool IsAutonomous { get; set; }
        public DateTime LastAccessTime { get; set; }
        public RoomType RoomType { get; set; }

        public Device Device { get; set; }
    }
}
