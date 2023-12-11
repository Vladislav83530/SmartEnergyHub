using SmartEnergyHub.BLL.Residence_.Models;

namespace SmartEnergyHub.BLL.Residence_.Abstract
{
    public interface IResidenceProvider
    {
        Task<ResidenceResponseModel> GetResidenceByCustomerIdAsync(string id);
    }
}
