using System;
using System.Linq;
using System.Linq.Expressions;
using Example.Data.Data;
using Example.Models.Models;

namespace Example.Models.Mappers
{
    public static class AppUserMapper
    {
        public static readonly Expression<Func<AppUser, UserDto>> AsDto =
            dataItem => new UserDto
            {
                UserId = dataItem.UserId,
                FirstName = dataItem.FirstName,
                LastName = dataItem.LastName,
                Age = dataItem.Age,
                RoleId = dataItem.RoleId
            };

        public static UserDto ToDto(this AppUser dataItem)
        {
            return AsDto.Compile()(dataItem);
        }

        public static IQueryable<UserDto> ToDtoList(this IQueryable<AppUser> dataCollection)
        {
            return dataCollection.Select(AsDto);
        }
    }
}
