using Microsoft.EntityFrameworkCore;
using TestAban.Entidades;

namespace TestAban
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
