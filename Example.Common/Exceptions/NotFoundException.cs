using System;
using Example.Common.Enums;
using Example.Common.Extensions;

namespace Example.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not Found")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(ErrorCodeEnum errorCode) : base(errorCode.GetDescription())
        {
        }
    }
}
