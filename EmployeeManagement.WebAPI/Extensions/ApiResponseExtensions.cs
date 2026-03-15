using EmployeeManagement.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Extensions;

public static class ApiResponseExtensions
{
    public static ApiResponse<T> ToSuccessResponse<T>(this T data, string message = "Success", string statusCode = "200")
    {
        return ApiResponse<T>.Success(data, message, statusCode);
    }

    public static ApiResponse<T> ToCreatedResponse<T>(this T data, string message = "Created")
    {
        return ApiResponse<T>.Success(data, message, "201");
    }

    public static ApiResponse<T> ToErrorResponse<T>(this string message, string statusCode = "400", List<string>? errors = null)
    {
        return ApiResponse<T>.Failure(message, statusCode, errors);
    }

    public static ApiResponse<T> ToNotFoundResponse<T>(this string message)
    {
        return ApiResponse<T>.Failure(message, "404");
    }

    public static ApiResponse<T> ToUnauthorizedResponse<T>(this string message)
    {
        return ApiResponse<T>.Failure(message, "401");
    }

    public static ApiResponse<T> ToForbiddenResponse<T>(this string message)
    {
        return ApiResponse<T>.Failure(message, "403");
    }

    public static ApiResponse<T> ToValidationErrorResponse<T>(this List<string> errors, string message = "Validation failed")
    {
        return ApiResponse<T>.Failure(message, "422", errors);
    }

    public static ApiResponse<T> ToServerErrorResponse<T>(this string message)
    {
        return ApiResponse<T>.Failure(message, "500");
    }
}
