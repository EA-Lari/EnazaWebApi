using EnazaWebApi.Data.Enums;

namespace EnazaWebApi.Logic.Dto
{
    public class UserGroupShowDto
    {
        public int UserGroupId { get; set; }

        public UserGroupCodeEnum Code { get; set; }

        public string Description { get; set; }
    }
}
