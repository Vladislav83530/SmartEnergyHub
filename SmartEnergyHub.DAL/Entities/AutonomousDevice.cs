using System.ComponentModel.DataAnnotations;

namespace SmartEnergyHub.DAL.Entities
{
    public class AutonomousDevice
    {
        [Key]
        public int Id { get; set; }
        public int BatteryLevel { get; set; }
        public TimeSpan TimeUntilDischarge {get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
