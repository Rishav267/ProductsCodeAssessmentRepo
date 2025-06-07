using ProductsCodeAssessmentRepo.Models;

namespace ProductsCodeAssessmentRepo.Contract
{
    public interface IProductRepository
    {
        Task<int> CreateProduct(ProductDTO product);

        Task<IEnumerable<Product>> GetProducts();

        Task<Product?> GetProduct(int id);
        Task<bool> DeleteProduct(int id);
        Task UpdateProduct(int id, Product product);

        Task<int> DecrementStock(int id, int quantity);
        Task<int> IncrementStock(int id, int quantity);
    }
}
