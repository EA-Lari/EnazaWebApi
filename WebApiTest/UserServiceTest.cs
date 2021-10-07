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
            var userWithoutLogin = FactoryUserDto(22);
            userWithoutLogin.Login = "";
            await Assert.ThrowsAsync<ArgumentException>(() => service.Add(userWithoutLogin));
            var userWithoutPassword = FactoryUserDto(33);
            userWithoutPassword.Password = "";
            await Assert.ThrowsAsync<ArgumentException>(() => service.Add(userWithoutPassword));
            Task.WaitAll(tasks.ToArray());
        }

        private async Task CheckAddWithAdmin()
        {
            var repo = FactoryRepo(ListWithAdmin());
            var service = FactoryService(repo);
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.Add(FactoryAdminDto(2)));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.Add(FactoryUserDto(0)));
        }

        private async Task CheckAddAllBlocked()
        {
            var repo = FactoryRepo(ListAllBlocked());
            var service = FactoryService(repo);
            await service.Add(FactoryAdminDto(2));
            await service.Add(FactoryUserDto(10));
        }

        private async Task CheckAddWithoutAdmin()
        {
            var repo = FactoryRepo(ListWithoutAdmin());
            var service = FactoryService(repo);
            await service.Add(FactoryAdminDto(2));
            await service.Add(FactoryUserDto(10));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.Add(FactoryUserDto(0)));
        }


        [Fact]
        public async Task Edit()
        {
            var tasks = new List<Task>();
            tasks.Add(CheckEditWithAdmin());
            tasks.Add(CheckEditAllBlocked());
            tasks.Add(CheckEditWithoutAdmin());
            var repo = FactoryRepo(ListWithoutAdmin());
            var service = FactoryService(repo);
            //Без логина
            var userWithoutLogin = FactoryUserDto(1,false);
            userWithoutLogin.Login = "";
            await service.Edit(userWithoutLogin);
            //////Без пароля
            var userWithoutPassword = FactoryUserDto(2,false);
            userWithoutPassword.Password = "";
            await service.Edit(userWithoutPassword);
            //////Без идентификатора
            var userWithoutID = FactoryUserDto(2);
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Edit(userWithoutID));
            Task.WaitAll(tasks.ToArray());
        }

        private async Task CheckEditWithAdmin()
        {
            var repo = FactoryRepo(ListWithAdmin());
            var service = FactoryService(repo);
            var user = FactoryUserDto(1, false);
            user.Group = UserGroupCodeEnum.Admin;
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.Edit(user));
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.Edit(FactoryUserDto(11, false)));
        }

        private async Task CheckEditAllBlocked()
        {
            var repo = FactoryRepo(ListAllBlocked());
            var service = FactoryService(repo);
            var user = FactoryUserDto(1, false);
            user.Group = UserGroupCodeEnum.Admin;
            await service.Edit(user);
            await service.Edit(FactoryUserDto(2, false));
        }

        private async Task CheckEditWithoutAdmin()
        {
            var repo = FactoryRepo(ListWithoutAdmin());
            var service = FactoryService(repo);
            var user = FactoryUserDto(1, false);
            user.Group = UserGroupCodeEnum.Admin;
            await service.Edit(user);
            await service.Edit(FactoryUserDto(1, false));
        }


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
                FactoryAdmin(4),
                FactoryAdmin(5, UserStateCodeEnum.Blocked),
                FactoryUser(0),
                FactoryUser(1),
                FactoryUser(2)
            };

        private List<User> ListAllBlocked()
           => new List<User>
           {
                FactoryAdmin(4, UserStateCodeEnum.Blocked),
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

        private UserEditDto FactoryAdminDto(int i = 0, bool isNew =true)
            => new UserEditDto 
            {
                UserId = isNew? null: (int?)i*10,
                Login = $"Admin{i}",
                Password = "123123123",
                Group = UserGroupCodeEnum.Admin
            };

        private UserEditDto FactoryUserDto(int i = 0,bool isNew=true)
            => new UserEditDto
            {
                UserId = isNew ? null : (int?)i,
                Login = $"User{i}",
                Password = "123123123",
                Group = UserGroupCodeEnum.User
            };

        private User FactoryUser(int i = 0, UserStateCodeEnum state = UserStateCodeEnum.Active)
           => FactoryUser(i,$"User{i}", UserGroupCodeEnum.User, state);

        private User FactoryAdmin(int i = 0, UserStateCodeEnum state = UserStateCodeEnum.Active)
           => FactoryUser(i*10,$"Admin{i}", UserGroupCodeEnum.Admin, state);

        private User FactoryUser(int i,string login, UserGroupCodeEnum group, UserStateCodeEnum state)
            => new User
            {
                UserId = i,
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
