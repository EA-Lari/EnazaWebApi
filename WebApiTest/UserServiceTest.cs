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
            var tasks = new List<Task>();
            tasks.Add(CheckAddWithAdmin());
            tasks.Add(CheckAddAllBlocked());
            tasks.Add(CheckAddWithoutAdmin());
            var repo = FactoryRepo(ListWithoutAdmin());
            var service = FactoryService(repo);
            var userWithoutLogin = FactoryUserDto(10);
            userWithoutLogin.Login = "";
            await Assert.ThrowsAsync<ArgumentException>(() => service.Add(userWithoutLogin));
            var userWithoutPassword = FactoryUserDto(20);
            userWithoutPassword.Password = "";
            await Assert.ThrowsAsync<ArgumentException>(() =>service.Add(userWithoutPassword));
            Task.WaitAll(tasks.ToArray());
        }

        private async Task CheckAddWithAdmin()
        {
            var repo = FactoryRepo(ListWithAdmin());
            var service = FactoryService(repo);
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.Add(FactoryAdminDto("Admin2")));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.Add(FactoryUserDto(0)));
        }

        private async Task CheckAddAllBlocked()
        {
            var repo = FactoryRepo(ListAllBlocked());
            var service = FactoryService(repo);
            await service.Add(FactoryAdminDto("Admin2"));
            await service.Add(FactoryUserDto(10));
        }

        private async Task CheckAddWithoutAdmin()
        {
            var repo = FactoryRepo(ListWithoutAdmin());
            var service = FactoryService(repo);
            await service.Add(FactoryAdminDto("Admin2"));
            await service.Add(FactoryUserDto(10));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.Add(FactoryUserDto(0)));
        }

        //[Fact]
        //public async Task Edit()
        //{
        //    var list = new List<UserEditDto>();
        //    var repo = FactoryRepo(ListWithAdmin());
        //    var service = FactoryService(repo);
        //    await CheckAddEdit(service.Edit);
        //    var userForEdit = (await service.GetUsers()).FirstOrDefault();
        //    //Без логина
        //    var userWithoutLogin = FactoryUserDto(1);
        //    userWithoutLogin.UserId = userForEdit.UserId;
        //    userWithoutLogin.Login = "";
        //    await service.Edit(userWithoutLogin);
        //    userForEdit = await service.GetUser(userForEdit.UserId);
        //    Assert.True(userWithoutLogin.Password == userForEdit.Password
        //        && userWithoutLogin.Group == userForEdit.Group.Code);
        //    //Без пароля
        //    var userWithoutPassword = FactoryUserDto(2);
        //    userWithoutPassword.UserId = userForEdit.UserId;
        //    userWithoutPassword.Password = "";
        //    await service.Edit(userWithoutPassword);
        //    userForEdit = await service.GetUser(userForEdit.UserId);
        //    Assert.True(userWithoutLogin.Login == userForEdit.Login
        //        && userWithoutLogin.Group == userForEdit.Group.Code);
        //    //Без идентификатора
        //    var userWithoutID = FactoryUserDto(2);
        //    userWithoutID.UserId = null;
        //    await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Edit(userWithoutID));
        //}

        private IUserService FactoryService(IRepositoryUsers repo)
            => new UserService(repo);

        private IRepositoryUsers FactoryRepo(IEnumerable<User> listUser)
        { 
            var mock = new Mock<IRepositoryUsers>();
            mock.Setup(x => x.Any(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> func) => listUser.Any(func.Compile()));
            return mock.Object;
        }

        private List<User> ListWithAdmin()
            => new List<User>
            {
                FactoryAdmin(0),
                FactoryAdmin(1, UserStateCodeEnum.Blocked),
                FactoryUser(0),
                FactoryUser(1),
                FactoryUser(2)
            };

        private List<User> ListAllBlocked()
           => new List<User>
           {
                FactoryAdmin(0, UserStateCodeEnum.Blocked),
                FactoryUser(0, UserStateCodeEnum.Blocked),
                FactoryUser(1, UserStateCodeEnum.Blocked),
                FactoryUser(2, UserStateCodeEnum.Blocked)
           };

        private List<User> ListWithoutAdmin()
           => new List<User>
           {
                FactoryUser(0),
                FactoryUser(1),
                FactoryUser(2)
           };

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
                Login = $"User{i}",
                Password = "123123123",
                Group = UserGroupCodeEnum.User
            };

        private User FactoryUser(int i = 0, UserStateCodeEnum state = UserStateCodeEnum.Active)
           => FactoryUser($"User{i}", UserGroupCodeEnum.User, state);

        private User FactoryAdmin(int i = 0, UserStateCodeEnum state = UserStateCodeEnum.Active)
           => FactoryUser($"Admin{i}", UserGroupCodeEnum.Admin, state);

        private User FactoryUser(string login, UserGroupCodeEnum group, UserStateCodeEnum state)
            => new User
            {
                Login = login,
                Group = new UserGroup
                {
                    Code = group
                },
                State = new UserState
                {
                    Code = state
                }
            };
    }
}
