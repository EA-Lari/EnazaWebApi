using EnazaWebApi.Data.Enums;
using System;

namespace EnazaWebApi.Logic.Dto
{
    public class UserEditDto
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime CreateDate { get; set; }

        public UserGroupCodeEnum Group { get; set; }
    }
}
