using SmartEnergyHub.BLL.Customer.Models;
using SmartEnergyHub.BLL.Models;

namespace SmartEnergyHub.BLL.Customer.Abstract
{
    public interface ICustomerInfoProvider
    {
        Task<CustomerResponseModel> GetCustomerAsync(string customerId);
        Task<string> UpdateCustomerAsync(UpdateCustomerRequestModel model);
        Task DeleteCustomerAsync(string customerId);
    }
}
