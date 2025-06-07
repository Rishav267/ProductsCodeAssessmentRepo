using AutoMapper;
using ProductsCodeAssessmentRepo.Models;

namespace ProductsCodeAssessmentRepo.Domain.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductDTO, Product>();
        }
    }
}
