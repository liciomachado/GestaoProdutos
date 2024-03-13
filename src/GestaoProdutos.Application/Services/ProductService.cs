using AutoMapper;
using GestaoProdutos.Application.Dtos;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Application.ViewModels;
using GestaoProdutos.Domain;
using GestaoProdutos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoProdutos.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductResponseViewModel>> GetByFilters(FilterProductDTO filter)
        {
            var response = await _productRepository.GetByFilter(filter.Description, filter.StartDateCreated, filter.FinishDateCreated,
                filter.StartDateValid, filter.FinishDateValid, filter.Size, filter.Page);

            var productsMapped = _mapper.Map<List<ProductResponseViewModel>>(response);
            return productsMapped;
        }

        public async Task<ProductResponseViewModel> GetById(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var mapped = _mapper.Map<ProductResponseViewModel>(product);
            return mapped;
        }

        public async Task InactiveProduct(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) throw new ApplicationException("O id do produto não é valido");
            product.Inactive();
            _productRepository.Update(product);
            await _productRepository.UnitOfWork.Commit();
        }

        public async Task<ProductResponseViewModel> SaveNewProduct(ProductRequestViewModel productDTO)
        {
            Supplier supplier = await GetNewOrUpdateSupplier(productDTO);
            var product = new Product(productDTO.Description, productDTO.DateCreated, productDTO.DateValid, supplier);
            _productRepository.Save(product);
            await _productRepository.UnitOfWork.Commit();
            var produtMapped = _mapper.Map<ProductResponseViewModel>(product);
            return produtMapped;
        }

        private async Task<Supplier> GetNewOrUpdateSupplier(ProductRequestViewModel productDTO)
        {
            Supplier supplier = new(productDTO.Supplier.Description, productDTO.Supplier.CNPJ);
            var supplierExistent = await _productRepository.GetSupplierByCnpj(supplier.Cnpj.Number);
            if (supplierExistent is not null) supplier = supplierExistent;
            supplier.ChangeDescription(productDTO.Supplier.Description);
            return supplier;
        }

        public async Task<ProductResponseViewModel> UpdateProduct(long id, ProductRequestUpdateViewModel productDTO)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) throw new ApplicationException("O id do produto não é valido");
            product.UpdateValues(productDTO.Description, productDTO.DateCreated, productDTO.DateValid);
            _productRepository.Update(product);
            await _productRepository.UnitOfWork.Commit();
            var produtMapped = _mapper.Map<ProductResponseViewModel>(product);
            return produtMapped;
        }
    }
}
