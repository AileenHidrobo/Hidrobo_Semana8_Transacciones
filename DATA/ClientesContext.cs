using Microsoft.EntityFrameworkCore;
using Hidrobo_Semana8.Models;

namespace Hidrobo_Semana8.DATA
{
    public class ClientesContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;database=semana8_clientesdb;user=root;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
