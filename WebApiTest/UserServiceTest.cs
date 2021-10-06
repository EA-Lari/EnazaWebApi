using EnazaWebApi.Data.Enums;
using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
using EnazaWebApi.Logic.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var mockRepo = FactoryMockRepo();
            mockRepo.Setup(x =>)
            var service = FactoryService();
            await CheckAddEdit(service.Edit);
            var userForEdit = (await service.GetUsers()).FirstOrDefault();
            //Без логина
            var userWithoutLogin = FactoryUserDto(1);
            userWithoutLogin.UserId = userForEdit.UserId;
            userWithoutLogin.Login = "";
            await service.Edit(userWithoutLogin);
            userForEdit = await service.GetUser(userForEdit.UserId);
            Assert.True(userWithoutLogin.Password == userForEdit.Password
                && userWithoutLogin.Group == userForEdit.Group.Code);
            //Без пароля
            var userWithoutPassword = FactoryUserDto(2);
            userWithoutPassword.UserId = userForEdit.UserId;
            userWithoutPassword.Password = "";
            await service.Edit(userWithoutPassword);
            userForEdit = await service.GetUser(userForEdit.UserId);
            Assert.True(userWithoutLogin.Login == userForEdit.Login
                && userWithoutLogin.Group == userForEdit.Group.Code);
            //Без идентификатора
            var userWithoutID = FactoryUserDto(2);
            userWithoutID.UserId = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Edit(userWithoutID));
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

        private IUserService FactoryService(IRepositoryUsers repo)
            => new UserService(repo);

        private Mock<IRepositoryUsers> FactoryMockRepo(List<UserEditDto> userList)
        { 
            var mock = new Mock<IRepositoryUsers>();
            mock.Setup(x => x.Add(It.IsAny<UserEditDto>()))
                .Callback((UserEditDto x) => userList.Add(x));
            mock.Setup(x => x.Update(It.IsAny<UserEditDto>()));
            mock.Setup(x => x.Any(It.IsAny<Expression<Func<User, bool>>>()))
                .Callback((Expression<Func<User, bool>> func)=> 
                {
                });

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
