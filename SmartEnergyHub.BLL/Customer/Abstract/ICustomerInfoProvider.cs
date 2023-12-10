using SmartEnergyHub.BLL.Models;

namespace SmartEnergyHub.BLL.Customer.Abstract
{
    public interface ICustomerInfoProvider
    {
        Task<CustomerResponseModel> GetCustomer(string customerId);
    }
}
