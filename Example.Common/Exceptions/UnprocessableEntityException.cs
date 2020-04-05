using System;
using Example.Common.Enums;
using Example.Common.Extensions;

namespace Example.Common.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException() : base("Unprocessable Entity")
        {
        }

        public UnprocessableEntityException(string message) : base(message)
        {
        }

        public UnprocessableEntityException(ErrorCodeEnum errorCode) : base(errorCode.GetDescription())
        {
        }
    }
}
