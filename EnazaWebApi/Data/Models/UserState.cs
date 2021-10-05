using EnazaWebApi.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnazaWebApi.Data.Models
{
    public class UserState
    {
        public int UserStateId { get; set; }

        public UserStateCodeEnum Code { get; set; }

        public string Description { get; set; }
    }

    public class UserStateConfiguration : IEntityTypeConfiguration<UserState>
    {
        public void Configure(EntityTypeBuilder<UserState> builder)
        {
            builder.HasKey(x => x.UserStateId);
        }
    }
}
