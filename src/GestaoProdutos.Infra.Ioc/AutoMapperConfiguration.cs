using GestaoProdutos.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GestaoProdutos.Infra.Ioc
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile));
        }
    }

}
