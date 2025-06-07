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
    }
}
