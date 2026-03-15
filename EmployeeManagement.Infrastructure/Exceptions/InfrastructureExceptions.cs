namespace EmployeeManagement.Infrastructure.Exceptions;

public class InfrastructureException : Exception
{
    public string ErrorCode { get; set; }
    public object? ErrorDetails { get; set; }

    public InfrastructureException(string message, string errorCode = "INFRASTRUCTURE_ERROR", object? details = null)
        : base(message)
    {
        ErrorCode = errorCode;
        ErrorDetails = details;
    }

    public InfrastructureException(string message, Exception? innerException, string errorCode = "INFRASTRUCTURE_ERROR", object? details = null)
        : base(message, innerException ?? new Exception())
    {
        ErrorCode = errorCode;
        ErrorDetails = details;
    }
}

public class DatabaseException : InfrastructureException
{
    public DatabaseException(string message, Exception? innerException = null)
        : base(message, innerException ?? new Exception(), "DATABASE_ERROR") { }
}

public class CacheException : InfrastructureException
{
    public CacheException(string message, Exception? innerException = null)
        : base(message, innerException ?? new Exception(), "CACHE_ERROR") { }
}

public class AuthenticationException : InfrastructureException
{
    public AuthenticationException(string message, Exception? innerException = null)
        : base(message, innerException ?? new Exception(), "AUTHENTICATION_ERROR") { }
}

public class EmailServiceException : InfrastructureException
{
    public EmailServiceException(string message, Exception? innerException = null)
        : base(message, innerException ?? new Exception(), "EMAIL_ERROR") { }
}

public class FileStorageException : InfrastructureException
{
    public FileStorageException(string message, Exception? innerException = null)
        : base(message, innerException ?? new Exception(), "FILE_STORAGE_ERROR") { }
}


