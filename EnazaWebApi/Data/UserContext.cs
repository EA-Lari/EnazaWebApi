using EnazaWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EnazaWebApi.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGroup> Groups { get; set; }

        public DbSet<UserState> States { get; set; }

    }
}
