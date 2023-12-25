using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.BLL.Residence_.Abstract;
using SmartEnergyHub.BLL.Residence_.Models;
using SmartEnergyHub.DAL.EF;
using SmartEnergyHub.DAL.Entities;

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
                .Include(x=>x.Devices)
                .Where(residence => residence.CustomerId == id)
                .Select(residence => new ResidenceResponseModel
                {
                    Id = residence.Id,
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

        public async Task UpdateResidence(UpdateResidenceRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var residenceToUpdate = this._context.Residences.Include(x=>x.ResidenceLocation).FirstOrDefault(e => e.Id == request.Id);

            if (residenceToUpdate != null)
            {
                residenceToUpdate.Area = request.Area;
                residenceToUpdate.RoomCount = request.RoomCount;
                residenceToUpdate.ResidenceLocation.City = request.City;
                residenceToUpdate.ResidenceLocation.Region = request.Region;
                residenceToUpdate.ResidenceLocation.Street = request.Street;
                residenceToUpdate.ResidenceLocation.HouseNumber = request.HouseNumber;
                residenceToUpdate.ResidenceLocation.FlatNumber = request.FlatNumber;

                await this._context.SaveChangesAsync();
            }
        }

        public async Task UpdateConnectionStatusToConnect(int residenceId)
        {
            if (residenceId <= 0)
            {
                throw new ArgumentNullException(nameof(residenceId));
            }

            var residenceToUpdate = this._context.Residences.FirstOrDefault(e => e.Id == residenceId);

            if (residenceToUpdate != null)
            {
                residenceToUpdate.ConnectionStatus = DAL.Entities.Enums.ConnectionStatus.Connected;

                await this._context.SaveChangesAsync();
            }
        }

        public async Task<ResidenceResponseModel> GetResidenceUpdateModel(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            ResidenceResponseModel result = await this._context.Residences
                .Include(x => x.Devices)
                .Where(residence => residence.Id == id)
                .Select(residence => new ResidenceResponseModel
                {
                    Id = residence.Id,
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

        public async Task ClearResidenceInfo(int residenceId, string customerId)
        {
            if (residenceId <= 0)
            {
                throw new ArgumentNullException(nameof(residenceId));
            }

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }


            var residenceToDelete = this._context.Residences
                .Include(x=>x.ResidenceLocation)
                .Include(x=>x.Devices)
                .ThenInclude(x=>x.DeviceInfo)
                .FirstOrDefault(e => e.Id == residenceId);

            if (residenceToDelete != null)
            {
                this._context.Residences.Remove(residenceToDelete);
                await _context.SaveChangesAsync();

                ResidenceLocation residenceLocation = new ResidenceLocation();


                Residence residence = new Residence
                {
                    CustomerId = customerId,
                    ResidenceLocation = residenceLocation,
                    ConnectionStatus = DAL.Entities.Enums.ConnectionStatus.Disconnected
                };

                _context.Residences.Add(residence);
                await _context.SaveChangesAsync();
            }
        }
    }
}
