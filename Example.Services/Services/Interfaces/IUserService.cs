using System.Threading.Tasks;
using Example.Models.Models;

namespace Example.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetById(int userId, bool asNoTracking = true);
        Task<UserDto> Create(UserCreateDto input);
        Task<UserDto> Edit(int userId, UserEditDto input);
        Task Delete(int userId);
    }
}
