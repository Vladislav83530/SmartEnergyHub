using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartEnergyHub.API.Filters;
using SmartEnergyHub.API.Settings;
using SmartEnergyHub.BLL.Device_.Models;
using SmartEnergyHub.BLL.Device_.Abstract;
using SmartEnergyHub.BLL.DeviceGenerator.Abstract;

namespace SmartEnergyHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceGeneratorSettings _deviceGeneratorSettings;
        private readonly IDeviceGenerator _deviceGenerator;
        private readonly IDeviceService _deviceService;

        public DeviceController(
            IOptions<DeviceGeneratorSettings> deviceGeneratorSettings,
            IDeviceGenerator deviceGenerator,
            IDeviceService deviceServices)
        {
            this._deviceGeneratorSettings = deviceGeneratorSettings?.Value ?? throw new ArgumentNullException(nameof(deviceGeneratorSettings));
            this._deviceGenerator = deviceGenerator ?? throw new ArgumentNullException(nameof(deviceGenerator));
            this._deviceService = deviceServices ?? throw new ArgumentNullException(nameof(deviceServices));
        }

        [HttpPost, Route("create-devices/{residenceId}")]
        public async Task<IActionResult> CreateDevices(int residenceId)
        {
            if (residenceId <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(residenceId));
            }

            await this._deviceGenerator.CreateRandomDevices(residenceId, _deviceGeneratorSettings.DeviceNameData, _deviceGeneratorSettings.SerialNumbers, _deviceGeneratorSettings.AccessTokens,
                _deviceGeneratorSettings.MACAddresses, _deviceGeneratorSettings.Rooms);

            return Ok();
        }

        [HttpPost, Route("get-devices/{residenceId}")]
        public async Task<IActionResult> GetDevices(int residenceId, [FromBody] FilterRequestModel request)
        {
            if (residenceId <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(residenceId));
            }

            if (request == null)
            {
                return ExceptionFilter.ErrorResult(nameof(request));
            }

            List<DeviceResponseModel> devices = await this._deviceService.GetDevicesAsync(residenceId, request.FilterModel, request.PaginationModel);

            if (devices == null)
            {
                return NotFound();
            }

            return Ok(devices);
        }
    } 
}
