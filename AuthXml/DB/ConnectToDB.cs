using Microsoft.EntityFrameworkCore;
using AuthXml.Models;

namespace AuthXml.DB
{
    public class ConnectToDB : DbContext
    {
        public ConnectToDB(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
