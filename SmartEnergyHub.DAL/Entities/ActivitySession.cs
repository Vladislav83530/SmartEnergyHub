using SmartEnergyHub.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartEnergyHub.DAL.Entities
{
    public class ActivitySession
    {
        [Key]
        public int Id { get; set; }
        public double KWh {  get; set; }
        public DateTime TurnOnTime { get; set; }
        public DateTime TurnOffTime { get; set; }
        public SessionStatus Status { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
