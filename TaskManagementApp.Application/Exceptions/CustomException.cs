using System;

namespace TaskManagementApp.Application.Exceptions
{
    public class CustomException(string message) : Exception(message)
    {
    }
}
