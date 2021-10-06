using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace EnazaWebApi.Logic
{
    public interface IValidate
    {
        Task CheckLogin(IQueryable<User> query, string login);

        Task CheckState(IQueryable<User> query, UserEditDto user);

        Task CheckForCompletenessTest(UserEditDto user);
    }
}
