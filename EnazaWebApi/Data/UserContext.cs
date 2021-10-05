using EnazaWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EnazaWebApi.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

    }
}
