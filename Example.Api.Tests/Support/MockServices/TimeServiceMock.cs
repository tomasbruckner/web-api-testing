using System;
using Example.Services.Services.Interfaces;

namespace Example.Api.Tests.Support.MockServices
{
    public class TimeServiceMock : ITimeService
    {
        public static readonly DateTime Now = new DateTime(2019, 9, 11, 16, 5, 24, DateTimeKind.Utc);

        public DateTime GetUtcNow()
        {
            return Now;
        }
    }
}
