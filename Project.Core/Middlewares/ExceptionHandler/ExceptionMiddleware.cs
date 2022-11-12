﻿using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Project.Core.Config;
using Project.Core.Logging;
using Project.Core.Middlewares.Translation;
using Project.DTO.Responses;
using Sentry;

namespace Project.Core.Middlewares.ExceptionHandler;

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

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        // var message = exception switch
        // {
        //     AccessViolationException => "Access violation error from the custom middleware",
        //     _ => "Internal Server Error from the custom middleware."
        // };
        var response = new ErrorDataResult<Result>(Localization.Translate(Messages.GeneralError));

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}