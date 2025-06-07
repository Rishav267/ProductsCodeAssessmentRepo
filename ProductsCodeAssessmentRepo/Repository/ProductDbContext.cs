using ProductsCodeAssessmentRepo.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsCodeAssessmentRepo.Repository
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ProductDb;Trusted_Connection=True;");
            }
        }
    }
}
