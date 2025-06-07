using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsCodeAssessmentRepo.Domain.Exceptions;
using ProductsCodeAssessmentRepo.Models;
using ProductsCodeAssessmentRepo.Repository;

namespace ProductsCodeAssessmentRepo.UnitTests.UnitTests.ProductRepositoryTests
{
    public class ProductRepositoryTest
    {
        private Mock<IMapper>? _mapperMock;
        private ProductDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ProductDbContext(options);
        }

        private ProductRepository GetRepository(ProductDbContext context)
        {
            var loggerMock = new Mock<ILogger<ProductRepository>>();
            _mapperMock = new Mock<IMapper>();
            return new ProductRepository(context, loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateProduct_ShouldAddProduct()
        {
            var context = GetInMemoryDbContext();
            var repo = GetRepository(context);

            var product = new ProductDTO { Name = "Test", Price = 10, Stock = 5 };
            _mapperMock?
                .Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                .Returns(new Product { Name = product.Name, Price = product.Price, Stock = product.Stock });
            var result = await repo.CreateProduct(product);

            Assert.Equal(1, result);
            Assert.Single(context.Products);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnAllProducts()
        {
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product { Name = "A", Price = 1, Stock = 1 });
            context.Products.Add(new Product { Name = "B", Price = 2, Stock = 2 });
            context.SaveChanges();

            var repo = GetRepository(context);
            var products = await repo.GetProducts();

            Assert.Equal(2, await context.Products.CountAsync());
            Assert.Collection(products,
                p => Assert.Equal("A", p.Name),
                p => Assert.Equal("B", p.Name));
        }

        [Fact]
        public async Task GetProduct_ShouldReturnProduct_WhenExists()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Test", Price = 10, Stock = 5 };
            context.Products.Add(product);
            context.SaveChanges();

            var repo = GetRepository(context);
            var result = await repo.GetProduct(product.Id);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task GetProduct_ShouldThrow_WhenNotFound()
        {
            var context = GetInMemoryDbContext();
            var repo = GetRepository(context);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => repo.GetProduct(999));
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Test", Price = 10, Stock = 5 };
            context.Products.Add(product);
            context.SaveChanges();

            var repo = GetRepository(context);
            var result = await repo.DeleteProduct(product.Id);

            Assert.True(result);
            Assert.Empty(context.Products);
        }

        [Fact]
        public async Task DeleteProduct_ShouldThrow_WhenNotFound()
        {
            var context = GetInMemoryDbContext();
            var repo = GetRepository(context);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => repo.DeleteProduct(999));
        }

        [Fact]
        public async Task UpdateProduct_ShouldUpdateFields()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Old", Price = 1, Stock = 1 };
            context.Products.Add(product);
            context.SaveChanges();

            var repo = GetRepository(context);
            var updated = new Product { Id = product.Id, Name = "New", Price = 2, Stock = 3, Description = "Desc" };
            await repo.UpdateProduct(product.Id, updated);

            var dbProduct = await context.Products.FindAsync(product.Id);
            Assert.Equal("New", dbProduct.Name);
            Assert.Equal(2, dbProduct.Price);
            Assert.Equal(3, dbProduct.Stock);
            Assert.Equal("Desc", dbProduct.Description);
        }

        [Fact]
        public async Task UpdateProduct_ShouldThrow_WhenIdMismatch()
        {
            var context = GetInMemoryDbContext();
            var repo = GetRepository(context);
            var product = new Product { Id = 1, Name = "Test", Price = 1, Stock = 1 };

            await Assert.ThrowsAsync<ArgumentException>(() => repo.UpdateProduct(2, product));
        }

        [Fact]
        public async Task UpdateProduct_ShouldThrow_WhenNotFound()
        {
            var context = GetInMemoryDbContext();
            var repo = GetRepository(context);
            var product = new Product { Id = 1, Name = "Test", Price = 1, Stock = 1 };

            await Assert.ThrowsAsync<ProductNotFoundException>(() => repo.UpdateProduct(1, product));
        }

        [Fact]
        public async Task DecrementStock_ShouldDecreaseStock()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Test", Price = 10, Stock = 5 };
            context.Products.Add(product);
            context.SaveChanges();

            var repo = GetRepository(context);
            var newStock = await repo.DecrementStock(product.Id, 2);

            Assert.Equal(3, newStock);
        }

        [Fact]
        public async Task DecrementStock_ShouldThrow_WhenInsufficientStock()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Test", Price = 10, Stock = 1 };
            context.Products.Add(product);
            context.SaveChanges();

            var repo = GetRepository(context);

            await Assert.ThrowsAsync<InsufficientStockException>(() => repo.DecrementStock(product.Id, 2));
        }

        [Fact]
        public async Task IncrementStock_ShouldIncreaseStock()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Test", Price = 10, Stock = 1 };
            context.Products.Add(product);
            context.SaveChanges();

            var repo = GetRepository(context);
            var newStock = await repo.IncrementStock(product.Id, 4);

            Assert.Equal(5, newStock);
        }
    }
}
