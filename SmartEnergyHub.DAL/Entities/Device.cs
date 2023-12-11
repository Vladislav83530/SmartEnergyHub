using SmartEnergyHub.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartEnergyHub.DAL.Entities
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string AccessToken { get; set; }
        public string MACAddress { get; set; }
        public DeviceType DeviceType { get; set; }

        public int ResidenceId { get; set; }
        public Residence Residence { get; set; }
        public int DeviceInfoId { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public AutonomousDevice AutonomousDevice { get; set; }
        public IEnumerable<ActivitySession> ActivitySessions { get; set; }
    }
}
