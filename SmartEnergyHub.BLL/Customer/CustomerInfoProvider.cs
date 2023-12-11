using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Customer.Abstract;
using SmartEnergyHub.BLL.Customer.Models;
using SmartEnergyHub.BLL.Models;
using SmartEnergyHub.DAL.EF;
using SmartEnergyHub.DAL.Entities;

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

            var customer = await this._context.Customers
                .Include(c => c.Residence)
                .ThenInclude(u => u.ResidenceLocation)
                .FirstOrDefaultAsync(customer => customer.Id == customerId);

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
                    Region = customer.Residence.ResidenceLocation.Region,
                    City = customer.Residence.ResidenceLocation.City,
                    Street = customer.Residence.ResidenceLocation.Street,
                    HouseNumber = customer.Residence.ResidenceLocation.HouseNumber,
                    FlatNumber = customer.Residence.ResidenceLocation.FlatNumber
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

            var customer = await this._context.Customers
                .Include(c => c.Residence)
                .ThenInclude(u => u.ResidenceLocation)
                .FirstOrDefaultAsync(customer => customer.Id == model.CustomerId);

            if (customer != null)
            {
                customer.Id = model.CustomerId;
                customer.FistName = model.FirstName;
                customer.LastName = model.LastName;
                customer.PhoneNumber = model.PhoneNumber;

                if (customer.Residence != null && customer.Residence.ResidenceLocation != null)
                {
                    customer.Residence.ResidenceLocation.Region = model.Region;
                    customer.Residence.ResidenceLocation.City = model.City;
                    customer.Residence.ResidenceLocation.Street = model.Street;
                    customer.Residence.ResidenceLocation.HouseNumber = model.HouseNumber;
                    customer.Residence.ResidenceLocation.FlatNumber = model.FlatNumber;
                }

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return customer.Id;
            }

            return null;
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            var customer = await this._context.Customers
                .Include(c => c.Residence)
                .ThenInclude(u => u.ResidenceLocation)
                .FirstOrDefaultAsync(customer => customer.Id == customerId);

            this._context.Customers.Remove(customer);
            await this._context.SaveChangesAsync();
        }

        public async Task<int> CreateDefaultResidenceAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            ResidenceLocation residenceLocation = new ResidenceLocation();


            Residence residence = new Residence
            {
                CustomerId = customerId,
                ResidenceLocation = residenceLocation
            };

            _context.Residences.Add(residence);
            await _context.SaveChangesAsync();

            return residence.Id;
        }
    }
}
