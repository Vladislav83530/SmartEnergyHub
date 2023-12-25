using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEnergyHub.API.Filters;
using SmartEnergyHub.BLL.Customer.Abstract;
using SmartEnergyHub.BLL.Customer.Models;
using SmartEnergyHub.BLL.Models;

namespace SmartEnergyHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerInfoProvider _customerInfoProvider;

        public CustomerController(ICustomerInfoProvider customerInfoProvider)
        {
            this._customerInfoProvider = customerInfoProvider ?? throw new ArgumentNullException(nameof(customerInfoProvider));
        }

        [Authorize]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ExceptionFilter.ErrorResult(nameof(id));
            }

            CustomerResponseModel model = await this._customerInfoProvider.GetCustomerAsync(id);

            if (model == null)
            {
                return NotFound(model);
            }

            return Ok(model);
        }

        [Authorize]
        [HttpPost, Route("update")]
        public async Task<IActionResult> UpdateCustomerInfo(UpdateCustomerRequestModel model)
        {
            if (model == null)
            {
                return ExceptionFilter.ErrorResult(nameof(model));
            }

            string customerId = await this._customerInfoProvider.UpdateCustomerAsync(model);

            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete, Route("delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ExceptionFilter.ErrorResult(nameof(id));
            }

            try
            {
                await this._customerInfoProvider.DeleteCustomerAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost, Route("add-residence/{id}")]
        public async Task<IActionResult> AddCustomerResidence(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ExceptionFilter.ErrorResult(nameof(id));
            }

            int result = await this._customerInfoProvider.CreateDefaultResidenceAsync(id);

            if (result <= 0)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
