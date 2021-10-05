using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EnazaWebApi.Data.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime CreateDate { get; set; }

        public int UserGroupId { get; set; }

        public int UserStateId { get; set; }

        public UserGroup Group { get; set; }

        public UserState State { get; set; }
    }

    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.UserGroupId);
            builder.HasOne(x => x.State)
                .WithMany()
                .HasForeignKey(x => x.UserStateId);
        }
    }
}
