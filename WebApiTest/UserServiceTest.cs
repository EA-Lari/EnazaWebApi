using EnazaWebApi.Data.Enums;
using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace WebApiTest
{
    public class UserServiceTest
    {
        [Fact]
        public async Task Add()
        {
            var service = FactoryService();
            await CheckAddEdit(service.Add);
            var userWithoutLogin = FactoryUserDto(1);
            userWithoutLogin.Login = "";
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.Add(userWithoutLogin));
            var userWithoutPassword = FactoryUserDto(2);
            userWithoutPassword.Password = "";
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.Add(userWithoutPassword));
        }

        [Fact]
        public async Task Edit()
        {
            var service = FactoryService();
            await CheckAddEdit(service.Edit);
            var userForEdit = (await service.GetUsers()).FirstOrDefault();
            var userWithoutLogin = FactoryUserDto(1);
            userWithoutLogin.UserId = userForEdit.UserId;
            userWithoutLogin.Login = "";
            await service.Edit(userWithoutLogin);
            userForEdit = await service.GetUser(userForEdit.UserId);
            Assert.True(userWithoutLogin.Password == userForEdit.Password
                && userWithoutLogin.Group == userForEdit.Group.Code);
            var userWithoutPassword = FactoryUserDto(2);
            userWithoutPassword.UserId = userForEdit.UserId;
            userWithoutPassword.Password = "";
            await service.Edit(userWithoutPassword);
            Assert.True(userWithoutLogin.Login == userForEdit.Login
                && userWithoutLogin.Group == userForEdit.Group.Code);
        }

        private async Task CheckAddEdit(Func<UserEditDto, Task> action)
        {
            await CheckBySingleAdmin(action);
            await CheckByUniqueLogin(action);
        }

        private async Task CheckBySingleAdmin(Func<UserEditDto, Task> action)
        {
            await action(FactoryAdminDto("Admin"));
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await action(FactoryAdminDto("Admin2")));
        }

        private async Task CheckByUniqueLogin(Func<UserEditDto, Task> action)
        {
            var userDto = FactoryUserDto();
            await action(userDto);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await action(userDto));
            await Assert.ThrowsAsync<ArgumentException>(async () =>
               await action(FactoryAdminDto("Admin")));
        }

        private IUserService FactoryService()
        {
            return new UserService();
        }

        private UserEditDto FactoryAdminDto(string login = "Admin")
            => new UserEditDto 
            {
                Login = login,
                Password = "123123123",
                Group = UserGroupCodeEnum.Admin
            };

        private UserEditDto FactoryUserDto(int i = 0)
            => new UserEditDto
            {
                Login = $"User{0}",
                Password = "123123123",
                Group = UserGroupCodeEnum.User
            };
    }
}
