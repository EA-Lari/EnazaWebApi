using EnazaWebApi.Logic.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnazaWebApi.Logic
{
    public interface IUserService
    {
        Task Add(UserEditDto user);

        Task SetStateBlocked(int userId);

        Task Edit(UserEditDto user);

        Task<List<UserShowDto>> GetUsers();

        Task<UserShowDto> GetUser(int userId);
    }
}
