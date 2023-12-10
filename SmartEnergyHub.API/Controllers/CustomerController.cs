﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEnergyHub.BLL.Customer.Abstract;
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
                throw new ArgumentNullException(nameof(id));
            }

            CustomerResponseModel model = await this._customerInfoProvider.GetCustomer(id);

            if (model == null)
            {
                return NotFound(model);
            }

            return Ok(model);
        }
    }
}
