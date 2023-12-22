using SmartEnergyHub.BLL.Residence_.Models;

namespace SmartEnergyHub.BLL.Residence_.Abstract
{
    public interface IResidenceProvider
    {
        Task<ResidenceResponseModel> GetResidenceByCustomerIdAsync(string id);
        Task<ResidenceResponseModel> GetResidenceUpdateModel(int id);
        Task UpdateConnectionStatusToConnect(int residenceId);
        Task UpdateResidence(UpdateResidenceRequestModel request);
        Task ClearResidenceInfo(int residenceId, string customerId);
    }
}
