using Azure;
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

        [HttpPut, Route("set-connected-status/{id}")]
        public async Task<IActionResult> SetConnectedStatus(int id)
        {
            if (id <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(id));
            }

            await this._residenceProvider.UpdateConnectionStatusToConnect(id);

            return Ok();
        }

        [HttpPut, Route("update-residence")]
        public async Task<IActionResult> UpdateResidence(UpdateResidenceRequestModel request)
        {
            if (request == null)
            {
                return ExceptionFilter.ErrorResult(nameof(request));
            }

            await this._residenceProvider.UpdateResidence(request);

            return Ok();
        }

        [HttpGet, Route("residence-by-id/{id}")]
        public async Task<IActionResult> GetResidenceById(int id)
        {
            if (id <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(id));
            }

            ResidenceResponseModel model = await this._residenceProvider.GetResidenceUpdateModel(id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpDelete, Route("{residenceId}/{customerId}")]
        public async Task<IActionResult> ClearResidenceInfo(int residenceId, string customerId)
        {
            if (residenceId <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(residenceId));
            }

            if (string.IsNullOrEmpty(customerId))
            {
                return ExceptionFilter.ErrorResult(nameof(customerId));
            }

            await this._residenceProvider.ClearResidenceInfo(residenceId, customerId);

            return Ok();
        }
    }
}
