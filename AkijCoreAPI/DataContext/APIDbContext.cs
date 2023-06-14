using AkijCoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AkijCoreAPI.DataContext
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
