using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartEnergyHub.API.Filters;
using SmartEnergyHub.API.Settings;
using SmartEnergyHub.BLL.Device_.Models;
using SmartEnergyHub.BLL.Device_.Abstract;
using SmartEnergyHub.BLL.DeviceGenerator.Abstract;
using Microsoft.AspNetCore.Authorization;
using SmartEnergyHub.DAL.Entities;

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
        [Authorize]
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
        [Authorize]
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

        [HttpPut, Route("update-active-status/{deviceId}/{isActive}")]
        [Authorize]
        public async Task<IActionResult> UpdateDeviceActiveStatus(int deviceId, bool isActive)
        {
            if (deviceId <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(deviceId));
            }

            await this._deviceService.UpdateDeviceStatus(deviceId, isActive);

            return Ok();
        }

        [HttpGet, Route("{deviceId}")]
        [Authorize]
        public async Task<IActionResult> GetDevice(int deviceId)
        {
            if (deviceId <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(deviceId));
            }

            DeviceResponseModel response = await this._deviceService.GetDevice(deviceId);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet, Route("get-sessions/{deviceId}/{period}")]
        [Authorize]
        public async Task<IActionResult> GetActivitySessions(int deviceId, Period period)
        {
            if (deviceId <= 0)
            {
                return ExceptionFilter.ErrorResult(nameof(deviceId));
            }

            List<ActivitySession> sessions = await this._deviceService.GetActivitySessions(deviceId, period);

            if (!sessions.Any())
            {
                return NotFound();
            }

            return Ok(sessions);
        }
    } 
}
