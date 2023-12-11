using Microsoft.AspNetCore.Mvc;
using SmartEnergyHub.API.Filters;
using SmartEnergyHub.BLL.Residence_.Abstract;
using SmartEnergyHub.BLL.Residence_.Models;

namespace SmartEnergyHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResidenceController : ControllerBase
    {
        private readonly IResidenceProvider _residenceProvider;

        public ResidenceController(IResidenceProvider residenceProvider)
        {
            this._residenceProvider = residenceProvider ?? throw new ArgumentNullException(nameof(residenceProvider));
        }

        [HttpGet, Route("{customerId}")]
        public async Task<IActionResult> Get(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return ExceptionFilter.ErrorResult(nameof(customerId));
            }

            ResidenceResponseModel response = await this._residenceProvider.GetResidenceByCustomerIdAsync(customerId);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
