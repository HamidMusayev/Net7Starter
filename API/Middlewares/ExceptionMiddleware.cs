﻿/*using CORE.Config;
using CORE.Localization;
using CORE.Logging;
using DTO.Responses;
using Sentry;
using System.Net;
using System.Text.Json;

namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly ConfigSettings _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILoggerManager _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger, IWebHostEnvironment env,
        ConfigSettings config)
    {
        _config = config;
        _logger = logger;
        _env = env;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");

            if (_config.SentrySettings.IsEnabled) SentrySdk.CaptureException(ex);

            if (_env.IsDevelopment()) throw;

            await HandleExceptionAsync(httpContext);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = new ErrorResult(Messages.GeneralError.Translate());
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}*/

