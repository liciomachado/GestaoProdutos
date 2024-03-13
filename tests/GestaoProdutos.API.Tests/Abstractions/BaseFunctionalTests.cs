using System.Net.Http;
using Xunit;

namespace GestaoProdutos.API.Tests.Abstractions
{
    public class BaseFunctionalTests : IClassFixture<FunctionalTestWebAppFactory>
    {
        protected HttpClient HttpClient { get; init; }

        public BaseFunctionalTests(FunctionalTestWebAppFactory webAppFactory)
        {
            HttpClient = webAppFactory.CreateClient();
        }
    }
}
