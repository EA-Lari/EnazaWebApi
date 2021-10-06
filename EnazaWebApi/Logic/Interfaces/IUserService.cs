using EnazaWebApi.Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnazaWebApi.Logic
{
    public interface IUserService
    {
        Task Add(UserEditDto user);

        Task Delete(int userId);

        Task Edit(UserEditDto user);

        Task<UserShowDto> GetUsers();

        UserShowDto GetUser();
    }
}
