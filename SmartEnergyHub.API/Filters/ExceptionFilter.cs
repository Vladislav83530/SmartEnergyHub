using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Net;

namespace SmartEnergyHub.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private const string DefaultTitle = "Internal server error";
        private const string DefaultMessage = "No message";
        private const string ErrorDebugLogFormat = "Message: {ExceptionMessage}. StackTrace: {StackTrace}. Path: {Path}. Params: {@Params}";

        public static IActionResult ErrorResult(string? parameterName = null, string? exMessage = null)
        {
            string errorMessage;

            if (!string.IsNullOrEmpty(parameterName))
            {
                errorMessage = $"Wrong paramerter value. Parameter name: {parameterName}";

                Log.Warning("Wrong paramerter value. Parameter name: {ParametrName}. {StackTrace}", parameterName, Environment.StackTrace);
            }
            else if (!string.IsNullOrEmpty(exMessage))
            {
                errorMessage = exMessage;
            }
            else
            {
                errorMessage = DefaultMessage;
            }

            return new JsonResult(new { message = $"{DefaultTitle}: {errorMessage}" })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        public void OnException(ExceptionContext context)
        {
            Serilog.ILogger logger = Log.Logger;

            WriteErrorDebug(logger, context);

            context.Result = ErrorResult(null, context.Exception.Message);
        }

        private void WriteErrorDebug(Serilog.ILogger logger, ExceptionContext context)
        {
            logger.Error(
                ErrorDebugLogFormat,
                context.Exception?.Message ?? DefaultMessage,
                context.Exception?.StackTrace ?? Environment.StackTrace,
                context.HttpContext.Request.Path,
                context.RouteData.Values.ToDictionary(x => x.Key, x => x.Value)
            );
        }
    }
}