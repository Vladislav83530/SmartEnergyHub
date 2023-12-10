using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Customer.Abstract;
using SmartEnergyHub.BLL.Models;
using SmartEnergyHub.DAL.EF;

namespace SmartEnergyHub.BLL.Customer
{
    public class CustomerInfoProvider : ICustomerInfoProvider
    {
        private readonly ApplicationDbContext _context;

        public CustomerInfoProvider(ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CustomerResponseModel> GetCustomer(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            var customer = await this._context.Customers.FirstOrDefaultAsync(customer => customer.Id == customerId);

            CustomerResponseModel customerResponseModel = null;

            if (customer != null)
            {
                customerResponseModel = new CustomerResponseModel
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FistName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Region = customer.Region,
                    City = customer.City,
                    Street = customer.Street,
                    HouseNumber = customer.HouseNumber,
                    FlatNumber = customer.FlatNumber
                };
            }

            return customerResponseModel;
        }
    }
}
