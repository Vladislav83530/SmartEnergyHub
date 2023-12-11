using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Residence_.Abstract;
using SmartEnergyHub.BLL.Residence_.Models;
using SmartEnergyHub.DAL.EF;

namespace SmartEnergyHub.BLL.Residence_
{
    public class ResidenceProvider : IResidenceProvider
    {
        private readonly ApplicationDbContext _context;

        public ResidenceProvider(ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ResidenceResponseModel> GetResidenceByCustomerIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            ResidenceResponseModel result = await this._context.Residences
                .Where(residence => residence.CustomerId == id)
                .Select(residence => new ResidenceResponseModel
                {
                    DeviceCount = residence.DeviceCount,
                    RoomCount = residence.RoomCount,
                    Area = residence.Area,
                    OwnerFirstName = residence.Customer.FistName,
                    OwnerLastName = residence.Customer.LastName,
                    Region = residence.ResidenceLocation.Region,
                    City = residence.ResidenceLocation.City,
                    Street = residence.ResidenceLocation.Street,
                    HouseNumber = residence.ResidenceLocation.HouseNumber,
                    FlatNumber = residence.ResidenceLocation.FlatNumber,
                    ConnectionStatus = residence.ConnectionStatus
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
