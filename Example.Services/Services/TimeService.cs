using System;
using Example.Services.Services.Interfaces;

namespace Example.Services.Services
{
    public class TimeService : ITimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
