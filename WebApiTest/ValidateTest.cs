using EnazaWebApi.Data.Enums;
using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WebApiTest
{
    public class ValidateTest
    {
        private readonly string _adminName = "Admin";
        [Fact]
        public void CheckStateTest()
        {
            var valid = FactoryValidate();
            var listAdmin = new List<User>();
            throw new NotImplementedException();
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

        private IValidate FactoryValidate()
        {
            throw new NotImplementedException();
        }

        private UserEditDto FactoryUserDto()
            => new UserEditDto
            {

            };

        private User FactoryAdmin()
            => FactoryUser(_adminName, UserGroupCodeEnum.Admin, UserStateCodeEnum.Active);

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
