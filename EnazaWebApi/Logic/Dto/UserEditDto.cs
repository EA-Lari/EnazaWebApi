using EnazaWebApi.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnazaWebApi.Logic.Dto
{
    public class UserEditDto
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime CreateDate { get; set; }

        public UserGroupEnum Group { get; set; }

        public UserStateCodeEnum State { get; set; }
    }
}
