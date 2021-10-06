using EnazaWebApi.Data.Enums;

namespace EnazaWebApi.Logic.Dto
{
    public class UserStateShowDto
    {
        public int UserStateId { get; set; }

        public UserStateCodeEnum Code { get; set; }

        public string Description { get; set; }
    }
}
