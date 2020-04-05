using System;
using Example.Common.Enums;
using Example.Common.Extensions;

namespace Example.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Bad Request")
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(ErrorCodeEnum errorCode) : base(errorCode.GetDescription())
        {
        }
    }
}
