﻿namespace EmployeeManagement.Shared.Response;

/// <summary>
/// Standard API Response wrapper for all endpoints
/// </summary>
public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Success(T? data, string message = "Success", string statusCode = "200")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            Data = data,
            Errors = []
        };
    }

    public static ApiResponse<T> Failure(string message, string statusCode = "400", List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Data = default,
            Errors = errors ?? []
        };
    }
}



