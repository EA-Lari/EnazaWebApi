using EnazaWebApi.Data.Enums;
using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace WebApiTest
{
    public class ValidateTest
    {
        private readonly string _adminName = "Admin";
        [Fact]
        public async Task CheckStateTest()
        {
            var valid = FactoryValidate();
            var listAdmin = new List<User>();
            listAdmin.Add(FactoryUser());
            await valid.CheckState((IQueryable<User>)listAdmin, FactoryAdminDto());
            listAdmin.Add(FactoryAdmin());
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await valid.CheckState((IQueryable<User>)listAdmin, FactoryAdminDto()));
            await valid.CheckState((IQueryable<User>)listAdmin, FactoryUserDto());
        }

        [Fact]
        public void CheckLoginTest()
        {
            var valid = FactoryValidate();
            var listAdmin = new List<User>();
            valid.CheckLogin((IQueryable<User>)listAdmin, _adminName);
            listAdmin.Add(FactoryAdmin());
            Assert.ThrowsAsync<ArgumentException>(async () => await valid.CheckLogin((IQueryable<User>)listAdmin, _adminName));
        }

        [Fact]
        public void CheckForCompletenessTest()
        {
            var valid = FactoryValidate();
            var userWithoutLigin = FactoryUserDto();
            userWithoutLigin.Login = "";
            Assert.ThrowsAsync<ArgumentException>(async () => await valid.CheckForCompletenessTest(userWithoutLigin));
            var userWithoutPassword = FactoryUserDto();
            userWithoutPassword.Password = "";
            Assert.ThrowsAsync<ArgumentException>(async () => await valid.CheckForCompletenessTest(userWithoutPassword));
            var userFull = FactoryUserDto();
            valid.CheckForCompletenessTest(userFull);
        }

        private IValidate FactoryValidate()
        {
            throw new NotImplementedException();
        }

        private UserEditDto FactoryAdminDto()
            => new UserEditDto 
            {
                Login = "Admin",
                Password = "123123123",
                Group = UserGroupCodeEnum.Admin
            };

        private UserEditDto FactoryUserDto()
            => new UserEditDto
            {
                Login = "User",
                Password = "123123123",
                Group = UserGroupCodeEnum.User
            };

        private User FactoryAdmin()
            => FactoryUser(_adminName, UserGroupCodeEnum.Admin, UserStateCodeEnum.Active);

        private User FactoryUser()
            => FactoryUser("User", UserGroupCodeEnum.User, UserStateCodeEnum.Active);

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
