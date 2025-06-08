using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsCodeAssessmentRepo.Contract;
using ProductsCodeAssessmentRepo.Domain.Exceptions;
using ProductsCodeAssessmentRepo.Models;

namespace ProductsCodeAssessmentRepo.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        private readonly ILogger<ProductRepository> _logger;
        private readonly IMapper _mapper;

        public ProductRepository(ProductDbContext context, ILogger<ProductRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> CreateProduct(ProductDTO productdto)
        {
            try
            {
                if (productdto.Price < 0)
                {
                    _logger.LogWarning("Product creation failed: Price cannot be negative.");
                    throw new ArgumentException("Product price cannot be negative.");
                }
                if (productdto.Stock < 0)
                {
                    _logger.LogWarning("Product creation failed: Stock cannot be negative.");
                    throw new ArgumentException("Product stock cannot be negative.");
                }

                var product = _mapper.Map<Product>(productdto);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product.Id;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while creating product.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating product.");
                throw;
            }
        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                if (products == null || products.Count == 0)
                {
                    _logger.LogInformation("No products found in the database.");
                    return Enumerable.Empty<Product>();
                }
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products from the database.");
                throw;
            }
        }


        public async Task<Product?> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);
                    throw new ProductNotFoundException(id);
                }
                return product;
            }
            catch (ProductNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
                throw;
            }
        }


        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);
                    throw new ProductNotFoundException(id);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ProductNotFoundException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error deleting product ID {ProductId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product ID {ProductId}", id);
                throw;
            }
        }


        public async Task UpdateProduct(int id, Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    _logger.LogWarning("Product ID mismatch: route ID {RouteId}, product ID {ProductId}", id, product.Id);
                    throw new ArgumentException("Product ID mismatch.");
                }

                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);
                    throw new ProductNotFoundException(id);
                }

                // Update only allowed fields
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating product ID {ProductId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product ID {ProductId}", id);
                throw;
            }
        }


        public async Task<int> DecrementStock(int id, int quantity)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);
                    throw new ProductNotFoundException(id);
                }

                if (product.Stock < quantity)
                {
                    _logger.LogWarning("Insufficient stock for product ID {ProductId}. Requested: {Quantity}, Available: {Stock}", id, quantity, product.Stock);
                    throw new InsufficientStockException(id, quantity, product.Stock);
                }

                product.Stock -= quantity;
                await _context.SaveChangesAsync();

                return product.Stock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decrementing stock for product ID {ProductId}", id);
                throw;
            }
        }



        public async Task<int> IncrementStock(int id, int quantity)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);
                    throw new ProductNotFoundException(id);
                }

                product.Stock += quantity;
                await _context.SaveChangesAsync();

                return product.Stock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing stock for product ID {ProductId}", id);
                throw;
            }
        }
    }
}
