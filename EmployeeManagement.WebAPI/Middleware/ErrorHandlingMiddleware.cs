﻿using EmployeeManagement.Shared.Response;
using System.Net;

namespace EmployeeManagement.WebAPI.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unhandled exception: {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Map exception to response
        var (statusCode, message, errors) = exception switch
        {
            // 404 Not Found
            KeyNotFoundException ex => 
                (HttpStatusCode.NotFound, "Not found", new List<string> { ex.Message }),

            // 400 Bad Request - Null argument (check before ArgumentException)
            ArgumentNullException ex => 
                (HttpStatusCode.BadRequest, "Required field missing", new List<string> { ex.Message }),
            
            // 400 Bad Request
            ArgumentException ex => 
                (HttpStatusCode.BadRequest, "Invalid request", new List<string> { ex.Message }),

            // 401 Unauthorized
            UnauthorizedAccessException ex => 
                (HttpStatusCode.Unauthorized, "Unauthorized", new List<string> { ex.Message }),

            // 409 Conflict
            InvalidOperationException ex => 
                (HttpStatusCode.Conflict, "Operation failed", new List<string> { ex.Message }),

            // 500 Internal Server Error (default)
            _ => (HttpStatusCode.InternalServerError, "Internal error", 
                  new List<string> { "An unexpected error occurred" })
        };

        context.Response.StatusCode = (int)statusCode;
        var response = new
        {
            isSuccess = false,
            statusCode = ((int)statusCode).ToString(),
            message = message,
            data = (object?)null,
            errors = errors
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}


