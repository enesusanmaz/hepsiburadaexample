using System;

namespace HepsiBuradaExample.Services.Helpers
{
    public static class ServiceMessageHelper
    {
        public static string GetExceptionMessage(ErrorType errorType, DataType? dataType = null)
        {
            string messageText = string.Empty;
            switch (errorType)
            {
                case ErrorType.NotFound:
                    messageText = $"{dataType} not found!";
                    break;
                case ErrorType.UnexpectedError:
                    messageText = "An unexpected error was encountered during the operation.";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return messageText;
        }
    }

    public enum ErrorType
    {
        UnexpectedError,
        NotFound
    }

    public enum DataType
    {
        Product,
        Order,
        Campaign
    }
}
