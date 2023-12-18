using SmartEnergyHub.BLL.Device_.Models;

namespace SmartEnergyHub.BLL.Device_.Models
{
    public class FilterRequestModel
    {
        public FilterModel FilterModel { get; set; }
        public PaginationModel PaginationModel { get; set; }
    }
}