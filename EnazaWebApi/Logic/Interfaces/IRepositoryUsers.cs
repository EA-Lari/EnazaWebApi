using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnazaWebApi.Logic.Interfaces
{
    public interface IRepositoryUsers
    {
        public Task Add(UserEditDto userDto);

        public Task Update(UserEditDto userDto);

        public Task SetBlockState(int userId);

        Task<List<UserShowDto>> GetUsers();

        Task<UserShowDto> GetUser(int userId);

        bool Any(Expression<Func<User, bool>> predicate);
    }
}
