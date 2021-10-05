using EnazaWebApi.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnazaWebApi.Data.Models
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }

        public UserGroupCodeEnum Code { get; set; }

        public string Description { get; set; }
    }

    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(x => x.UserGroupId);
        }
    }

}
