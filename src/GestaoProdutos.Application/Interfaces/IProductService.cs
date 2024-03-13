using GestaoProdutos.Application.Dtos;
using GestaoProdutos.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoProdutos.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseViewModel> GetById(long id);
        Task<ProductResponseViewModel> SaveNewProduct(ProductRequestViewModel productDTO);
        Task<ProductResponseViewModel> UpdateProduct(long id, ProductRequestUpdateViewModel productDTO);
        Task InactiveProduct(long id);
        Task<List<ProductResponseViewModel>> GetByFilters(FilterProductDTO filter);
    }
}
