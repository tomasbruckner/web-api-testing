using System.Linq;
using System.Threading.Tasks;
using Example.Common.Enums;
using Example.Data.Data;
using Example.Models.Mappers;
using Example.Models.Models;
using Example.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Example.Services.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly RepositoryContext _db;
        private readonly ITimeService _timeService;

        public UserService(RepositoryContext db, ITimeService timeService)
        {
            _db = db;
            _timeService = timeService;
        }

        public async Task<UserDto> GetById(int userId, bool asNoTracking = true)
        {
            var entity = await Query(userId, asNoTracking)
                .ToDtoList()
                .FirstOrDefaultAsync();

            ThrowIfNotFound(entity);

            return entity;
        }

        public async Task<UserDto> Create(UserCreateDto input)
        {
            var entity = new AppUser
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Age = input.Age,
                Created = _timeService.GetUtcNow(),
                RoleId = (int) RoleEnum.User
            };

            _db.Add(entity);
            await _db.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<UserDto> Edit(int userId, UserEditDto input)
        {
            var entity = await Query(userId, false)
                .FirstOrDefaultAsync();
            ThrowIfNotFound(entity);

            entity.FirstName = input.FirstName;
            entity.LastName = input.LastName;
            entity.Age = input.Age;

            _db.Update(entity);
            await _db.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task Delete(int userId)
        {
            _db.Remove(await Query(userId, false).FirstOrDefaultAsync());

            await _db.SaveChangesAsync();
        }

        private IQueryable<AppUser> Query(int userId, bool asNoTracking = true)
        {
            var query = _db.AppUsers
                .Where(o => o.UserId == userId);

            return asNoTracking ? query.AsNoTracking() : query;
        }
    }
}
