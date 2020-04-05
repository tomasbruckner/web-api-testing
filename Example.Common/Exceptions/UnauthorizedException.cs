using System;
using Example.Common.Enums;
using Example.Common.Extensions;

namespace Example.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized")
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(ErrorCodeEnum errorCode) : base(errorCode.GetDescription())
        {
        }
    }
}
