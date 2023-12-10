using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Customer.Abstract;
using SmartEnergyHub.BLL.Customer.Models;
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

        public async Task<CustomerResponseModel> GetCustomerAsync(string customerId)
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

        public async Task<string> UpdateCustomerAsync(UpdateCustomerRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrEmpty(model.CustomerId))
            {
                throw new ArgumentNullException(nameof(model.CustomerId));
            }

            var customer = await this._context.Customers.FirstOrDefaultAsync(customer => customer.Id == model.CustomerId);

            if (customer != null)
            {
                customer.Id = model.CustomerId;
                customer.FistName = model.FirstName;
                customer.LastName = model.LastName;
                customer.PhoneNumber = model.PhoneNumber;
                customer.Region = model.Region;
                customer.City = model.City;
                customer.Street = model.Street;
                customer.HouseNumber = model.HouseNumber;
                customer.FlatNumber = model.FlatNumber;

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return customer.Id;
            }

            return null;
        }
    }
}
