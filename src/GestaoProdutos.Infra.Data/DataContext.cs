using GestaoProdutos.Domain;
using GestaoProdutos.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestaoProdutos.Infra.Data
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>(e => e.OwnsOne(x => x.Cnpj));
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
