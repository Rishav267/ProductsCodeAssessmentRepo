using Microsoft.AspNetCore.Mvc;
using ProductsCodeAssessmentRepo.Contract;
using ProductsCodeAssessmentRepo.Models;

namespace ProductsCodeAssessmentRepo.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// Create new product entry in database.
        /// </summary>
        /// <param name="product"></param>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductDTO product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }

            var createdProduct = await _productService.CreateProduct(product);
            return Ok(createdProduct);
        }

        /// <summary>
        /// Returns a list of all products in the database.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts().ConfigureAwait(false);
            return Ok(products);
        }

        /// <summary>
        /// Return product by id from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id).ConfigureAwait(false);
            return Ok(product);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var res = await _productService.DeleteProduct(id);
            return Ok(res);
        }

        /// <summary>
        /// Update an existing product in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            await _productService.UpdateProduct(id, product);
            return Ok();
        }

        /// <summary>
        /// Decrement the stock of a product by a specified quantity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        // PUT /api/products/decrement-stock/{id}/{quantity}
        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStock(int id, int quantity)
        {
            var result = await _productService.DecrementStock(id, quantity);
            return Ok(result);
        }

        /// <summary>
        /// Increase the stock of a product by a specified quantity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        // PUT /api/products/add-to-stock/{id}/{quantity}
        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> AddToStock(int id, int quantity)
        {
            var result = await _productService.IncrementStock(id, quantity);
            return Ok(result);
        }
    }
}
