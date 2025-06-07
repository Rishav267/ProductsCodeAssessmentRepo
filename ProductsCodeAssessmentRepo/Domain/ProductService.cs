using ProductsCodeAssessmentRepo.Contract;
using ProductsCodeAssessmentRepo.Models;

namespace ProductsCodeAssessmentRepo.Domain
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> CreateProduct(ProductDTO product)
        {
            return await _productRepository.CreateProduct(product);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productRepository.GetProducts();
        }

        public async Task<Product?> GetProduct(int id)
        {
            return await _productRepository.GetProduct(id);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                return await _productRepository.DeleteProduct(id);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                throw new ArgumentException("Product ID mismatch");
            }
            await _productRepository.UpdateProduct(id, product);
        }

        public async Task<int> DecrementStock(int id, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }
            return await _productRepository.DecrementStock(id, quantity);
        }

        public async Task<int> IncrementStock(int id, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }
            return await _productRepository.IncrementStock(id, quantity);
        }
    }
}
