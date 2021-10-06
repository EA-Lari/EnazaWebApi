using System;

namespace EnazaWebApi.Logic.Dto
{
    public class UserShowDto
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime CreateDate { get; set; }

        public UserGroupShowDto Group { get; set; }

        public UserStateShowDto State { get; set; }
    }
}
