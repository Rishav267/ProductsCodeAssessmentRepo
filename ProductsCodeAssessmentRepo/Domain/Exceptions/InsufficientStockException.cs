namespace ProductsCodeAssessmentRepo.Domain.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(int productId, int requested, int available)
            : base($"Insufficient stock for product ID {productId}. Requested: {requested}, Available: {available}.") { }
    }
}
