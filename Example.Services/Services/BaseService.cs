using Example.Common.Exceptions;

namespace Example.Services.Services
{
    public class BaseService
    {
        protected void ThrowIfNotFound(object entity, string message = "Not found")
        {
            if (entity == null)
            {
                throw new NotFoundException(message);
            }
        }
    }
}
