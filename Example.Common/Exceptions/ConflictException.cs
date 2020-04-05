using System;
using Example.Common.Enums;
using Example.Common.Extensions;

namespace Example.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base("Conflict")
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(ErrorCodeEnum errorCode) : base(errorCode.GetDescription())
        {
        }
    }
}
