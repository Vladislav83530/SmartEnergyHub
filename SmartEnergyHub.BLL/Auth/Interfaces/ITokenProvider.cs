using Microsoft.Extensions.Configuration;
using SmartEnergyHub.DAL.Entities.APIUser;

namespace SmartEnergyHub.BLL.Auth.Interfaces
{
    public interface ITokenProvider
    {
        string CreateToken(IConfiguration configuration, User user);
    }
}
