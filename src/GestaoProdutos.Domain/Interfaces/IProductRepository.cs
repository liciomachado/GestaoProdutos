using GestaoProdutos.Domain.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoProdutos.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByIdAsync(long id);
        void Save(Product product);
        void Update(Product product);
        Task<Supplier> GetSupplierByCnpj(long id);
        Task<List<Product>> GetByFilter(string description, DateTime? startDateCreated, DateTime? finishDateCreated, DateTime? startDateValid, DateTime? finishDateValid, int size, int page);
    }
}
