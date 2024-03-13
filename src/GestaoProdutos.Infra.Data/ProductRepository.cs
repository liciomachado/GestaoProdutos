using GestaoProdutos.Domain;
using GestaoProdutos.Domain.Core;
using GestaoProdutos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoProdutos.Infra.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;
        public void Dispose() => _context.Dispose();

        public async Task<List<Product>> GetByFilter(string description, DateTime? startDateCreated, DateTime? finishDateCreated, DateTime? startDateValid, DateTime? finishDateValid, int size, int page)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(description))
                query = query.Where(p => p.Description.ToLower().Contains(description.ToLower()));

            if (startDateCreated.HasValue)
                query = query.Where(p => p.DateCreated >= startDateCreated);

            if (finishDateCreated.HasValue)
                query = query.Where(p => p.DateCreated <= finishDateCreated);

            if (startDateValid.HasValue)
                query = query.Where(p => p.DateValid >= startDateValid);

            if (finishDateValid.HasValue)
                query = query.Where(p => p.DateValid <= finishDateValid);

            var skip = (page - 1) * size;
            query = query.OrderBy(x => x.Id);
            query = query.Skip(skip).Take(size);
            return await query.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(long id)
        {
            return await _context.Products.Include(a => a.Supplier).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Supplier> GetSupplierByCnpj(long cnpj)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(x => x.Cnpj.Number == cnpj);
        }

        public void Save(Product product)
        {
            _context.Products.Add(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
