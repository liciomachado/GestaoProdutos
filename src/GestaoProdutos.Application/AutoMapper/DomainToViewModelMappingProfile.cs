using AutoMapper;
using GestaoProdutos.Application.ViewModels;
using GestaoProdutos.Domain;

namespace GestaoProdutos.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Product, ProductResponseViewModel>().ReverseMap();
            CreateMap<Supplier, SupplierResponseViewModel>()
                .ForMember(dest => dest.CNPJ, map => map.MapFrom(src => src.Cnpj.Number))
                .ReverseMap();
        }
    }
}
