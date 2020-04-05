using System;
using Example.Common.Enums;
using Example.Common.Extensions;

namespace Example.Common.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("Forbidden")
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(ErrorCodeEnum errorCode) : base(errorCode.GetDescription())
        {
        }
    }
}
