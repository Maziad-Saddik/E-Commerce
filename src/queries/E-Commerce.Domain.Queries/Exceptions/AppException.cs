﻿namespace E_Commerce.Domain.Queries.Exceptions;

public class AppException : Exception
{
    public ExceptionStatusCode StatusCode { get; set; }

    public AppException(ExceptionStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
